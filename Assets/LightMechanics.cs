using UnityEngine;
using System.Collections;

public class SpherePlacer : MonoBehaviour
{
    public float detectionRange = 3f;
    public LayerMask platformLayer;
    public Transform redSphere;
    public Transform greenSphere;
    public Transform blueSphere;
    public float throwForce = 10f;
    public float sphereSize = 0.5f;
    public float lookRange = 10f;

    private bool throwMode = false; // Track if the player is in throw mode
    private bool retrieveMode = false; // Track if the player is in retrieve mode
    private bool remoteRetrieveMode = false; // Track if the player is in remote retrieve mode

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleThrowMode();
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    ToggleRetrieveMode();
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRemoteRetrieveMode();
        }

        if (throwMode)
        {
            retrieveMode = false;
            remoteRetrieveMode = false;
            HandleThrowInput();
        }
        //else if (retrieveMode)
        //{
        //    throwMode = false;
        //    remoteRetrieveMode = false;
        //    HandleRetrieveInput();
        //}
        else if (remoteRetrieveMode)
        {
            throwMode= false;
            retrieveMode = false;
            HandleRemoteRetrieveInput();
        }
        else
        {
            HandlePlacementInput();
        }
    }

    void ToggleThrowMode()
    {
        throwMode = !throwMode;
        Debug.Log($"Throw mode: {throwMode}");
    }

    void ToggleRetrieveMode()
    {
        retrieveMode = !retrieveMode;
        Debug.Log($"Retrieve mode: {retrieveMode}");
    }

    void ToggleRemoteRetrieveMode()
    {
        remoteRetrieveMode = !remoteRetrieveMode;
        Debug.Log($"Remote retrieve mode: {remoteRetrieveMode}");
    }

    void HandleThrowInput()
    {
        // Check for throw input (1, 2, or 3)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ThrowSphere(redSphere);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ThrowSphere(greenSphere);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ThrowSphere(blueSphere);
        }
    }

    void HandleRetrieveInput()
    {
        // Check for retrieve input (1, 2, or 3)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RetrieveSphere("Red");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RetrieveSphere("Green");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RetrieveSphere("Blue");
        }
    }

    void HandleRemoteRetrieveInput()
    {
        // Check for remote retrieve input (1, 2, or 3)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RemoteRetrieveSphere("Red");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RemoteRetrieveSphere("Green");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RemoteRetrieveSphere("Blue");
        }
    }

    void HandlePlacementInput()
    {
        // Check for placement input (1, 2, or 3)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlaceSphere(redSphere);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlaceSphere(greenSphere);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlaceSphere(blueSphere);
        }
    }

    void ThrowSphere(Transform sphere)
    {
        if (sphere.parent != transform)
        {
            Debug.LogWarning($"{sphere.tag} sphere is already placed.");
            return;
        }

        if (IsLookingAtPlatform(out Vector3 lookDirection, out Transform platform))
        {
            sphere.SetParent(null);

            Vector3 targetPosition = platform.position - new Vector3(0, platform.localScale.y / 2 + sphere.localScale.y / 2 - 2, 0);

            StartCoroutine(MoveSphereToPosition(sphere, targetPosition));

            sphere.SetParent(platform);
            Debug.Log($"Threw {sphere.tag} sphere towards {platform.name}.");
        }
        else
        {
            Debug.Log("Not looking at a platform.");
        }
    }

    IEnumerator MoveSphereToPosition(Transform sphere, Vector3 targetPosition)
    {
        Rigidbody sphereRb = sphere.GetComponent<Rigidbody>();
        if (sphereRb == null)
        {
            sphereRb = sphere.gameObject.AddComponent<Rigidbody>();
        }

        sphereRb.useGravity = false;

        float speed = 10f;
        while (Vector3.Distance(sphere.position, targetPosition) > 0.1f)
        {
            sphereRb.velocity = (targetPosition - sphere.position).normalized * speed;
            yield return null;
        }

        sphereRb.velocity = Vector3.zero;
        Debug.Log($"{sphere.tag} sphere reached the target position.");
    }

    void PlaceSphere(Transform sphere)
    {
        if (sphere.parent != transform)
        {
            Debug.LogWarning($"{sphere.tag} sphere is already placed.");
            return;
        }

        Collider[] nearbyPlatforms = Physics.OverlapSphere(transform.position, detectionRange, platformLayer);

        if (nearbyPlatforms.Length > 0)
        {
            Transform platform = nearbyPlatforms[0].transform;

            PlaceSphereBelowPlatform(sphere, platform);

            sphere.SetParent(platform);

            Debug.Log($"Placed {sphere.tag} sphere below {platform.tag} platform.");
        }
        else
        {
            Debug.Log("No nearby platform found to place the sphere.");
        }
    }

    void RetrieveSphere(string sphereTag)
    {
        Collider[] nearbyPlatforms = Physics.OverlapSphere(transform.position, detectionRange, platformLayer);

        foreach (Collider platform in nearbyPlatforms)
        {
            if (platform.transform.childCount > 0)
            {
                Transform sphere = null;
                foreach (Transform child in platform.transform)
                {
                    if (child.CompareTag(sphereTag))
                    {
                        sphere = child;
                        break;
                    }
                }

                if (sphere != null)
                {
                    Rigidbody sphereRb = sphere.GetComponent<Rigidbody>();
                    if (sphereRb != null)
                    {
                        Destroy(sphereRb);
                        Debug.Log($"Removed Rigidbody from {sphere.tag} sphere.");
                    }

                    sphere.SetParent(transform);

                    sphere.localPosition = Vector3.zero;

                    Debug.Log($"Retrieved {sphere.tag} sphere from {platform.tag} platform.");
                    break; 
                }
            }
        }
    }

    void RemoteRetrieveSphere(string sphereTag)
    {
        
        if (IsLookingAtPlatform(out Vector3 lookDirection, out Transform platform))
        {
            if (platform.childCount > 0)
            {
                Transform sphere = null;
                foreach (Transform child in platform)
                {
                    if (child.CompareTag(sphereTag))
                    {
                        sphere = child;
                        break;
                    }
                }

                if (sphere != null)
                {
                    Rigidbody sphereRb = sphere.GetComponent<Rigidbody>();
                    if (sphereRb != null)
                    {
                        Destroy(sphereRb);
                        Debug.Log($"Removed Rigidbody from {sphere.tag} sphere.");
                    }

                    sphere.SetParent(transform);

                    sphere.localPosition = Vector3.zero;

                    Debug.Log($"Remotely retrieved {sphere.tag} sphere from {platform.tag} platform.");
                }
                else
                {
                    Debug.Log($"No {sphereTag} sphere found on the platform.");
                }
            }
            else
            {
                Debug.Log("No spheres found on the platform.");
            }
        }
        else
        {
            Debug.Log("Not looking at a platform.");
        }
    }

    void PlaceSphereBelowPlatform(Transform sphere, Transform platform)
    {
        Vector3 positionBelowPlatform = platform.position - new Vector3(0, platform.localScale.y / 2 + sphere.localScale.y / 2 - 2, 0);

        sphere.position = positionBelowPlatform;
    }

    bool IsLookingAtPlatform(out Vector3 lookDirection, out Transform platform)
    {
        Vector3 raycastOrigin = transform.position + Vector3.down * 0.5f;

        lookDirection = transform.forward;

        if (Physics.Raycast(raycastOrigin, lookDirection, out RaycastHit hit, lookRange, platformLayer))
        {
            platform = hit.transform;
            Debug.DrawLine(raycastOrigin, hit.point, Color.green, 1f);
            return true;
        }

        platform = null;
        Debug.DrawLine(raycastOrigin, raycastOrigin + lookDirection * lookRange, Color.red, 1f);
        return false;
    }
}