using UnityEngine;

public class MeleeAttackBox : MonoBehaviour
{
    [SerializeField] private Entity owner;
    [SerializeField] LayerMask groundLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity target = collision.GetComponent<Entity>();
        if (!target || target.CompareTag(owner.tag))
            return;
        if (IsWallBetweenTarget(collision.transform))
            return;

        target.TakeDamage(owner.gameObject, owner.Status.GetValue(StatusType.MeleeAttackDamage));
        Vector3 direction = new Vector3(owner.transform.localScale.x, 0, 0).normalized;
        if (owner.AttackType == AttackType.MeleeAttack)
        {
            target.GetComponent<Movement>().Knockback(direction);
        }
        if (owner.AttackType == AttackType.StrongMeleeAttack)
        {
            target.GetComponent<Movement>().Knockback(direction * 3);
        }
    }

    private bool IsWallBetweenTarget(Transform target)
    {
        Vector3 origin = owner.transform.position;
        Vector3 direction = (target.position - origin).normalized;
        float distance = Vector3.Distance(origin, target.position);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, groundLayer);
        if (hit)
            return true;
        else 
            return false;
    }

    public Entity GetOwner()
    {
        return owner;
    }
}
