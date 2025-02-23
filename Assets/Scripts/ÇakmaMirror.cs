using UnityEngine;
using System.Collections;

public class SphereReflector : MonoBehaviour
{
    public Transform targetPlatform; 
    public float reflectionSpeed = 10f;

    private void OnCollisionEnter(Collision collision)
    {
      
        if (collision.gameObject.CompareTag("Red") || collision.gameObject.CompareTag("Green") || collision.gameObject.CompareTag("Blue"))
        {
            
            Transform sphere = collision.transform;
            Rigidbody sphereRb = sphere.GetComponent<Rigidbody>();

            Vector3 direction = targetPlatform.position - new Vector3(0, targetPlatform.localScale.y / 2 + sphere.localScale.y / 2 - 2, 0);

            sphere.position= direction;
            sphere.SetParent(targetPlatform);
            sphereRb.isKinematic=true;
            
            //StartCoroutine(MoveSphereToTarget(sphere, direction));
        }

    }

    IEnumerator MoveSphereToTarget(Transform sphere, Vector3 direction)
    {
        
        
        while (Vector3.Distance(sphere.position, direction) > 0.1f)
        {
            sphere.position += direction * reflectionSpeed * Time.deltaTime;
            yield return null;
        }

        
        

        Debug.Log($"Sphere reached the target platform: {targetPlatform.name}");
    }
}