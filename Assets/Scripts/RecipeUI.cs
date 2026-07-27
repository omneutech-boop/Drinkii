using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecipeUI : MonoBehaviour
{
    public GameObject ingredientRowPrefab;
    public Transform rowContainer;

    private List<GameObject> spawnedRows = new List<GameObject>();
    private List<IngredientData> trackedIngredients = new List<IngredientData>();

    void Start()
    {
         GameManager.Instance.OnRecipeLoaded += HandleRecipeLoaded;
         GameManager.Instance.OnRecipeProgressChanged += RefreshUI;
         BuildRows();
         RefreshUI();
    }

    void OnDestroy()
{
    if (GameManager.Instance != null)
    {
        GameManager.Instance.OnRecipeLoaded -= HandleRecipeLoaded;
        GameManager.Instance.OnRecipeProgressChanged -= RefreshUI;
    }
}

void HandleRecipeLoaded()
{
    BuildRows();
    RefreshUI();
}

    void BuildRows()
    {
        foreach (var row in spawnedRows) Destroy(row);
        spawnedRows.Clear();
        trackedIngredients.Clear();

        foreach (var req in GameManager.Instance.currentRecipe.requiredIngredients)
        {
            GameObject row = Instantiate(ingredientRowPrefab, rowContainer);
            Transform iconTransform = row.transform.Find("IngredientIcon");
            iconTransform.GetComponent<Image>().sprite = req.ingredient.sprite;

            spawnedRows.Add(row);
            trackedIngredients.Add(req.ingredient);
        }
    }

    void RefreshUI()
    {
        for (int i = 0; i < spawnedRows.Count; i++)
        {
            IngredientData ingredient = trackedIngredients[i];
            bool complete = GameManager.Instance.IsIngredientComplete(ingredient);

            Transform overlay = spawnedRows[i].transform.Find("CaughtOverlay");
            overlay.gameObject.SetActive(complete);
        }
    }
}