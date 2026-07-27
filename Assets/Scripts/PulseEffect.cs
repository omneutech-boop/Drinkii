using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    public Color pulseColor = Color.green;
    public float pulseSpeed = 2f;
    private SpriteRenderer sr;
    private Color baseColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        sr.color = Color.Lerp(baseColor, pulseColor, t * 0.5f);
    }
}