using UnityEngine;

/// <summary>
/// 플레이어 행동을 전체적을 관리하는 클래스
/// </summary>
public class PlayerController : MonoBehaviour, ILivingEntity
{
    private Movement movement;
    private PlayerAttack playerAttack;
    private Status status;

    private void Start()
    {
        movement = GetComponent<Movement>();
        playerAttack = GetComponent<PlayerAttack>();
        status = GetComponent<Status>();
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
