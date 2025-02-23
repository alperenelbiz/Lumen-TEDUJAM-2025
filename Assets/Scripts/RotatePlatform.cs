using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatePlatform : MonoBehaviour
{
    [System.Serializable]
    public class PlatformPair
    {
        public Transform platform; // The platform to check
        public List<Transform> rotatePlatforms; // The platforms that will rotate
        public List<string> requiredTags; // List of required tags for the spheres
        public bool hasRotated = false; // Ensures rotation happens only once
        public bool isRotating = false; // Is the rotation in progress?
    }

    public List<PlatformPair> platformGatePairs; // List of platform-gate pairs
    [SerializeField] private float rotateDuration = 1.5f;

    void Update()
    {
        foreach (var pair in platformGatePairs)
        {
            bool isCorrectSpherePlaced = CheckPlatformForCorrectSphere(pair.platform, pair.requiredTags);

            if (isCorrectSpherePlaced && !pair.isRotating && !pair.hasRotated)
            {
                PlatformRotation(pair);
            }
            else if(!isCorrectSpherePlaced) { pair.hasRotated = false; }
        }
    }

    bool CheckPlatformForCorrectSphere(Transform platform, List<string> requiredTags)
    {
        if (platform.childCount == 0)
        {
            return false; // No objects present
        }

        HashSet<string> foundTags = new HashSet<string>();

        foreach (Transform sphere in platform)
        {
            foundTags.Add(sphere.tag);
        }

        return foundTags.SetEquals(requiredTags); // Ensure exact match
    }

    void PlatformRotation(PlatformPair pair)
    {
        if (pair.rotatePlatforms.Count < 2) return;

        pair.isRotating = true;
        pair.hasRotated = true;

        AudioManager.Instance.PlayPlatformRotateSound();
        Vector3 centerPoint = (pair.rotatePlatforms[0].position + pair.rotatePlatforms[1].position) / 2;
        Vector3 direction0 = pair.rotatePlatforms[0].position - centerPoint;
        Vector3 direction1 = pair.rotatePlatforms[1].position - centerPoint;

        Vector3 targetPos0 = centerPoint + Quaternion.Euler(0, 180, 0) * direction0;
        Vector3 targetPos1 = centerPoint + Quaternion.Euler(0, 180, 0) * direction1;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(pair.rotatePlatforms[0].DOMove(targetPos0, rotateDuration).SetEase(Ease.InOutQuad));
        sequence.Join(pair.rotatePlatforms[1].DOMove(targetPos1, rotateDuration).SetEase(Ease.InOutQuad));
        sequence.OnComplete(() => pair.isRotating = false);
    }
}