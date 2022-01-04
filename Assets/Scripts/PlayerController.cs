using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Combo")]
    [SerializeField] private ComboTree comboTree;

    [HideInInspector] public UnityEvent<Entity, AttackType, int> onAttack;

    private Movement movement;
    private Entity entity;
    private ComboNode currentComboNode;

    private int currentComboCount;
    private bool isInputAttack = false;
    private bool isAttacking = false;
    private Vector3 lookDirection;

    private void Start()
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

    private void LookUpdate()
    {
        if (lookDirection.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(lookDirection.x), 1f, 1f);
        else if (movement.GetMoveDirection().x != 0)
            transform.localScale = new Vector3(Mathf.Sign(movement.GetMoveDirection().x), 1f, 1f);
    }

    private void MoveUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        movement.Move(new Vector3(x, 0, 0));
    }

    private void JumpUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            movement.Jump(Vector3.up);
    }

    private void AttackUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            InputAttack(AttackType.MeleeAttack);
        else if (Input.GetMouseButtonDown(1))
            InputAttack(AttackType.RangedAttack);
    }

    private void InputAttack(AttackType attackType)
    {
        entity.AttackType = attackType;
        isInputAttack = true;

        if (!isAttacking)
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
            isAttacking = true;

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
        lookDirection = Vector3.zero;
    }

    private IEnumerator Attack()
    {
        isInputAttack = false;
        lookDirection = (Utilities.GetMouseWorldPos() - transform.position).normalized;

        if (entity.AttackType == AttackType.RangedAttack)
            entity.Shoot(lookDirection);

        onAttack.Invoke(entity, entity.AttackType, currentComboCount);

        yield return new WaitForSeconds(entity.GetAttackDelay());

        isAttacking = false;

        if (isInputAttack)
            TryAttack();
        else
            ResetAttackParameter();
    }
}
