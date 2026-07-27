using UnityEngine;

public enum PowerUpType
{
    Ghost, ExtraLife, Turtle, Shield, Freeze,
    Bubble, Baby, MoldyTomato, BlackHeart, CupFreeze
}

[CreateAssetMenu(fileName = "NewPowerUp", menuName = "Drinks/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public string powerUpName;
    public Sprite sprite;
    public PowerUpType type;
    public bool isNegative;
    public float duration;
}