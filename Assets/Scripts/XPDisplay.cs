using UnityEngine;
using TMPro;

public class XPDisplayUI : MonoBehaviour
{
    private TextMeshProUGUI xpText;

    void Awake()
    {
        xpText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        GameManager.Instance.OnXPChanged += UpdateXP;
        UpdateXP();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnXPChanged -= UpdateXP;
    }

    void UpdateXP()
    {
        xpText.text = GameManager.Instance.totalXP.ToString();
    }
}