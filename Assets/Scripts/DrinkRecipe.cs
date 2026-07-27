using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RequiredIngredient
{
    public IngredientData ingredient;
    public int amountNeeded;
}

[CreateAssetMenu(fileName = "NewDrinkRecipe", menuName = "Drinks/Recipe")]
public class DrinkRecipe : ScriptableObject
{
    public string drinkName;
    public List<RequiredIngredient> requiredIngredients;
    public Sprite[] cupFillStages; // [0]=0%, [1]=25%, [2]=75%, [3]=100%
    public DrinkRecipe nextRecipe;
    public List<IngredientData> obstaclePool;
    public Vector3 cupScale = Vector3.one; 
    public float powerUpSpawnChance = 0.1f; 

}