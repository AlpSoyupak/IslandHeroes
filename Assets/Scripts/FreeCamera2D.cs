using UnityEngine;
using UnityEngine.InputSystem; // IMPORTANT

public class FreeCamera2D : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // Use the new Input System
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
        }

        transform.Translate(input * speed * Time.deltaTime);
    }
}
