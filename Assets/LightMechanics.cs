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

    private bool throwMode = false; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleThrowMode();
        }

        if (throwMode)
        {
            if (redSphere.parent == transform && greenSphere.parent == transform && blueSphere.parent == transform)
            {
                throwMode = true;
               
            }
            else throwMode = false;
            HandleThrowInput();
        }
        else
        {
            HandlePlacementInput();
        }
        if (Input.GetKeyUp(KeyCode.R)) { RetrieveSphere(); }
    }

    void ToggleThrowMode()
    {
        if (redSphere.parent == transform && greenSphere.parent == transform && blueSphere.parent == transform)
        {
            throwMode = !throwMode;
            Debug.Log($"Throw mode: {throwMode}");
        }
        else
        {
            Debug.Log("You need all three spheres to enter throw mode.");
        }
    }

    void HandleThrowInput()
    {
        
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

    void HandlePlacementInput()
    {
        
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            RetrieveSphere();
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
            //PlaceSphereBelowPlatform(sphere, platform);

            
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
       // sphereRb.useGravity = true;

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

    void RetrieveSphere()
    {
        Collider[] nearbyPlatforms = Physics.OverlapSphere(transform.position, detectionRange, platformLayer);

        foreach (Collider platform in nearbyPlatforms)
        {
            if (platform.transform.childCount > 0)
            {
                Transform sphere = platform.transform.GetChild(0);

                sphere.SetParent(transform);
                Rigidbody sphereRb = sphere.GetComponent<Rigidbody>();
                if (sphereRb != null)
                {
                    Destroy(sphereRb);

                }
                sphere.localPosition = Vector3.zero;

                Debug.Log($"Retrieved {sphere.tag} sphere from {platform.tag} platform.");
                break; 
            }
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