using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    public bool canMove = true;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            return;
        }

        animator.SetBool("isWalking", true);

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!canMove) return;
        
        if (context.performed)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
            foreach (Collider2D hit in hitColliders)
            {
                if (hit.TryGetComponent<LightBulbInteraction>(out LightBulbInteraction bulb))
                {
                    bulb.OpenMiniGame();
                }
                else if(hit.TryGetComponent<PetAI>(out PetAI pet))
                {
                    pet.TalkToPet();
                }
            }
        }
    }
}
