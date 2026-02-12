using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private Vector2 lastDirection; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastDirection = new Vector2(0, -1);
        if (rb == null)
            Debug.LogError("Rigidbody2D가 없습니다!");
        if (animator == null)
            Debug.LogError("Animator가 없습니다!");
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.magnitude > 0.1f)
        {
           
            lastDirection = movement.normalized;
            animator.SetFloat("MoveX", lastDirection.x);
            animator.SetFloat("MoveY", lastDirection.y);
        }
        else
        {
            
            animator.SetFloat("MoveX", lastDirection.x * 0.01f);
            animator.SetFloat("MoveY", lastDirection.y * 0.01f);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
