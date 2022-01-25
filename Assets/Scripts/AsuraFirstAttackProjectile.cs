using UnityEngine;

public class AsuraFirstAttackProjectile : Projectile
{
    private bool isStop = false;
    private bool isPlayerEnter = false;

    private void Update()
    {
        if (isStop)
            return;

        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isStop = true;
            Disable();
        }
        else if (!isStop && !isPlayerEnter && collision.CompareTag("Player"))
        {
            isPlayerEnter = true;
            collision.GetComponent<Entity>().TakeDamage(damage);
            Movement movement = collision.GetComponent<Movement>();
            if (movement != null)
                movement.Knockback((transform.position - transform.position).normalized);
        }
    }

    private void Disable()
    {
        isPlayerEnter = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<FadeEffect>().Fade();
    }
}
