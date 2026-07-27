using UnityEngine;

public class Cup : MonoBehaviour
{
    public static Cup Instance;
    private SpriteRenderer sr;

    void Awake()
    {
        Instance = this;
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetFillSprite(Sprite fillSprite)
    {
        sr.sprite = fillSprite;
    }

public void SetScale(Vector3 scale)
{
    transform.localScale = scale;
}

   void OnTriggerEnter2D(Collider2D other)
{
    IngredientFall ingredient = other.GetComponent<IngredientFall>();
    if (ingredient == null) return;

    if (ingredient.powerUpData != null)
    {
        GameManager.Instance.CatchPowerUp(ingredient.powerUpData);
    }
    else if (ingredient.data.isObstacle)
    {
        if (!GameManager.Instance.shieldActive && !GameManager.Instance.obstaclesDisabled)
            GameManager.Instance.CatchIngredient(ingredient.data);
    }
    else
    {
        GameManager.Instance.CatchIngredient(ingredient.data);
    }

    Destroy(ingredient.gameObject);
}
}