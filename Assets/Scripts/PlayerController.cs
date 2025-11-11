using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    [Header("Interact Settings")]
    public float interactDistanceHorizontal = 1f;
    public float interactDistanceVertical = 1f;
    public LayerMask interactLayer;

    [Header("Sprite Settings")]
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private Vector2 lookDirection = Vector2.down;
    private Interactable currentInteractable;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (UIManager.Instance != null && UIManager.Instance.IsUIOpen)
        {
            movement = Vector2.zero;

            if (Input.GetKeyDown(KeyCode.E))
            {
                UIManager.Instance.CloseDialogue();
                UIManager.Instance.ClosePuzzlePanel();
            }
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.sqrMagnitude > 0.01f)
        {
            lookDirection = movement.normalized;
            UpdateSpriteDirection();
        }

        DetectInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void DetectInteractable()
    {
        float distance = Mathf.Abs(lookDirection.x) > Mathf.Abs(lookDirection.y)
            ? interactDistanceHorizontal
            : interactDistanceVertical;

        RaycastHit2D hit = Physics2D.Raycast(rb.position, lookDirection, distance, interactLayer);

        if (hit.collider != null && hit.collider.TryGetComponent(out Interactable interactable))
        {
            currentInteractable = interactable;
        }
        else
        {
            currentInteractable = null;
        }

        Debug.DrawRay(rb.position, lookDirection * distance, Color.darkRed);
    }

    void UpdateSpriteDirection()
    {
        if (lookDirection.y > 0.1f)
            spriteRenderer.sprite = backSprite;
        else if (lookDirection.y < -0.1f)
            spriteRenderer.sprite = frontSprite;
        else if (lookDirection.x > 0.1f)
            spriteRenderer.sprite = rightSprite;
        else if (lookDirection.x < -0.1f)
            spriteRenderer.sprite = leftSprite;
    }
}