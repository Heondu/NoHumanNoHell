using UnityEngine;

public class SmokeAttackBox : MonoBehaviour
{
    private int damage;
    private Entity owner;

    public void Setup(int damage, Entity owner)
    {
        this.damage = damage;
        this.owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Entity target = collision.GetComponent<Entity>();
        target.TakeDamage(damage);
        Vector3 direction = new Vector3(owner.transform.localScale.x, 0, 0).normalized;
        Movement movement = target.GetComponent<Movement>();
        if (movement != null)
            movement.Knockback(direction);
        enabled = false;
    }
}
