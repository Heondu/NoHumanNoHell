using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpCount;
    [SerializeField] private float dashForceOnGround;
    [SerializeField] private float dashForceOnAir;
    [SerializeField] private float dashTime;
    [SerializeField] private float knockbackForce;

    private new Rigidbody2D rigidbody2D;
    private bool isGrounded = false;
    private bool isDash = false;
    private int currentJumpCount = 0;
    private int currentDashCount = 0;
    private Vector3 moveDirection;


    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void Jump(Vector3 direction)
    {
        if (currentJumpCount == 0 && !isGrounded) return;
        if (currentJumpCount >= jumpCount) return;

        currentJumpCount++;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(direction * jumpForce, ForceMode2D.Impulse);
    }

    public void Dash(Vector3 direction)
    {
        if (isDash) return;
        if (currentDashCount != 0 && !isGrounded) return;

        isDash = true;
        currentDashCount++;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(direction * (isGrounded ? dashForceOnGround : dashForceOnAir), ForceMode2D.Impulse);
        StartCoroutine("StopDash");
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(dashTime);

        rigidbody2D.velocity = Vector2.zero;
        isDash = false;
    }

    public void Knockback(Vector3 direction)
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(direction * knockbackForce,  ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            currentJumpCount = 0;
            currentDashCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
}
