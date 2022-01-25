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

[RequireComponent(typeof(Status))]
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
    public UnityEvent onTakeDamage;
    public UnityEvent onDead;

    public Status Status { get; private set; }
    public AttackType AttackType { get; set; }
    public Vector3 Position => capsuleCollider2D.bounds.center;
    private List<AttackTimer> attackTimers = new List<AttackTimer>();

    private bool isDead;
    public bool IsDead => isDead;
    public bool CanBeDamaged
    {
        get { return canBeDamaged; }
        set { canBeDamaged = value; }
    }

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

    public void TakeDamage(float damage)
    {
        if (!canBeDamaged) return;

        Status.CurrentHP -= damage;

        onTakeDamage.Invoke();

        if (Status.CurrentHP == 0f)
        {
            isDead = true;
            onDead.Invoke();
        }
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

    public bool CanAttack(string key)
    {
        AttackTimer attackTimer = attackTimers.FirstOrDefault(x => x.Key == key);
        if (attackTimer == null)
            return true;
        return attackTimer.CanAttack();
    }

    public void SetAttackTimer(string key, float delay)
    {
        AttackTimer attackTimer = attackTimers.FirstOrDefault(x => x.Key == key);
        if (attackTimer == null)
            AddAttackTimer(key, delay);
        else
            attackTimer.Init();
    }

    private void AddAttackTimer(string key, float delay)
    {
        AttackTimer attackTimer = new AttackTimer();
        attackTimer.Setup(key, delay);
        attackTimer.Init();
        attackTimers.Add(attackTimer);
    }
}

public class AttackTimer
{
    public string Key { get; private set; }
    private float delay;
    private float time;
    
    public void Setup(string key, float delay)
    {
        Key = key;
        this.delay = delay;
    }

    public void Init()
    {
        time = Time.time;
    }

    public bool CanAttack()
    {
        return Time.time - time >= delay;
    }
}