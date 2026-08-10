using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DrinkRecipe currentRecipe;
    public int lives = 4;
    public bool gameActive = true;

    public GameObject pauseScreen;
    public bool isPaused = false;

    public GameObject winScreen;
    public GameObject gameOverScreen;
    public GameObject blackHeartOverlay;

    public bool obstaclesDisabled = false;
    public bool obstaclesSlowed = false;
    public bool shieldActive = false;
    public bool ingredientsFrozen = false;
    public bool catchesBlocked = false;
    public bool cupMovementDisabled = false;
    
    public bool moldyTomatoActive = false;
    public int moldySpawnCountMultiplier = 12; // spawns this many at once, or fires this many times faster
    public float moldySpawnIntervalMultiplier = 0.9f; // spawns much faster
    public float moldyMinXOverride = -5f;
    public float moldyMaxXOverride = 5f;

    public event System.Action OnRecipeProgressChanged;
    public event System.Action OnLivesChanged;
    public event System.Action OnRecipeLoaded;
    public event System.Action OnPowerUpInventoryChanged;

    private Dictionary<IngredientData, int> caughtCounts = new Dictionary<IngredientData, int>();
    public Dictionary<PowerUpData, int> powerUpInventory = new Dictionary<PowerUpData, int>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadRecipe(DrinkRecipe newRecipe)
    {
        currentRecipe = newRecipe;
        caughtCounts.Clear();
        lives = 4;
        gameActive = true;

        if (Cup.Instance != null)
        {
            if (newRecipe.cupFillStages.Length > 0)
                Cup.Instance.SetFillSprite(newRecipe.cupFillStages[0]);
            Cup.Instance.SetScale(newRecipe.cupScale);
        }

        OnRecipeLoaded?.Invoke();
        OnRecipeProgressChanged?.Invoke();
        OnLivesChanged?.Invoke();
    }

    public List<IngredientData> GetSpawnableIngredients()
    {
        List<IngredientData> pool = new List<IngredientData>();
        foreach (var req in currentRecipe.requiredIngredients)
        {
            int caught = caughtCounts.ContainsKey(req.ingredient) ? caughtCounts[req.ingredient] : 0;
            if (caught < req.amountNeeded)
                pool.Add(req.ingredient);
        }
        return pool;
    }

    public void CatchIngredient(IngredientData ingredient)
    {
        if (!gameActive) return;

        if (ingredient.isObstacle)
        {
            LoseLife();
            return;
        }

        if (catchesBlocked) return;


        if (!caughtCounts.ContainsKey(ingredient))
            caughtCounts[ingredient] = 0;

        int neededAmount = 0;
        foreach (var req in currentRecipe.requiredIngredients)
            if (req.ingredient == ingredient) neededAmount = req.amountNeeded;

        caughtCounts[ingredient] = Mathf.Min(caughtCounts[ingredient] + 1, neededAmount);

        CheckRecipeComplete();
        UpdateCupVisual();
        OnRecipeProgressChanged?.Invoke();
    }

    public void CatchPowerUp(PowerUpData powerUp)
    {
        if (powerUp.isNegative)
        {
            ApplyEffect(powerUp);
        }
        else
        {
            if (!powerUpInventory.ContainsKey(powerUp))
                powerUpInventory[powerUp] = 0;

            powerUpInventory[powerUp]++;
            Debug.Log("Power-up caught: " + powerUp.powerUpName + " | new count: " + powerUpInventory[powerUp]);
            OnPowerUpInventoryChanged?.Invoke();
        }
    }

    public void ActivatePowerUp(PowerUpData powerUp)
    {
        if (!powerUpInventory.ContainsKey(powerUp) || powerUpInventory[powerUp] <= 0)
            return;

        powerUpInventory[powerUp]--;
        OnPowerUpInventoryChanged?.Invoke();

        ApplyEffect(powerUp);
    }

    void ApplyEffect(PowerUpData powerUp)
    {
        switch (powerUp.type)
        {
            case PowerUpType.ExtraLife:
                lives++;
                OnLivesChanged?.Invoke();
                break;

            case PowerUpType.Baby:
                caughtCounts.Clear();
                if (currentRecipe.cupFillStages.Length > 0)
                    Cup.Instance.SetFillSprite(currentRecipe.cupFillStages[0]);
                OnRecipeProgressChanged?.Invoke();
                break;

            case PowerUpType.Ghost:
                StartCoroutine(FlagRoutine(() => obstaclesDisabled = true, () => obstaclesDisabled = false, powerUp.duration));
                break;

            case PowerUpType.Turtle:
                StartCoroutine(FlagRoutine(() => obstaclesSlowed = true, () => obstaclesSlowed = false, powerUp.duration));
                break;

            case PowerUpType.Shield:
                StartCoroutine(FlagRoutine(() => shieldActive = true, () => shieldActive = false, powerUp.duration));
                break;

            case PowerUpType.Freeze:
                StartCoroutine(FlagRoutine(() => ingredientsFrozen = true, () => ingredientsFrozen = false, powerUp.duration));
                break;

            case PowerUpType.Bubble:
               StartCoroutine(FlagRoutine(() => catchesBlocked = true, () => catchesBlocked = false, powerUp.duration));
               break;

            case PowerUpType.MoldyTomato:
                StartCoroutine(FlagRoutine(() => moldyTomatoActive = true, () => moldyTomatoActive = false, powerUp.duration));
                break;

            case PowerUpType.BlackHeart:
                StartCoroutine(BlackHeartRoutine(powerUp.duration));
                break;

            case PowerUpType.CupFreeze:
                StartCoroutine(FlagRoutine(() => cupMovementDisabled = true, () => cupMovementDisabled = false, powerUp.duration));
                break;
        }
    }

    IEnumerator FlagRoutine(System.Action turnOn, System.Action turnOff, float duration)
    {
        turnOn();
        yield return new WaitForSeconds(duration);
        turnOff();
    }

    IEnumerator BlackHeartRoutine(float duration)
    {
        if (blackHeartOverlay != null) blackHeartOverlay.SetActive(true);
        yield return new WaitForSeconds(duration);
        if (blackHeartOverlay != null) blackHeartOverlay.SetActive(false);
    }

    void LoseLife()
{
    lives--;
    OnLivesChanged?.Invoke();

    if (lives <= 0)
    {
        PowerUpData extraLifeData = FindExtraLifeInInventory();
        if (extraLifeData != null)
        {
            powerUpInventory[extraLifeData]--;
            OnPowerUpInventoryChanged?.Invoke();
            lives = 1;
            OnLivesChanged?.Invoke();
        }
        else
        {
            GameOver();
        }
    }
}

