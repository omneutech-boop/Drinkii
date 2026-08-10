using UnityEngine;
using TMPro;

public class LevelLabelUI : MonoBehaviour
{
    private TextMeshProUGUI label;

    void Awake()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        GameManager.Instance.OnRecipeLoaded += UpdateLabel;
        UpdateLabel();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnRecipeLoaded -= UpdateLabel;
    }

    void UpdateLabel()
    {
        label.text = GameManager.Instance.currentRecipe.levelLabel;
    }
}