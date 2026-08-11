using UnityEngine;

public class IngredientFall : MonoBehaviour
{
    public float fallSpeed = 3f;
    public IngredientData data;
    public PowerUpData powerUpData;
    private SpriteRenderer sr;
    private PulseEffect pulse;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        pulse = GetComponent<PulseEffect>();
    }

    public void Initialize(IngredientData ingredientData)
    {
        data = ingredientData;
        powerUpData = null;
        sr.sprite = data.sprite;
        fallSpeed = GameManager.Instance.currentRecipe.fallSpeed; 
        if (pulse != null) pulse.enabled = false;
    }

    public void InitializePowerUp(PowerUpData powerUp)
{
    powerUpData = powerUp;
    data = null;
    sr.sprite = powerUp.sprite;
    fallSpeed = GameManager.Instance.currentRecipe.fallSpeed;
    if (pulse != null)
    {
        pulse.enabled = true;
        pulse.pulseColor = powerUp.isNegative ? Color.red : Color.green;
    }
    else
    {
        Debug.Log("PulseEffect component is NULL on this object!");
    }
}

    void Update()
    {
        if (GameManager.Instance.ingredientsFrozen) return;

        float speed = fallSpeed;
        if (data != null && data.isObstacle && GameManager.Instance.obstaclesSlowed)
            speed *= 0.4f;

        transform.Translate(Vector3.down * speed * Time.deltaTime);

         if (transform.position.y < -6f)
    {
        if (data != null && !data.isObstacle)
            GameManager.Instance.RegisterMiss();

        Destroy(gameObject);
    }

    }
}