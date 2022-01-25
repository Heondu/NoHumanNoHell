using UnityEngine;

public class EnemyProjectile : Projectile
{
    private Animator animator;
    private string instigatorTag;

    public override void Setup(Vector2 direction, int damage, GameObject instigator)
    {
        base.Setup(direction, damage, instigator);

        animator = GetComponent<Animator>();
        instigatorTag = instigator.tag;
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsColliderSelf(collision))
            return;
        CheckWallAndDestroy(collision);
        CheckPlayerAttackAndDestroy(collision);
        CheckEntityAndDoDamage(collision);
    }

    private bool IsColliderSelf(Collider2D collision)
    {
        if (instigator == null) 
            return false;
        if (collision.CompareTag(instigatorTag))
            return true;
        return false;
    }

    private void CheckWallAndDestroy(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Destroy();
    }

    private void CheckPlayerAttackAndDestroy(Collider2D collision)
    {
        MeleeAttackBox meleeAttackBox = collision.GetComponent<MeleeAttackBox>();
        if (meleeAttackBox == null)
            return;
        if (meleeAttackBox.GetOwner().gameObject.tag != instigatorTag)
            Destroy();
    }

    private void CheckEntityAndDoDamage(Collider2D collision)
    {
        Entity targetEntity = collision.GetComponent<Entity>();
        if (targetEntity != null)
        {
            animator.Play("Attack");
            targetEntity.TakeDamage(damage);
            Movement movement = collision.GetComponent<Movement>();
            if (movement != null)
                movement.Knockback((transform.position - transform.position).normalized);
        }
    }
}
