using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("Salto")]
    public float jumpForce = 6f;
    public float extraGravityMultiplier = 2f;
    public float fallMultiplier = 2.5f;

    [Header("Detección de suelo")]
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        Vector2 boxSize = new Vector2(0.4f, 0.1f); // Ancho del personaje y grosor mínimo
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
    }

    void FixedUpdate()
    {
        float targetVelocityX = moveInput * moveSpeed;
        rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);

        if (!isGrounded)
        {
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.fixedDeltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * extraGravityMultiplier * Time.fixedDeltaTime;
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector2 boxSize = new Vector2(0.4f, 0.1f); 
        Vector3 boxCenter = transform.position + Vector3.down * (groundCheckDistance + boxSize.y * 0.5f);

        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

}
