using System;
using System.Collections;
using UnityEngine;

public class ReSpawnManager : SingletonBehaviour<ReSpawnManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 1f;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("Player object with tag 'Player' not found!");
            }
        }
        if (respawnPoint == null)
        {
            GameObject respawnPointObject = GameObject.FindGameObjectWithTag("SpawnPoint");
            if (respawnPointObject != null)
            {
                respawnPoint = respawnPointObject.transform;
            }
            else
            {
                Debug.LogError("Respawn point object with tag 'SpawnPoint' not found!");
            }
        }
    }

    public void ReSpawnPlayer()
    {
        if (player != null && respawnPoint != null)
        {
            StartCoroutine(ReSpawnPlayerCoroutine());
        }
        else
        {
            Debug.LogError("Cannot respawn player: Player or RespawnPoint is not set!");
        }
    }

    private IEnumerator ReSpawnPlayerCoroutine()
    {
        // Shrink effect
        yield return StartCoroutine(ShrinkPlayer());

        player.gameObject.SetActive(false);
        player.position = respawnPoint.position;
        yield return new WaitForSeconds(respawnDelay);
        player.gameObject.SetActive(true);
        
        Camera.main.transform.position = player.position + new Vector3(1, 6, -6);

        // Expand effect
        yield return StartCoroutine(ExpandPlayer());
    }

    private IEnumerator ShrinkPlayer()
    {
        float elapsedTime = 0;
        Vector3 originalScale = player.localScale;
        while (elapsedTime < 0.5f)
        {
            player.localScale = Vector3.Lerp(originalScale, Vector3.zero, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.localScale = Vector3.zero;
    }

    private IEnumerator ExpandPlayer()
    {
        float elapsedTime = 0;
        Vector3 originalScale = new Vector3(1, 1, 1);
        while (elapsedTime < 0.5f)
        {
            player.localScale = Vector3.Lerp(Vector3.zero, originalScale, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.localScale = originalScale;
    }

}