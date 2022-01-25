using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected LayerMask groundMask;
    
    protected Vector2 direction;
    protected ProjectileMover projectileMover;
    protected int damage;
    protected GameObject instigator;

    public virtual void Setup(Vector2 direction, int damage, GameObject instigator)
    {
        projectileMover = GetComponent<ProjectileMover>();
        this.direction = direction;
        this.damage = damage;
        this.instigator = instigator;
    }

    protected void Move()
    {
        projectileMover.MoveTo(direction);
    }

    protected void Rotate()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnBecameVisible()
    {
        CancelInvoke();
    }

    private void OnBecameInvisible()
    {
        Invoke("Destroy", 3f);
    }

    protected void Destroy()
    {
        Destroy(gameObject);
    }
}