PowerUpData FindExtraLifeInInventory()
{
    foreach (var kvp in powerUpInventory)
    {
        if (kvp.Key.type == PowerUpType.ExtraLife && kvp.Value > 0)
            return kvp.Key;
    }
    return null;
}

    void GameOver()
    {
        gameActive = false;
        if (gameOverScreen != null) gameOverScreen.SetActive(true);
    }

    void CheckRecipeComplete()
    {
        foreach (var req in currentRecipe.requiredIngredients)
        {
            int caught = caughtCounts.ContainsKey(req.ingredient) ? caughtCounts[req.ingredient] : 0;
            if (caught < req.amountNeeded) return;
        }
        gameActive = false;
        if (winScreen != null) winScreen.SetActive(true);
    }

    void UpdateCupVisual()
    {
        int totalNeeded = 0, totalCaught = 0;
        foreach (var req in currentRecipe.requiredIngredients)
        {
            totalNeeded += req.amountNeeded;
            totalCaught += caughtCounts.ContainsKey(req.ingredient) ? caughtCounts[req.ingredient] : 0;
        }
        float percent = (float)totalCaught / totalNeeded;
        int stageIndex = Mathf.Clamp(Mathf.FloorToInt(percent * (currentRecipe.cupFillStages.Length - 1)), 0, currentRecipe.cupFillStages.Length - 1);
        Cup.Instance.SetFillSprite(currentRecipe.cupFillStages[stageIndex]);
    }

    public bool IsIngredientComplete(IngredientData ingredient)
    {
        int neededAmount = 0;
        foreach (var req in currentRecipe.requiredIngredients)
            if (req.ingredient == ingredient) { neededAmount = req.amountNeeded; break; }
        int caught = caughtCounts.ContainsKey(ingredient) ? caughtCounts[ingredient] : 0;
        return caught >= neededAmount;
    }

    public int GetCaughtCount(IngredientData ingredient)
    {
        return caughtCounts.ContainsKey(ingredient) ? caughtCounts[ingredient] : 0;
    }

    public int GetPowerUpCount(PowerUpData powerUp)
    {
        return powerUpInventory.ContainsKey(powerUp) ? powerUpInventory[powerUp] : 0;
    }

    public void RestartLevel()
    {
        gameOverScreen.SetActive(false);
        LoadRecipe(currentRecipe);
    }

    public void GoToNextLevel()
    {
        winScreen.SetActive(false);
        if (currentRecipe.nextRecipe != null)
            LoadRecipe(currentRecipe.nextRecipe);
    }

    public void TogglePause()
{
    isPaused = !isPaused;

    if (isPaused)
    {
        Time.timeScale = 0f;
        if (pauseScreen != null) pauseScreen.SetActive(true);
    }
    else
    {
        Time.timeScale = 1f;
        if (pauseScreen != null) pauseScreen.SetActive(false);
    }
}
}