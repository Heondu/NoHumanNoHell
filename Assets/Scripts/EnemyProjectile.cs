using UnityEngine;

public class EnemyProjectile : Projectile
{
    private string instigatorTag;

    public override void Setup(Vector2 direction, int damage, GameObject instigator)
    {
        base.Setup(direction, damage, instigator);

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
            targetEntity.TakeDamage(gameObject, damage);
    }

    private void OnBecameVisible()
    {
        CancelInvoke();
    }

    private void OnBecameInvisible()
    {
        Invoke("Destroy", 3f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
