using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack
}

public class EnemyAI : MonoBehaviour
{
    [Header("Detect")]
    [SerializeField] private Vector2 detectRange;
    [SerializeField] private LayerMask detectTargetLayer;

    [Header("Attack")]
    [SerializeField] private float attackCastTime;

    private Entity entity;
    private Movement movement;
    private Animator animator;

    private Entity target;
    private EnemyState currentState;

    private void Start()
    {
        Setup();
        ChangeState(EnemyState.Idle);
    }

    private void Setup()
    {
        animator = GetComponent<Animator>();
        entity = GetComponent<Entity>();
        movement = GetComponent<Movement>();
    }

    private void ChangeState(EnemyState newState)
    {
        StopCoroutine(currentState.ToString());
        currentState = newState;
        StartCoroutine(currentState.ToString());
    }

    private void Look(Vector3 direction)
    {
        transform.localScale = new Vector3(direction.x < 0 ? -1f : 1f, 1f, 1f);
    }

    private Entity Detect()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, detectRange, 0, detectTargetLayer);
        if (collider != null)
            target = collider.GetComponent<Entity>();
        else
            target = null;
        return target;
    }

    private IEnumerator Idle()
    {
        while (true)
        {
            if (Detect())
                ChangeState(EnemyState.Chase);

            movement.Move(Vector3.zero);

            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        while (true)
        {
            if (!Detect())
                ChangeState(EnemyState.Idle);
            else
            {
                if (!IsTargetInMeleeRange())
                {
                    Vector3 direction = (target.transform.position - transform.position).normalized;
                    direction.y = 0;
                    movement.Move(direction);
                    Look(direction);
                }
                else
                {
                    movement.Move(Vector3.zero);
                    ChangeState(EnemyState.Attack);
                }
            }

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCastTime);

        while (true)
        {
            if (Detect())
            {
                if (IsTargetInMeleeRange())
                {
                    Vector3 direction = (target.transform.position - transform.position).normalized;
                    Look(direction);

                    animator.Play("Attack");

                    yield return new WaitForSeconds(entity.Status.GetValue(StatusType.MeleeAttackDelay));
                }
                else
                {
                    ChangeState(EnemyState.Chase);
                    yield return null;
                }
            }
            else
            {
                ChangeState(EnemyState.Idle);
                yield return null;
            }
        }
    }
    public bool IsTargetInMeleeRange()
    {
        float distance = Vector3.SqrMagnitude(target.transform.position - transform.position);
        float range = entity.Status.GetValue(StatusType.MeleeAttackRange);
        return distance <= range * range;
    }
}
