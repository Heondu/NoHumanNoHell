using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] private float maxDistance;

    private bool isReturn = false;
    private Vector3 startPos;
    private Entity entity;

    public override void Setup(Vector2 direction, int damage, GameObject instigator)
    {
        base.Setup(direction, damage, instigator);

        startPos = instigator.transform.position;
        entity = instigator.GetComponent<Entity>();
    }

    private void Update()
    {
        MoveUpdate();
        Rotate();
    }

    private void MoveUpdate()
    {
        if (instigator == null)
        {
            Destroy(gameObject);
            return;
        }

        Move();

        if (!isReturn)
        {
            ReturnCheckAndSet();
        }
        else
        {
            SetDirectionToTarget();
            DistanceCheckAndDestroy();
        }
    }

    private bool IsGreaterThanMaxDistance()
    {
        float distance = Mathf.Abs(Vector2.SqrMagnitude(startPos - transform.position));
        return distance >= maxDistance;
    }

    private bool IsHitTheWall()
    {
        return Physics2D.Raycast(transform.position, direction, 0.1f, groundLayer);
    }

    private void ReturnCheckAndSet()
    {
        if (IsGreaterThanMaxDistance() || IsHitTheWall())
        {
            isReturn = true;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    private void SetDirectionToTarget()
    {
        direction = (entity.Position - transform.position).normalized;
    }

    private void DistanceCheckAndDestroy()
    {
        float distance = Mathf.Abs(Vector2.SqrMagnitude(instigator.transform.position - transform.position));
        if (distance <= 0.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(instigator.tag)) return;

        Entity targetEntity = collision.GetComponent<Entity>();
        if (targetEntity != null)
        {
            targetEntity.TakeDamage(damage);
            Movement movement = collision.GetComponent<Movement>();
            if (movement != null)
                movement.Knockback((transform.position - transform.position).normalized);
        }
    }
}
