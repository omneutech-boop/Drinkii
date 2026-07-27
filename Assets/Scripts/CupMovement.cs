using UnityEngine;
using UnityEngine.InputSystem;

public class CupMovement : MonoBehaviour
{
    public float minX = -2.5f;
    public float maxX = 2.5f;

    void Update()
    {
        if (GameManager.Instance.cupMovementDisabled) return;
        Vector2? pointerScreenPos = null;

        // Touch (mobile device)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            pointerScreenPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        // Mouse (Editor testing)
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