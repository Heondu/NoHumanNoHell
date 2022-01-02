using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float distance;
    [SerializeField] private float cooldown;
    [SerializeField] private float delay;

    private Movement movement;
    private EnemyAnimation enemyAnimation;

    private bool isAttacking = false;
    private Vector3 attackDirection;

    private void Start()
    {
        movement = GetComponent<Movement>();
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    public void Attack(Vector3 direction)
    {
        if (isAttacking) return;

        isAttacking = true;
        attackDirection = direction;

        StartCoroutine("AttackCo", direction);
    }

    private IEnumerator AttackCo(Vector3 direction)
    {
        yield return new WaitForSeconds(delay);

        movement.Dash(direction);

        enemyAnimation.Play("Attack");

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Vector2.one, 0, direction, distance);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag(gameObject.tag)) continue;

            ILivingEntity entity = hit.collider.GetComponent<ILivingEntity>();
            if (entity != null)
            {
                entity.TakeDamage(damage, gameObject);
            }
        }

        attackDirection = Vector3.zero;

        StartCoroutine("Cooldown", cooldown);
    }

    private IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);

        isAttacking = false;
        enemyAnimation.Play("Idle");
    }

    public Vector3 GetAttackDirection()
    {
        return attackDirection;
    }

    public float GetAttackDistance()
    {
        return distance;
    }
}
