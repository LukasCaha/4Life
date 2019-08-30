using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public AudioSource jump;
    public AudioSource fall;
    public Transform topLeft;
    public Transform bottomRight;
    public LayerMask jumpableLayers;
    public Hook hook;

    public float speed;
    public float jumpPower;
    public float fallSFXTreshold;
    public bool canMove = true;
    public bool hooked = false;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 previousVelocity;

    private bool grounded = false;
    private float horizontal = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;

            transform.Translate(new Vector2(horizontal * speed, 0));
        }
        
        grounded = Physics2D.OverlapArea(topLeft.position, bottomRight.position, jumpableLayers);
        if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (grounded||hooked) && VelocityInBounds(rb.velocity.y, 0.001f))
        {
            hook.Detach();
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Force);
            jump.Play();
        }
        else
        {
            if (Vector3.Distance(rb.velocity, previousVelocity) >= fallSFXTreshold && previousVelocity.y < 0)
            {
                fall.Play();
            }
        }
        previousVelocity = rb.velocity;

        float direction = GetComponentInChildren<Combat>().GetDirection();
        if (direction*horizontal > 0)           //směr pušky je rozdílný od směru pohybu
        {
            if (horizontal > 0)
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            if (horizontal < 0)
                transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }

        if (horizontal != 0)
            animator.SetBool("Walking", true);
        else
            animator.SetBool("Walking", false);
    }

    bool VelocityInBounds(float velocity, float bounds)
    {
        if (Mathf.Abs(velocity) < bounds)
            return true;
        else
            return false;
    }
}
