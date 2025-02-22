using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChildColorController : MonoBehaviour
{
    [SerializeField] private GameObject player; // The player object
    [SerializeField] private GameObject redObject, blueObject, greenObject; // Child objects

    [SerializeField] private VisualEffect redVFX;
    [SerializeField] private VisualEffect blueVFX;
    [SerializeField] private VisualEffect greenVFX;

    void Update()
    {
        ManageVFXVisibility(redObject, redVFX);
        ManageVFXVisibility(blueObject, blueVFX);
        ManageVFXVisibility(greenObject, greenVFX);
    }

    private void ManageVFXVisibility(GameObject obj, VisualEffect vfx)
    {
        if (obj.transform.parent == player.transform)
        {
            vfx.enabled = false; // Disable VFX if object is a child of the player
        }
        else
        {
            vfx.enabled = true; // Enable VFX if object is not a child of the player
        }
    }
}
