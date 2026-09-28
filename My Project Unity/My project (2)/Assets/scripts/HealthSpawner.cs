using UnityEngine;
using System.Collections;

// Separate from ObjectSpawner on purpose: this runs on a fixed timer
// (not affected by difficulty ramp-up) since heal pickups should stay rare.
public class HealthSpawner : MonoBehaviour
{
    [Header("Prefab & Spawn Area")]
    public GameObject healthObjectPrefab;
    public float spawnYPosition = 6f;
    public float leftSpawnBound = -8f;
    public float rightSpawnBound = 8f;

    [Header("Timing")]
    public float spawnInterval = 120f; // Fixed: one every 2 minutes

    private bool isSpawning = true;
    private Coroutine spawnRoutine;

    void Start()
    {
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (GameManager.Instance != null && !GameManager.Instance.IsGameActive())
                continue; // Don't spawn once the game has ended

            SpawnHealthObject();
        }
    }

    void SpawnHealthObject()
    {
        if (healthObjectPrefab == null)
        {
            Debug.LogWarning("HealthSpawner: No healthObjectPrefab assigned!");
            return;
        }

        float randomX = Random.Range(leftSpawnBound, rightSpawnBound);
        Vector3 spawnPosition = new Vector3(randomX, spawnYPosition, 0f);

        Instantiate(healthObjectPrefab, spawnPosition, Quaternion.identity);
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
    }
}
