using UnityEngine;
using UnityEngine.InputSystem;

public class CupMovement : MonoBehaviour
{
    public float minX = -2f;
    public float maxX = 1.31f;

    void Update()
    {
        Vector2? pointerScreenPos = null;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            pointerScreenPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            pointerScreenPos = Mouse.current.position.ReadValue();
        }

        if (pointerScreenPos.HasValue)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(pointerScreenPos.Value.x, pointerScreenPos.Value.y, Camera.main.nearClipPlane + 10f)
            );

            float clampedX = Mathf.Clamp(worldPos.x, minX, maxX);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }
}