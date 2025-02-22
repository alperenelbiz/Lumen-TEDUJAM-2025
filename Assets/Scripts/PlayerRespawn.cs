using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("RestartLevel"))
        {
            Debug.Log("You died!");
            ReSpawnManager.Instance.ReSpawnPlayer();
        }
    }
}
