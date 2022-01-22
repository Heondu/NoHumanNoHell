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
    protected BehaviorTree behaviorTree;

    protected Entity target;
    [HideInInspector] public bool IsAttacking;
    protected bool isStop = false;

    private Vector3 originPos;
    [HideInInspector] public Vector3 MovePosition;
    [SerializeField] private float patrolTime;
    [SerializeField] private float patrolRange;

    public Entity Entity => entity;
    public Entity Target => target;
    public bool IsStop => isStop;
    public Vector3 OriginPos => originPos;
    public float PatrolTime => patrolTime;
    public float PatrolRange => patrolRange;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        animator = GetComponent<Animator>();
        entity = GetComponent<Entity>();
        movement = GetComponent<Movement>();
        behaviorTree = GetComponent<BehaviorTree>();
    }

    protected virtual void Start()
    {
        target = FindObjectOfType<PlayerController>().GetComponent<Entity>();
        behaviorTree.Init(this);
        originPos = transform.position;

        entity.onTakeDamage.AddListener(PlayHitAnim);
        entity.onDead.AddListener(OnDead);
    }

    private void Update()
    {
        if (entity.IsDead)
            return;

        behaviorTree.BTUpdate();
    }

    protected void Look(Vector3 direction)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(direction.x) * Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    public void Attack()
    {
        IsAttacking = true;
        entity.SetAttackTimer(entity.AttackType.ToString(), entity.GetAttackDelay());
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
            animator.Play("Shoot");
            yield return new WaitForSeconds(entity.Status.GetValue(StatusType.RangedAttackDelay));
        }
        IsAttacking = false;
    }

    public void Shoot()
    {
        Projectile clone = Instantiate(entity.ProjectilePrefab, entity.RangedAttackPoint.position, Quaternion.identity).GetComponent<Projectile>();
        Vector3 direction = (target.Position - entity.RangedAttackPoint.position).normalized;
        clone.Setup(direction, (int)entity.Status.GetValue(StatusType.RangedAttackDamage), gameObject);
    }

    public void LookAtTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Look(direction);
    }

    private void AttackEnd()
    {;
        IsAttacking = false;
        WaitAttackDelay(entity.Status.GetValue(StatusType.MeleeAttackDelay));
    }

    protected void WaitAttackDelay(float time)
    {
        StopCoroutine("WaitAttackDelayCo");
        StartCoroutine("WaitAttackDelayCo", time);
    }

    private IEnumerator WaitAttackDelayCo(float time)
    {
        isStop = true;
        yield return new WaitForSeconds(time);
        isStop = false;
    }

    public void WaitAndPlayAnim(string name, float delay)
    {
        WaitAttackDelay(delay);
        StartCoroutine(WaitAndPlayAnimCo(name, delay));
    }

    private IEnumerator WaitAndPlayAnimCo(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (entity.IsDead)
            yield break;
        animator.Play(name);
    }

    public virtual void OnDead()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        animator.Play("Death");
        Destroy(gameObject, 1);
    }

    public void PlayHitAnim()
    {
        animator.Play("Hit");
    }
}
