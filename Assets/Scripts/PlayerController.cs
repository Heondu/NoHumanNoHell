using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Combo")]
    [SerializeField] private ComboTree comboTree;

    [HideInInspector] public UnityEvent<Entity, AttackType, int> onAttack;
    [HideInInspector] public UnityEvent onJump;

    private Movement movement;
    private Entity entity;
    private ComboNode currentComboNode;

    private int currentComboCount;
    private bool isInputAttack = false;
    private bool isAttacking = false;
    private Vector3 lookDirection;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        movement = GetComponent<Movement>();
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        MoveUpdate();
        JumpUpdate();
        AttackUpdate();
        LookUpdate();
    }

    private void MoveUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        movement.Move(new Vector3(x, 0, 0));
    }

    private void JumpUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onJump.Invoke();
            movement.Jump(Vector3.up);
        }
    }

    private void AttackUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            InputAttack(AttackType.MeleeAttack);
        else if (Input.GetMouseButtonDown(1))
            InputAttack(AttackType.StrongMeleeAttack);
    }

    private void LookUpdate()
    {
        Vector3 localScale = transform.localScale;
        if (lookDirection.x != 0)
            localScale.x = Mathf.Sign(lookDirection.x) * Mathf.Abs(localScale.x);
        else if (movement.GetMoveDirection().x != 0)
            localScale.x = Mathf.Sign(movement.GetMoveDirection().x) * Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    private void InputAttack(AttackType attackType)
    {
        entity.AttackType = attackType;
        isInputAttack = true;

        if (entity.CanAttack(attackType.ToString()))
            TryAttack();
    }
    
    private void TryAttack()
    {
        currentComboNode = GetNextComboNode(entity.AttackType);
        if (currentComboNode == null)
            ResetAttackParameter();
        else
        {
            entity.Status.SetValue(StatusType.MeleeAttackDamage, currentComboNode.Damage);
            ++currentComboCount;
            entity.SetAttackTimer(entity.AttackType.ToString(), entity.GetAttackDelay());

            StopCoroutine("Attack");
            StartCoroutine("Attack");
        }
    }

    private ComboNode GetNextComboNode(AttackType attackType)
    {
        return currentComboNode == null ? comboTree.FindFirstNode(attackType) : currentComboNode.FindNextNode(attackType);
    }

    private void ResetAttackParameter()
    {
        currentComboCount = 0;
        isInputAttack = false;
        AttackEnd();
    }

    private void AttackEnd()
    {
        lookDirection = Vector3.zero;
    }

    private IEnumerator Attack()
    {
        isInputAttack = false;
        lookDirection = (Utilities.GetMouseWorldPos() - transform.position).normalized;
        LookUpdate();

        onAttack.Invoke(entity, entity.AttackType, currentComboCount);

        yield return new WaitForSeconds(entity.GetAttackDelay());

        if (isInputAttack)
            TryAttack();
        else
            ResetAttackParameter();
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
}
