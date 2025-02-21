using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GateController : MonoBehaviour
{
    public List<Transform> platforms; 
    public string nextSceneName; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the gate. Checking platforms...");

            if (CheckAllPlatformsHaveSameColorSpheres())
            {
                OpenGate();
            }
            else
            {
                Debug.Log("Not all platforms have the same color spheres. Gate remains closed.");
            }
        }
    }

    bool CheckAllPlatformsHaveSameColorSpheres()
    {
        if (platforms.Count == 0)
        {
            Debug.LogWarning("No platforms assigned in the list.");
            return false;
        }

       

        foreach (Transform platform in platforms)
        {
            
            if (platform.childCount == 0)
            {
                Debug.Log($"Platform {platform.name} has no spheres.");
                return false;
            }

            
            string platformSphereTag = null;

            foreach (Transform sphere in platform)
            {
                
                string sphereTag = sphere.tag;

                if (platformSphereTag == null)
                {
                    platformSphereTag = sphereTag;
                }
                else if (platform.tag != sphere.tag)
                {
                    Debug.Log($"Platform {platform.name} has spheres with different tags: {platformSphereTag} and {sphereTag}.");
                    return false;
                }
            }

           
           
        }

        Debug.Log("All platforms have the same color spheres.");
        return true;
    }

    void OpenGate()
    {
        Debug.Log("All platforms have the same color spheres. Opening gate...");

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set.");
        }
    }
}