using UnityEngine;

public class SpherePlacer : MonoBehaviour
{
    public float detectionRange = 3f; 
    public LayerMask platformLayer; 
    public Transform redSphere; 
    public Transform greenSphere; 
    public Transform blueSphere; 

    void Update()
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
            Debug.LogWarning("No nearby platform found to place the sphere.");
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
}