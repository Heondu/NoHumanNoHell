using UnityEngine;

public class MeleeAttackBox : AttackBox
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity target = collision.GetComponent<Entity>();
        if (!target || target.CompareTag(owner.tag))
            return;
        //if (IsWallBetweenTarget(collision.transform))
        //    return;

        target.TakeDamage(owner.Status.GetValue(StatusType.MeleeAttackDamage));
        Vector3 direction = new Vector3(owner.transform.localScale.x, 0, 0).normalized;
        if (owner.AttackType == AttackType.MeleeAttack)
        {
            Movement movement = target.GetComponent<Movement>();
            if (movement != null)
                movement.Knockback(direction);
        }
        if (owner.AttackType == AttackType.StrongMeleeAttack)
        {
            Movement movement = target.GetComponent<Movement>();
            if (movement != null)
                movement.Knockback(direction * 3);
        }
    }
}
