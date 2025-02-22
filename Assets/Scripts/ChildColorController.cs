using UnityEngine;
using System.Collections;
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
            vfx.enabled = false; 
        }
        else
        {
            vfx.enabled = true; 
        }
    }
    
}
