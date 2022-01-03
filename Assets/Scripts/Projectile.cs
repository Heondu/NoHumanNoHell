using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
                if (distance >= maxDistance)
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


        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.1f, groundMask);
        if (hit)
        {
            isReturn = true;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(instigator.tag)) return;

        ILivingEntity entity = collision.GetComponent<ILivingEntity>();
        if (entity != null)
        {
            entity.TakeDamage(damage, instigator);
        }
    }
}
