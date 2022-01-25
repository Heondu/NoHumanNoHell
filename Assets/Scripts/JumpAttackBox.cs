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

        target.TakeDamage(damage);
        Movement movement = target.GetComponent<Movement>();
        if (movement != null)
        {
            Vector3 direction = new Vector3(owner.transform.localScale.x, 0, 0).normalized;
            movement.Knockback(direction * 5);
        }
    }
}
