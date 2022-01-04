using UnityEngine;
using UnityEngine.Events;

public enum AttackType
{
    MeleeAttack,
    RangedAttack
}

[RequireComponent(typeof(Status), typeof(Movement))]
public class Entity : MonoBehaviour
{
    [SerializeField] private AttackType defaultAttackType;

    [Header("Ranged Attack")]
    [SerializeField] private GameObject projectilePrefab;

    [Header("Option")]
    [SerializeField] bool canBeDamaged = true;

    private Movement movement;

    [Header("Event")]
    public UnityEvent<Entity, GameObject, float> onTakeDamage;
    public UnityEvent<Entity> onDead;

    public Status Status { get; private set; }
    public AttackType AttackType { get; set; }
    public GameObject ProjectilePrefab => projectilePrefab;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        Status = GetComponent<Status>();
        movement = GetComponent<Movement>();
        AttackType = defaultAttackType;
    }

    public void Shoot(Vector3 direction)
    {
        Shoot(direction, Status.GetValue(StatusType.RangedAttackDamage));
    }

    private void Shoot(Vector3 direction, float damage)
    {
        PlayerProjectile clone = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity).GetComponent<PlayerProjectile>();
        clone.Setup(direction, Status.GetValue(StatusType.RangedAttackRange), (int)damage, gameObject);
    }

    public void TakeDamage(GameObject instigator, float damage)
    {
        if (!canBeDamaged) return;

        Status.CurrentHP -= damage;

        Vector3 direction = (transform.position - instigator.transform.position).normalized;
        movement.Knockback(direction);

        onTakeDamage.Invoke(this, instigator, damage);

        if (Status.CurrentHP == 0f)
            onDead.Invoke(this);
    }

    public float GetAttackDelay()
    {
        return AttackType == AttackType.MeleeAttack ? Status.GetValue(StatusType.MeleeAttackDelay) : Status.GetValue(StatusType.RangedAttackDelay);
    }
}
