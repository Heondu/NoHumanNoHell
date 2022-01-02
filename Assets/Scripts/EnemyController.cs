using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, ILivingEntity
{
    private SpriteRenderer spriteRenderer;
    private Movement movement;
    private EnemyAttack enemyAttack;
    private Status status;
    private Transform target;

    [SerializeField] private Vector2 detectSize;
    [SerializeField] private LayerMask detectLayerMask;
    private bool isAttacking = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        enemyAttack = GetComponent<EnemyAttack>();
        status = GetComponent<Status>();
    }

    private void Update()
    {
        FSM();
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        StopCoroutine("Flash");
        StartCoroutine("Flash");

        status.TakeDamage(damage);
        Vector3 direction = (transform.position - instigator.transform.position).normalized;
        movement.Knockback(direction);

        if (status.GetCurrentHP() == 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Flash()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.black;
    }

    private void FSM()
    {
        Detect();

        if (target == null) return;

        float distance = Vector3.SqrMagnitude(target.position - transform.position);

        if (distance <= enemyAttack.GetAttackDistance())
        {
            if (isAttacking) return;

            isAttacking = true;
            StartCoroutine("AttackCo");
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
                StopCoroutine("AttackCo");
            }
            Move();
        }
    }

    private void Detect()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, detectSize, 0, detectLayerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                target = collider.transform;
                return;
            }
            else
            {
                target = null;
            }
        }
    }

    private void Move()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        movement.MoveTo(direction);
    }

    private void Attack()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        enemyAttack.Attack(direction);
    }

    private IEnumerator AttackCo()
    {
        yield return new WaitForSeconds(1f);

        Attack();
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, detectSize);
    }
}
