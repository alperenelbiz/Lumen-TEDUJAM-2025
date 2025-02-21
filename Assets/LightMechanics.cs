using UnityEngine;

public class SpherePlacer : MonoBehaviour
{
    public float detectionRange = 3f; 
    public LayerMask platformLayer; 
    public Transform blueSphere; 
    public Transform redSphere; 
    public Transform greenSphere; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            CheckAndPlaceSphere();
        }
    }

    void CheckAndPlaceSphere()
    {
        
        Collider[] nearbyPlatforms = Physics.OverlapSphere(transform.position, detectionRange, platformLayer);

        foreach (Collider platform in nearbyPlatforms)
        {
            
            string platformTag = platform.tag;

            
            if (platformTag == "Blue" || platformTag == "Red" || platformTag == "Green")
            {
                
                Transform sphereToPlace = GetSphereByTag(platformTag);

                if (sphereToPlace != null)
                {
                   
                    PlaceSphereBelowPlatform(sphereToPlace, platform.transform);

                    
                    sphereToPlace.SetParent(platform.transform);
                }
                else
                {
                    Debug.LogWarning($"No sphere found with tag: {platformTag}");
                }
            }
        }
    }

    Transform GetSphereByTag(string tag)
    {
        
        switch (tag)
        {
            case "Blue":
                return blueSphere;
            case "Red":
                return redSphere;
            case "Green":
                return greenSphere;
            default:
                return null;
        }
    }

    void PlaceSphereBelowPlatform(Transform sphere, Transform platform)
    {
        
        Vector3 positionBelowPlatform = platform.position - new Vector3(0, platform.localScale.y / 2 + sphere.localScale.y / 2 - 2, 0);

        
        sphere.position = positionBelowPlatform;

        Debug.Log($"Placed {sphere.tag} sphere below {platform.tag} platform and reparented it.");
    }
}