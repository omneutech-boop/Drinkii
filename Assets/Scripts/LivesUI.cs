using UnityEngine;
using TMPro;

public class LivesUI : MonoBehaviour
{
    private TextMeshProUGUI livesText;

    void Awake()
    {
        livesText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        GameManager.Instance.OnLivesChanged += UpdateLives;
        UpdateLives();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLivesChanged -= UpdateLives;
    }

    void UpdateLives()
    {
    livesText.text = GameManager.Instance.lives.ToString();
    }
}