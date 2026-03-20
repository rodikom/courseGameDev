using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundChecker;
    public LayerMask groundLayer;

    [SerializeField, Range(0f, 15f)] private float speed = 8f;
    [SerializeField, Range(0f, 35f)] private float jumpingPower = 11f;

    [Header("Weapon")]
    [SerializeField] private GameObject weapon;

    private Animator animator;
    private float horizontal;
    private bool isAttacking;
    private bool isFacingRight = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (isFacingRight && horizontal < 0f)
            Flip();
        else if (!isFacingRight && horizontal > 0f)
            Flip();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetTrigger("Jump");
        }

        if (context.canceled && rb.velocity.y > 0f)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundChecker.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed &&  !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }
    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        weapon.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        weapon.SetActive(false);
        isAttacking = false;
    }
}