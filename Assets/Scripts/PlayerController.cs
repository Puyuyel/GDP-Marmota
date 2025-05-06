using UnityEngine;
using UnityEngine.UI;

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

    [Header("Jetpack")]
    public float jetpackForce = 7f;
    public float maxFuel = 3f;
    public float fuelBurnRate = 1f;
    public float fuelRegenRate = 2f;
    public Slider fuelSlider;

    [Header("Detección de suelo")]
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool jumpRequested;
    private bool useJetpack;
    private float moveInput;
    private float currentFuel;
    private float currentJetpackForce;
    private float jetpackBlockTimer = 0f;
    public float jetpackDelayAfterJump = 0.2f;
    private bool jumpReleased = false;
    private bool jumpPressedLastFrame = false;
    private bool allowJetpack = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentFuel = maxFuel;

        if (fuelSlider != null)
        {
            fuelSlider.maxValue = maxFuel;
            fuelSlider.value = currentFuel;
        }
    }

    void Update()
    {
        // Detección de suelo
        Vector2 boxSize = new Vector2(0.4f, 0.1f);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
        bool wasGrounded = isGrounded;
        isGrounded = hit.collider != null;

        if (isGrounded && !wasGrounded)
        {
            currentJetpackForce = 0f;
            allowJetpack = false; // resetear cuando aterriza
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        bool jumpPressed = Input.GetButton("Jump");

        // Saltar desde el suelo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
            jumpReleased = false;
        }

        // Detectar si soltó el botón luego de saltar
        if (!jumpPressed && !isGrounded)
        {
            jumpReleased = true;
        }

        // Permitir jetpack solo si se volvió a presionar después de soltar
        if (!isGrounded && jumpReleased && jumpPressed && !jumpPressedLastFrame)
        {
            allowJetpack = true;
        }

        jumpPressedLastFrame = jumpPressed;

        // Actualizar barra de combustible
        if (fuelSlider != null)
        {
            fuelSlider.value = currentFuel;
        }
    }

    void FixedUpdate()
    {
        float targetVelocityX = moveInput * moveSpeed;
        rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);

        // Saltar
        if (jumpRequested)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpRequested = false;
        }

        bool canUseJetpack = !isGrounded && allowJetpack && Input.GetButton("Jump") && currentFuel > 0f;

        if (canUseJetpack)
        {
            currentJetpackForce = Mathf.Lerp(currentJetpackForce, jetpackForce, Time.fixedDeltaTime * 3f);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentJetpackForce);

            currentFuel -= fuelBurnRate * Time.fixedDeltaTime;
            currentFuel = Mathf.Max(currentFuel, 0f);
        }
        else
        {
            currentJetpackForce = 0f;

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

        if (isGrounded && currentFuel < maxFuel)
        {
            currentFuel += fuelRegenRate * Time.fixedDeltaTime;
            currentFuel = Mathf.Min(currentFuel, maxFuel);
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
