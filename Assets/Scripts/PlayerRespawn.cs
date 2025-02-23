using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerRespawn : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            AudioManager.Instance.PlayCharacterRespawnSound();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("RestartLevel"))
        {
            Debug.Log("You died!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            AudioManager.Instance.PlayCharacterRespawnSound();
        }
    }
}
