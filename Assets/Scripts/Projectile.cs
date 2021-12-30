using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement movement;
    [SerializeField] private Vector2 direction;
    private float maxDistance;
    private float damage;
    private GameObject instigator;
    private bool isReturn = false;
    private Vector3 startPos;

    public void Setup(Vector2 direction, float maxDistance, float damage, GameObject instigator)
    {
        movement = GetComponent<Movement>();
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
            movement.MoveTo(direction);

            if (!isReturn)
            {
                float distance = Mathf.Abs(Vector2.SqrMagnitude(startPos - transform.position));
                if (distance >= maxDistance)
                {
                    isReturn = true;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == instigator) return;

        ILivingEntity entity = collision.GetComponent<ILivingEntity>();
        if (entity != null)
        {
            entity.TakeDamage(damage, instigator);
        }
    }
}
