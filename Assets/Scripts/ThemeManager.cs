using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    public SpriteRenderer wallRenderer;
    public UnityEngine.UI.Image powerUpPanelImage;
    public UnityEngine.UI.Image ingredientPanelImage;

    public BackgroundTheme defaultTheme;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ApplyTheme(defaultTheme);
    }

    public void ApplyTheme(BackgroundTheme theme)
    {
        if (wallRenderer != null) wallRenderer.sprite = theme.wallSprite;
        if (powerUpPanelImage != null) powerUpPanelImage.sprite = theme.powerUpPanelSprite;
        if (ingredientPanelImage != null) ingredientPanelImage.sprite = theme.ingredientPanelSprite;
    }
}