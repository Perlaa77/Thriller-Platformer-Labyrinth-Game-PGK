using UnityEngine;

public class SecondController : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float jumpHeightThreshold = 1f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 6f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    private float horizontalInput;
    private bool jumpPressed;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("SecondController: Brak przypisanego Targetu (gracza) w Inspectorze!", this);
            return;
        }

        float xDifference = target.position.x - transform.position.x;
        float yDifference = target.position.y - transform.position.y;

        if (Mathf.Abs(xDifference) > stopDistance)
        {
            horizontalInput = Mathf.Sign(xDifference);
        }
        else
        {
            horizontalInput = 0;
        }

        if (animator != null) animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        if (horizontalInput != 0)
        {
            sr.flipX = horizontalInput < 0;
        }

        if (isGrounded)
        {
            if (yDifference > jumpHeightThreshold && Mathf.Abs(xDifference) < stopDistance * 3f)
            {
                Debug.Log($"Skok (Ró¿nica Y: {yDifference})", this);
                jumpPressed = true;
            }
            //else if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(rb.linearVelocity.x) < 0.1f)
            //{
            //    Debug.Log($"Skok awaryjny (Zablokowanie X)", this);
            //    jumpPressed = true;
            //}
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer) != null;

        // Wypisz do konsoli, czy postaæ w ogóle dotyka ziemi w danej klatce fizyki
        // (Zmieñ logikê na if, ¿eby nie spamowa³o konsoli w nieskoñczonoœæ, ale dla testu warto to widzieæ)
        //Debug.Log("Second isGrounded: " + isGrounded);

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
        }
    }

    private void OnDrawGizmos() // Zmieniono na OnDrawGizmos, by widzieæ w ka¿dej chwili
    {
        // Rysowanie Ground Check
        if (groundCheck != null)
        {
            // Zmiana koloru w zale¿noœci od isGrounded
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }

        // Rysowanie linii pod¹¿ania do celu
        if (target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}