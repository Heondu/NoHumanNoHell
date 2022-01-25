using UnityEngine;

public class AsuraThirdAttackProjectile : Projectile
{
    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Entity>().TakeDamage(damage);
            Movement movement = collision.GetComponent<Movement>();
            if (movement != null)
                movement.Knockback((transform.position - transform.position).normalized);
        }
    }
}
