using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simplemovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f; // Speed of movement

    void Update()
    {
        // Get input from the player
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        // Calculate movement direction
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed * Time.deltaTime;

        // Apply movement to the GameObject
        transform.Translate(movement);
    }
}
