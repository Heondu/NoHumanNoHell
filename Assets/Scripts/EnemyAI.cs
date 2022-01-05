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
    [Header("Attack")]
    [SerializeField] protected float attackCastTime;

    protected Entity entity;
    protected Movement movement;
    protected Animator animator;
    protected IBehaviorTree enemyBT;
    protected DetectBox detectBox;

    protected Entity target;
    protected bool isAttacking = false;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        animator = GetComponent<Animator>();
        entity = GetComponent<Entity>();
        movement = GetComponent<Movement>();
        enemyBT = GetComponent<IBehaviorTree>();
        detectBox = GetComponentInChildren<DetectBox>();
    }

    private void Start()
    {
        enemyBT.Init(this);
    }

    private void Update()
    {
        enemyBT.BTUpdate();
    }

    protected void Look(Vector3 direction)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(direction.x) * Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    public void Detect()
    {
        target = detectBox.GetTarget();
    }

    public bool HasTarget()
    {
        return target;
    }

    public void Idle()
    {
        movement.Move(Vector3.zero);
    }

    public void Chase()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;
        movement.Move(direction);
        Look(direction);
    }

    public void Attack()
    {
        StartCoroutine("AttackCo");
    }

    private IEnumerator AttackCo()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackCastTime);

        animator.Play("Attack");

        yield return new WaitForSeconds(entity.Status.GetValue(StatusType.MeleeAttackDelay));

        isAttacking = false;
    }

    public void LookAtTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Look(direction);
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public bool IsTargetInMeleeRange()
    {
        float distance = Vector3.SqrMagnitude(target.transform.position - transform.position);
        float range = entity.Status.GetValue(StatusType.MeleeAttackRange);
        return distance <= range * range;
    }
}
