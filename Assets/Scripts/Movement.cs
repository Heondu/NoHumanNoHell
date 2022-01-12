using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpCount;

    [Header("Dash")]
    [SerializeField] private float dashForceDefault;
    [SerializeField] private float dashForceStrong;
    [SerializeField] private float dashForceOnAir;
    [SerializeField] private float dashTime;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce;

    [Header("Layer")]
    [SerializeField] private LayerMask groundLayer;

    private new Rigidbody2D rigidbody2D;
    private CapsuleCollider2D capsuleCollider2D;
    private bool isGrounded = false;
    private bool isSlope = false;
    private bool isDash = false;
    private int currentJumpCount = 0;
    private int currentDashCount = 0;
    private Vector3 moveDirection;
    private float angle;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        CheckForGround();
    }

    public void Move(Vector3 direction)
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
        StopCoroutine("DashCo");
        StartCoroutine("DashCo", direction * (isGrounded ? dashForceDefault : dashForceOnAir));
    }

    private void Dash(Vector3 direction, float force)
    {
        StopCoroutine("DashCo");
        StartCoroutine("DashCo", direction * force);
    }

    private IEnumerator DashCo(Vector3 force)
    {
        if (currentDashCount != 0 && !isGrounded)
            yield break;

        ++currentDashCount;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashTime);

        rigidbody2D.velocity = Vector2.zero;
    }

    public void DashToLookWeek()
    {
        if (isGrounded)
            Dash(new Vector3(transform.localScale.x, 0, 0), dashForceDefault);
        else
            Dash((Utilities.GetMouseWorldPos() - transform.position).normalized);
    }

    public void DashToLookStrong()
    {
        if (isGrounded)
            Dash(new Vector3(transform.localScale.x, 0, 0), dashForceStrong);
        else
            Dash((Utilities.GetMouseWorldPos() - transform.position).normalized);
    }

    public void Knockback(Vector3 direction)
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }

    private void CheckForGround()
    {
        Bounds bounds = capsuleCollider2D.bounds;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(bounds.center.x, bounds.min.y), Vector2.down, 0.1f, groundLayer);

        if (hit)
        {
            angle = Vector2.Angle(hit.normal, Vector2.up);
            isSlope = angle != 0 ? true : false;

            if (rigidbody2D.velocity.y > 0) return;

            isGrounded = true;
            currentJumpCount = 0;
            currentDashCount = 0;
        }
        else
        {
            isGrounded = false;
        }
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
