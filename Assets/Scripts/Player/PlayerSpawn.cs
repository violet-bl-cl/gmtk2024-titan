using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public float PlayerSpawnTime = 0.0f;
    public GameObject PlayerSpawnPrefab;
    private Coroutine _spawnCoroutine;
    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerSpawnPrefab = FindAnyObjectByType<PlayerController>().gameObject;
        _spawnCoroutine = StartCoroutine(nameof(SpawnPlayer));
    }

    IEnumerator SpawnPlayer()
    {
        PlayerSpawnPrefab.SetActive(false);
        yield return new WaitForSeconds(PlayerSpawnTime);
        PlayerSpawnPrefab.SetActive(true);
        StopCoroutine(_spawnCoroutine);
        _spriteRenderer.enabled = false;
        _spawnCoroutine = null;
    }
}
