using UnityEngine;

public class PlayerController : MonoBehaviour, ILivingEntity
{
    private Movement movement;
    private Attack attack;

    private void Start()
    {
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
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
            Vector3 direction = (Utilities.GetMouseWorldPos() - transform.position).normalized;
            movement.Dash(direction);
            attack.MeleeAttack(direction);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 direction = (Utilities.GetMouseWorldPos() - transform.position).normalized;
            attack.RangedAttack(direction);
        }
    }

    public void TakeDamage(float damage, GameObject instigator)
    {

    }
}
