using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public Camera mainCamera;
    public float spawnHeight = 2f; 
    public float spawnInterval = 2f; // Tiempo inicial entre spawns
    public float minSpawnInterval = 0.5f; // Límite mínimo de tiempo entre spawns
    public float difficultyIncreaseRate = 0.1f; // Velocidad del aumento de dificultad
    public float trapezoidWidthTop = 4f;
    public float trapezoidWidthBottom = 8f;

    private float _difficultyMultiplier = 1f; // Multiplicador de dificultad

    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseDifficulty());
    }

    IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            _difficultyMultiplier += difficultyIncreaseRate;

            spawnInterval = Mathf.Max(spawnInterval - 0.2f, minSpawnInterval);
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.speed *= _difficultyMultiplier;
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float camWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float camTop = 5f;
        float camBottom = -15f;
        float spawnY;

        int randomChoice = Random.Range(0, 2);

        if(randomChoice == 0)
        {
            spawnY = camTop + spawnHeight;
        } else
        {
            spawnY = camBottom - spawnHeight;
        }
    
        float lerpFactor = Random.Range(0f, 1f);
        float minX = -Mathf.Lerp(trapezoidWidthBottom / 2, trapezoidWidthTop / 2, lerpFactor);
        float maxX = Mathf.Lerp(trapezoidWidthBottom / 2, trapezoidWidthTop / 2, lerpFactor);
        float spawnX = Random.Range(minX, maxX);

        return new Vector3(spawnX, spawnY, 0);
    }
}
