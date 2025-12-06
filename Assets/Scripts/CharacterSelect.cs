using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect selectedCharacter;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 worldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                CharacterSelect c = hit.collider.GetComponent<CharacterSelect>();
                if (c != null)
                {
                    selectedCharacter = c;
                    Debug.Log("Selected: " + c.name);
                }
            }
        }
    }
}
