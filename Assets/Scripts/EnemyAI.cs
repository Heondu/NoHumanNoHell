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
    protected CapsuleCollider2D capsuleCollider2D;

    protected Entity target;
    protected bool isAttacking = false;

    private Vector3 originPos;
    private Vector3 patrolPosition;
    [SerializeField] private float patrolTime;
    [SerializeField] private float patrolRange;
    private bool isPatrol;

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
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        enemyBT.Init(this);
        originPos = transform.position;
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

    public void Patrol()
    {
        if (!isPatrol)
            StartCoroutine("PatrolCo");

        Vector3 direction = (patrolPosition - transform.position).normalized;
        movement.Move(direction);
        Look(direction);
    }

    public bool IsClosePatolPos()
    {
        return (Vector3.SqrMagnitude(patrolPosition - transform.position) < 0.5f);
    }

    public bool IsReachablePos()
    {
        Vector3 direction = (patrolPosition - transform.position).normalized;

        LayerMask EnemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");

        RaycastHit2D[] hits = Physics2D.RaycastAll(capsuleCollider2D.bounds.center, direction, 0.5f, EnemyLayer + groundLayer);
        foreach (RaycastHit2D hit in hits)
            if (hit.collider.gameObject != gameObject)
                return false;

        RaycastHit2D hitGround = Physics2D.Raycast(transform.position + direction * 0.5f, Vector2.down, 0.1f, groundLayer);
        if (!hitGround)
            return false;

        return true;
    }

    public void FindPatrolPos()
    {
        if (!isPatrol)
            patrolPosition = new Vector3(Random.Range(originPos.x - patrolRange, originPos.x + patrolRange), originPos.y, originPos.z);
    }

    private IEnumerator PatrolCo()
    {
        isPatrol = true;

        yield return new WaitForSeconds(patrolTime);

        isPatrol = false;
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
        entity.SetAttackTimer();
        StartCoroutine("AttackCo");
    }

    private IEnumerator AttackCo()
    {
        yield return new WaitForSeconds(attackCastTime);

        if (entity.AttackType == AttackType.MeleeAttack)
        {
            animator.Play("Attack");
            yield return new WaitForSeconds(entity.Status.GetValue(StatusType.MeleeAttackDelay));
        }
        else if (entity.AttackType == AttackType.RangedAttack)
        {
            if (HasTarget())
                Shoot((target.Position - entity.RangedAttackPoint.position).normalized);
            yield return new WaitForSeconds(entity.Status.GetValue(StatusType.RangedAttackDelay));
        }
    }

    public void Shoot(Vector3 direction)
    {
        Shoot(direction, entity.Status.GetValue(StatusType.RangedAttackDamage));
    }

    private void Shoot(Vector3 direction, float damage)
    {
        Projectile clone = Instantiate(entity.ProjectilePrefab, entity.RangedAttackPoint.position, Quaternion.identity).GetComponent<Projectile>();
        clone.Setup(direction, (int)damage, gameObject);
    }

    public void LookAtTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Look(direction);
    }

    public bool CanAttack()
    {
        return entity.CanAttack();
    }

    public bool IsTargetInAttackRange()
    {
        float distance = Vector3.SqrMagnitude(target.transform.position - transform.position);
        float range = entity.AttackType == AttackType.MeleeAttack ? entity.Status.GetValue(StatusType.MeleeAttackRange) : entity.Status.GetValue(StatusType.RangedAttackRange);
        return distance <= range * range;
    }
}
