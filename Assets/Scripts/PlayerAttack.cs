using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int meleeDamage;
    [SerializeField] private float meleeDistance;
    [SerializeField] private float meleeCooldown;

    [SerializeField] private int rangedDamage;
    [SerializeField] private float rangedDistance;
    [SerializeField] private float rangedCooldown;
    [SerializeField] private GameObject projectile;

    [SerializeField] private bool isDrawDebug = true;

    [SerializeField] private int maxCombo;
    private int currentCombo;

    private Movement movement;
    private Vector3 attackDirection;
    private PlayerAnimation playerAnimation;

    private bool isAttacking = false;
    private bool isMeleeInputOn;
    private bool isRangedInputOn;

    private void Start()
    {
        movement = GetComponent<Movement>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void MeleeAttackCheck()
    {
        if (isAttacking)
        {
            isMeleeInputOn = true;
        }
        else
        {
            isAttacking = true;
            MeleeAttack();
        }
    }

    public void RangedAttackCheck()
    {
        if (isAttacking)
        {
            isRangedInputOn = true;
        }
        else
        {
            isAttacking = true;
            RangedAttack();
        }
    }

    private void MeleeAttack()
    {
        Vector3 direction = (Utilities.GetMouseWorldPos() - transform.position).normalized;
        attackDirection = direction;
        movement.Dash(direction);

        isMeleeInputOn = false;
        currentCombo++;
        playerAnimation.Play("Player_MeleeAttack" + currentCombo);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Vector2.one, 0, direction, meleeDistance);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag(gameObject.tag)) continue;

            ILivingEntity entity = hit.collider.GetComponent<ILivingEntity>();
            if (entity != null)
            {
                entity.TakeDamage(meleeDamage, gameObject);
            }
        }

        StartCoroutine("Cooldown", meleeCooldown);
    }

    private void RangedAttack()
    {
        Vector3 direction = (Utilities.GetMouseWorldPos() - transform.position).normalized;
        attackDirection = direction;

        isRangedInputOn = false;
        currentCombo++;
        playerAnimation.Play("Player_RangedAttack");

        Projectile clone = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        clone.Setup(direction, rangedDistance, rangedDamage, gameObject);

        StartCoroutine("Cooldown", rangedCooldown);
    }

    private IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);

        if (currentCombo == maxCombo)
        {
            AttackEnd();
            yield break;
        }

        if (isMeleeInputOn)
        {
            MeleeAttack();
        }
        else if (isRangedInputOn)
        {
            RangedAttack();
        }
        else
        {
            AttackEnd();
        }
    }

    private void AttackEnd()
    {
        isAttacking = false;
        isMeleeInputOn = false;
        isRangedInputOn = false;
        currentCombo = 0;
        attackDirection = Vector3.zero;
        playerAnimation.Play("Player_Idle");
    }

    private void OnDrawGizmos()
    {
        if (!isDrawDebug) return;

        if (isAttacking)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, attackDirection * meleeDistance);
            Gizmos.DrawWireCube(transform.position + attackDirection * meleeDistance, Vector3.one);
        }
    }

    public Vector3 GetAttackDirection()
    {
        return attackDirection;
    }
}