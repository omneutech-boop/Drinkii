using UnityEngine;
using System.Collections.Generic;

public class IngredientSpawner : MonoBehaviour
{
    public GameObject ingredientPrefab;
    public GameObject powerUpPrefab;
    public float spawnInterval = 1.5f;
    public float powerUpSpawnChance = 0.1f;
    public float minX = -3f;
    public float maxX = 3f;
    public float spawnY = 6f;

    private float timer;

    void Update()
    {
        if (!GameManager.Instance.gameActive) return;
        if (GameManager.Instance.ingredientsFrozen) return;

        float interval = spawnInterval;
        if (GameManager.Instance.moldyTomatoActive)
            interval /= GameManager.Instance.moldySpawnCountMultiplier;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            SpawnSomething();
        }
    }

    void SpawnSomething()
    {
        if (GameManager.Instance.moldyTomatoActive)
        {
            SpawnObstacleChaos();
            return;
        }

        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        if (Random.value < powerUpSpawnChance)
        {
            List<PowerUpData> combinedPool = new List<PowerUpData>();
            combinedPool.AddRange(GameManager.Instance.currentRecipe.powerUpPool);
            combinedPool.AddRange(GameManager.Instance.currentRecipe.powerDownPool);

            if (combinedPool.Count > 0)
            {
                PowerUpData chosen = combinedPool[Random.Range(0, combinedPool.Count)];
                GameObject obj = Instantiate(powerUpPrefab, spawnPos, Quaternion.identity);
                obj.GetComponent<IngredientFall>().InitializePowerUp(chosen);
            }
            return;
        }

        List<IngredientData> pool = new List<IngredientData>(GameManager.Instance.GetSpawnableIngredients());

        if (!GameManager.Instance.obstaclesDisabled)
            pool.AddRange(GameManager.Instance.currentRecipe.obstaclePool);

        if (pool.Count == 0) return;

        IngredientData chosenIngredient = pool[Random.Range(0, pool.Count)];
        GameObject ingObj = Instantiate(ingredientPrefab, spawnPos, Quaternion.identity);
        ingObj.GetComponent<IngredientFall>().Initialize(chosenIngredient);
    }

    void SpawnObstacleChaos()
    {
        if (GameManager.Instance.obstaclesDisabled) return;
        if (GameManager.Instance.currentRecipe.obstaclePool.Count == 0) return;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(spawnY * 0.2f, spawnY * 0.8f);
        Vector3 spawnPos = new Vector3(randomX, randomY, 0f);

        IngredientData chosenObstacle = GameManager.Instance.currentRecipe.obstaclePool[
            Random.Range(0, GameManager.Instance.currentRecipe.obstaclePool.Count)
        ];

        GameObject obj = Instantiate(ingredientPrefab, spawnPos, Quaternion.identity);
        obj.GetComponent<IngredientFall>().Initialize(chosenObstacle);
    }

    void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Vector3 leftPoint = new Vector3(minX, spawnY, 0f);
    Vector3 rightPoint = new Vector3(maxX, spawnY, 0f);
    Gizmos.DrawLine(new Vector3(minX, -6f, 0f), new Vector3(minX, spawnY, 0f));
    Gizmos.DrawLine(new Vector3(maxX, -6f, 0f), new Vector3(maxX, spawnY, 0f));
}
}