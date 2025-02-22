using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GateMovement : MonoBehaviour
{
    [System.Serializable]
    public class PlatformGatePair
    {
        public Transform platform; // The platform to check
        public GameObject gate;   // The gate connected to this platform
        public string requiredTag; // The required tag for the sphere (e.g., "Red", "Green", "Blue")
        public bool isOpen = false; // Is the gate open?
    }

    public List<PlatformGatePair> platformGatePairs; // List of platform-gate pairs
    [SerializeField] private float moveDuration = 1.5f; 
    [SerializeField] private float moveDistance = 5f;


    void Update()
    {
        // Check all platform-gate pairs
        foreach (var pair in platformGatePairs)
        {
            // Check if the platform has the correct sphere
            bool isCorrectSpherePlaced = CheckPlatformForCorrectSphere(pair.platform, pair.requiredTag);

            // Open or close the gate based on the result
            if (isCorrectSpherePlaced && !pair.isOpen)
            {
                OpenGate(pair);
            }
            else if(!isCorrectSpherePlaced && pair.isOpen)
            {
                CloseGate(pair);
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

    void OpenGate(PlatformGatePair pair)
    {
        if (pair.gate == null) return;

        pair.isOpen = true; // ✅ Now correctly modifies isOpen state
        
        pair.gate.transform.DOMoveY(pair.gate.transform.position.y - moveDistance, moveDuration)
            .SetEase(Ease.InOutQuad);

        Debug.Log($"Gate {pair.gate.name} opened.");
    }

    void CloseGate(PlatformGatePair pair)
    {
        if (pair.gate == null) return;

        pair.isOpen = false; // ✅ Now correctly modifies isOpen state

        
        pair.gate.transform.DOMoveY(pair.gate.transform.position.y + moveDistance, moveDuration)
            .SetEase(Ease.InOutQuad);

        Debug.Log($"Gate {pair.gate.name} closed.");
    }

}
