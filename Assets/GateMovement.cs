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
        public List<string> requiredTags; // ✅ List of required tags for the sphere
        public bool isOpen = false; // Is the gate open?
    }

    public List<PlatformGatePair> platformGatePairs; // List of platform-gate pairs
    [SerializeField] private float moveDuration = 1.5f;
    [SerializeField] private float moveDistance = 5f;

    void Update()
    {
        foreach (var pair in platformGatePairs)
        {
            bool isCorrectSpherePlaced = CheckPlatformForCorrectSphere(pair.platform, pair.requiredTags);

            if (isCorrectSpherePlaced && !pair.isOpen)
            {
                OpenGate(pair);
            }
            else if (!isCorrectSpherePlaced && pair.isOpen)
            {
                CloseGate(pair);
            }
        }
    }

    bool CheckPlatformForCorrectSphere(Transform platform, List<string> requiredTags)
    {
        if (platform.childCount == 0)
        {
            Debug.Log($"❌ No objects found on platform {platform.name}");
            return false; // No objects present
        }

        HashSet<string> foundTags = new HashSet<string>();

        // Collect all tags from child objects
        foreach (Transform sphere in platform)
        {
            foundTags.Add(sphere.tag);
        }

        Debug.Log($"🔎 Found tags on {platform.name}: {string.Join(", ", foundTags)}");
        Debug.Log($"✅ Required tags: {string.Join(", ", requiredTags)}");

        // Ensure the found tags match the required tags exactly (no extra or missing tags)
        bool isMatch = foundTags.SetEquals(requiredTags);

        if (!isMatch)
        {
            Debug.Log($"❌ Tags do not match exactly for {platform.name}");
        }
        else
        {
            Debug.Log($"✅ Correct tags placed for {platform.name}, gate should open.");
        }

        return isMatch;
    }

    void OpenGate(PlatformGatePair pair)
    {
        if (pair.gate == null) return;

        pair.isOpen = true;

        pair.gate.transform.DOMoveY(pair.gate.transform.position.y - moveDistance, moveDuration)
            .SetEase(Ease.InOutQuad);

        Debug.Log($"🚪 Gate {pair.gate.name} opened.");
    }

    void CloseGate(PlatformGatePair pair)
    {
        if (pair.gate == null) return;

        pair.isOpen = false;

        pair.gate.transform.DOMoveY(pair.gate.transform.position.y + moveDistance, moveDuration)
            .SetEase(Ease.InOutQuad);

        Debug.Log($"🚪 Gate {pair.gate.name} closed.");
    }
}
