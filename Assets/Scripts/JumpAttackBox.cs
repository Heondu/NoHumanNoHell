using UnityEngine;

public class JumpAttackBox : AttackBox
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity target = collision.GetComponent<Entity>();
        if (!target || target.CompareTag(owner.tag))
            return;
        if (IsWallBetweenTarget(collision.transform))
            return;

        target.TakeDamage(owner.gameObject, damage);
        Vector3 direction = new Vector3(owner.transform.localScale.x, 0, 0).normalized;
        target.GetComponent<Movement>().Knockback(direction * 5);
    }
}
