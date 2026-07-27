using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Drinks/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    public Sprite sprite;
    public bool isObstacle;
}
