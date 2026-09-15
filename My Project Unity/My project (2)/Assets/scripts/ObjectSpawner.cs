using UnityEngine;

using System.Collections;

public class ObjectSpawner : MonoBehaviour

{

    [Header("Prefab & Spawn Area")]

    public GameObject[] fallingObjectPrefabs; // Drag in Rock, Triangle, or any other variants here

    public float spawnYPosition = 6f;   // Height above the screen to spawn from

    public float leftSpawnBound = -8f;

    public float rightSpawnBound = 8f;

    [Header("Spawn Rate")]

    public float startingSpawnInterval = 1.5f; // Seconds between spawns at the beginning

    public float minSpawnInterval = 0.3f;      // Fastest it will ever get

    public float difficultyRampTime = 60f;     // Seconds to reach minSpawnInterval

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

            SpawnObject();

            float currentInterval = GetCurrentSpawnInterval();

            yield return new WaitForSeconds(currentInterval);

        }

    }

    float GetCurrentSpawnInterval()

    {

        if (GameManager.Instance == null)

            return startingSpawnInterval;

        float elapsed = GameManager.Instance.GetSurvivalTime();

        float t = Mathf.Clamp01(elapsed / difficultyRampTime);

        // Lerp from starting interval down to minimum interval as time passes

        return Mathf.Lerp(startingSpawnInterval, minSpawnInterval, t);

    }

    void SpawnObject()

    {

        if (fallingObjectPrefabs == null || fallingObjectPrefabs.Length == 0)

        {

            Debug.LogWarning("ObjectSpawner: No fallingObjectPrefabs assigned!");

            return;

        }

        // Pick a random prefab from the list (Rock, Triangle, etc.)

        int randomIndex = Random.Range(0, fallingObjectPrefabs.Length);

        GameObject chosenPrefab = fallingObjectPrefabs[randomIndex];

        float randomX = Random.Range(leftSpawnBound, rightSpawnBound);

        Vector3 spawnPosition = new Vector3(randomX, spawnYPosition, 0f);

        Instantiate(chosenPrefab, spawnPosition, Quaternion.identity);

    }

    public void StopSpawning()

    {

        isSpawning = false;

        if (spawnRoutine != null) StopCoroutine(spawnRoutine);

    }

}
 