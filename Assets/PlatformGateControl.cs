using System.Collections.Generic;
using UnityEngine;

public class PlatformGateControl : MonoBehaviour
{
    [System.Serializable]
    public class PlatformGatePair
    {
        public Transform platform; // The platform to check
        public GameObject gate;   // The gate connected to this platform
        public string requiredTag; // The required tag for the sphere (e.g., "Red", "Green", "Blue")
    }

    public List<PlatformGatePair> platformGatePairs; // List of platform-gate pairs

    void Update()
    {
        // Check all platform-gate pairs
        foreach (var pair in platformGatePairs)
        {
            // Check if the platform has the correct sphere
            bool isCorrectSpherePlaced = CheckPlatformForCorrectSphere(pair.platform, pair.requiredTag);

            // Open or close the gate based on the result
            if (isCorrectSpherePlaced)
            {
                OpenGate(pair.gate);
            }
            else
            {
                CloseGate(pair.gate);
            }
        }
    }

    bool CheckPlatformForCorrectSphere(Transform platform, string requiredTag)
    {
        // Check if the platform has at least one child sphere
        if (platform.childCount == 0)
        {
            return false;
        }

        // Check all child spheres on the platform
        foreach (Transform sphere in platform)
        {
            // If any sphere has the correct tag, return true
            if (sphere.CompareTag(requiredTag))
            {
                return true;
            }
        }

        // No sphere with the correct tag was found
        return false;
    }

    void OpenGate(GameObject gate)
    {
        // Activate the gate (or disable its collider/renderer)
        gate.SetActive(false); // Example: Disable the gate GameObject
        Debug.Log($"Gate {gate.name} opened.");
    }

    void CloseGate(GameObject gate)
    {
        // Deactivate the gate (or enable its collider/renderer)
        gate.SetActive(true); // Example: Enable the gate GameObject
        Debug.Log($"Gate {gate.name} closed.");
    }
}