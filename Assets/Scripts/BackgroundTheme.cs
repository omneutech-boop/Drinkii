using UnityEngine;

[CreateAssetMenu(fileName = "NewBackgroundTheme", menuName = "Drinks/BackgroundTheme")]
public class BackgroundTheme : ScriptableObject
{
    public string themeName;
    public Sprite wallSprite;
    public Sprite powerUpPanelSprite;
    public Sprite ingredientPanelSprite;
}