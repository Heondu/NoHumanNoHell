using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float meleeDamage;
    [SerializeField] private float meleeDistance;
    [SerializeField] private float meleeCooldown;

    [SerializeField] private float rangedDamage;
    [SerializeField] private float rangedDistance;
    [SerializeField] private float rangedCooldown;
    [SerializeField] private GameObject projectile;

    [SerializeField] private bool isDrawDebug = true;

    private bool isMeleeAttacking = false;
    private bool isRangedAttacking = false;
    private Vector3 meleeDirection;

    public void MeleeAttack(Vector2 direction)
    {
        if (isMeleeAttacking) return;

        isMeleeAttacking = true;
        meleeDirection = direction;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Vector2.one, 0, direction, meleeDistance);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject == gameObject) continue;

            ILivingEntity entity = hit.collider.GetComponent<ILivingEntity>();
            if (entity != null)
            {
                entity.TakeDamage(meleeDamage, gameObject);
            }
        }

        StartCoroutine("MeleeCooldown");
    }

    private IEnumerator MeleeCooldown()
    {
        yield return new WaitForSeconds(meleeCooldown);

        isMeleeAttacking = false;
    }

    public void RangedAttack(Vector2 direction)
    {
        if (isRangedAttacking) return;

        isRangedAttacking = true;

        Projectile clone = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        clone.Setup(direction, rangedDistance, rangedDamage, gameObject);

        StartCoroutine("RangedCooldown");
    }

    private IEnumerator RangedCooldown()
    {
        yield return new WaitForSeconds(rangedCooldown);

        isRangedAttacking = false;
    }

    private void OnDrawGizmos()
    {
        if (!isDrawDebug) return;

        if (isMeleeAttacking)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, meleeDirection * meleeDistance);
            Gizmos.DrawWireCube(transform.position + meleeDirection * meleeDistance, Vector3.one);
        }
    }
}
