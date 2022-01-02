using UnityEngine;

public class PlayerController : MonoBehaviour, ILivingEntity
{
    private Movement movement;
    private PlayerAttack playerAttack;
    private Status status;
    private new Rigidbody2D rigidbody2D;

    private void Start()
    {
        movement = GetComponent<Movement>();
        playerAttack = GetComponent<PlayerAttack>();
        status = GetComponent<Status>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Jump();
        Attack();
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");

        movement.MoveTo(new Vector3(x, 0, 0));
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.Jump(Vector3.up);
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerAttack.MeleeAttackCheck();
        }
        if (Input.GetMouseButtonDown(1))
        {
            playerAttack.RangedAttackCheck();
        }
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        status.TakeDamage(damage);
    }
}
