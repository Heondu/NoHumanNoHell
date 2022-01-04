using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private ProjectileMover projectileMover;
    [SerializeField] private Vector2 direction;
    private float maxDistance;
    private int damage;
    private GameObject instigator;
    private bool isReturn = false;
    private Vector3 startPos;
    [SerializeField] private LayerMask groundMask;

    public void Setup(Vector2 direction, float maxDistance, int damage, GameObject instigator)
    {
        projectileMover = GetComponent<ProjectileMover>();
        this.direction = direction;
        this.maxDistance = maxDistance;
        this.damage = damage;
        this.instigator = instigator;
        startPos = instigator.transform.position;
    }

    private void Update()
    {
        if (instigator != null)
        {
            projectileMover.MoveTo(direction);

            if (!isReturn)
            {
                float distance = Mathf.Abs(Vector2.SqrMagnitude(startPos - transform.position));
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.1f, groundMask);
                if (distance >= maxDistance || hit)
                {
                    isReturn = true;
                    GetComponent<CircleCollider2D>().enabled = false;
                }
            }
            else
            {
                direction = (instigator.transform.position - transform.position).normalized;

                float distance = Mathf.Abs(Vector2.SqrMagnitude(instigator.transform.position - transform.position));
                if (distance <= 0.5f)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
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
            targetEntity.TakeDamage(instigator, damage);
        }
    }
}
