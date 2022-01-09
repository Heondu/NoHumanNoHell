using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum AttackType
{
    MeleeAttack,
    StrongMeleeAttack,
    RangedAttack
}

[RequireComponent(typeof(Status), typeof(Movement))]
public class Entity : MonoBehaviour
{
    [SerializeField] private AttackType defaultAttackType;

    [Header("Ranged Attack")]
    public GameObject ProjectilePrefab;
    public Transform RangedAttackPoint;

    [Header("Option")]
    [SerializeField] bool canBeDamaged = true;

    private Movement movement;
    private CapsuleCollider2D capsuleCollider2D;

    [Header("Event")]
    public UnityEvent<Entity, float> onTakeDamage;
    public UnityEvent<Entity> onDead;

    public Status Status { get; private set; }
    public AttackType AttackType { get; set; }
    public Vector3 Position => capsuleCollider2D.bounds.center;
    private List<AttackTimer> attackTimers = new List<AttackTimer>();

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        Status = GetComponent<Status>();
        movement = GetComponent<Movement>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        AttackType = defaultAttackType;
    }

    public void TakeDamage(GameObject instigator, float damage)
    {
        if (!canBeDamaged) return;

        Status.CurrentHP -= damage;

        movement.Knockback((transform.position - instigator.transform.position).normalized);

        onTakeDamage.Invoke(this, damage);

        if (Status.CurrentHP == 0f)
            onDead.Invoke(this);
    }

    public float GetAttackDelay()
    {
        return GetAttackDelay(AttackType);
    }

    public float GetAttackDelay(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.MeleeAttack: return Status.GetValue(StatusType.MeleeAttackDelay);
            case AttackType.StrongMeleeAttack: return Status.GetValue(StatusType.StrongMeleeAttackDelay);
            case AttackType.RangedAttack: return Status.GetValue(StatusType.RangedAttackDelay);
            default: return 0f;
        }
    }

    public void SetCanBeDamaged(bool value)
    {
        canBeDamaged = value;
    }

    public bool CanAttack()
    {
        AttackTimer attackTimer = attackTimers.FirstOrDefault(x => x.AttackType == AttackType);
        if (attackTimer == null)
            return true;
        return attackTimer.CanAttack();
    }

    public void SetAttackTimer()
    {
        AttackTimer attackTimer = attackTimers.FirstOrDefault(x => x.AttackType == AttackType);
        if (attackTimer == null)
            AddAttackTimer();
        else
            attackTimer.SetTime();
    }

    private void AddAttackTimer()
    {
        AttackTimer attackTimer = new AttackTimer();
        attackTimer.Setup(this, AttackType);
        attackTimer.SetTime();
        attackTimers.Add(attackTimer);
    }
}

public class AttackTimer
{
    private Entity entity;
    public AttackType AttackType { get; private set; }
    private float time;
    
    public void Setup(Entity entity, AttackType attackType)
    {
        this.entity = entity;
        this.AttackType = attackType;
    }

    public void SetTime()
    {
        time = Time.time;
    }

    public bool CanAttack()
    {
        return Time.time - time >= entity.GetAttackDelay(AttackType);
    }
}