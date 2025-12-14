using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class FreeCamera2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 3f;

    private float targetZoom;

    Camera cam;
    Tilemap tilemap;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        targetZoom = cam.orthographicSize;

        tilemap = FindFirstObjectByType<Tilemap>();
        if (tilemap == null)
            Debug.LogError("FreeCamera2D: No Tilemap found for clamping.");
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    void LateUpdate()
    {
        if (tilemap != null)
            ClampToMap();
    }

    // ---------------- MOVEMENT ----------------
    void HandleMovement()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
        }

        Vector3 move = new Vector3(input.x, input.y, 0f);
        transform.Translate(move * moveSpeed * Time.deltaTime);
    }

    // ---------------- ZOOM ----------------
    void HandleZoom()
    {
        float scroll = Mouse.current?.scroll.ReadValue().y ?? 0f;
        if (Mathf.Abs(scroll) < 0.01f) return;

        targetZoom -= scroll * zoomSpeed * Time.deltaTime;
        targetZoom = Mathf.Max(minZoom, targetZoom);

        //Clamp zoom to map bounds BEFORE applying
        Bounds b = tilemap.localBounds;
        float maxZoomH = b.size.y / 2f;
        float maxZoomW = b.size.x / (2f * cam.aspect);
        float safeMaxZoom = Mathf.Min(maxZoomH, maxZoomW);

        targetZoom = Mathf.Min(targetZoom, safeMaxZoom);

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetZoom,
            Time.deltaTime * 10f
        );
    }

    // ---------------- CLAMP ----------------
    void ClampToMap()
    {
        Bounds b = tilemap.localBounds;

        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        Vector3 pos = transform.position;

        float minX = b.min.x + camHalfWidth;
        float maxX = b.max.x - camHalfWidth;
        float minY = b.min.y + camHalfHeight;
        float maxY = b.max.y - camHalfHeight;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
}
