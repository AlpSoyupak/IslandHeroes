using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class ClickToRun : MonoBehaviour
{
    public float speed = 3f;

    private Vector3 targetPosition;
    private Animator animator;
    private bool isMoving = false;
    private CharacterSelect mySelect;
    private Camera cam;
    public Tilemap grassTilemap;

    public void Init(Tilemap grass)
    {
        grassTilemap = grass;
    }

    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        mySelect = GetComponent<CharacterSelect>();
        targetPosition = transform.position;
    }

    void Update()
    {
        // Only move if THIS warrior is selected
        if (CharacterSelect.selectedCharacter != mySelect)
            return;

        // Detect world click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldPos = cam.ScreenToWorldPoint(mousePos);

            targetPosition = worldPos;
            isMoving = true;

            animator.Play("Run");
        }

        // Move
        if (isMoving)
        {
            // Flip based on direction
            Debug.Log("here");
            if (targetPosition.x < transform.position.x)
                GetComponent<SpriteRenderer>().flipX = true;
            else
                GetComponent<SpriteRenderer>().flipX = false;

            // Calculate next position
            Vector3 nextPos = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            // Convert next position to tile cell
            Vector3Int nextCell = grassTilemap.WorldToCell(nextPos);

            // Stop if next step is not grass
            if (!grassTilemap.HasTile(nextCell))
            {
                isMoving = false;
                animator.Play("Idle");
                return;
            }

            // Apply movement
            transform.position = nextPos;

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isMoving = false;
                animator.Play("Idle");
            }
        }
    }
}
