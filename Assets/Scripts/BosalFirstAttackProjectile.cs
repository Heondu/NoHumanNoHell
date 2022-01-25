using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BosalFirstAttackProjectile : MonoBehaviour
{
    [SerializeField] private float warningTime;
    [SerializeField] private float delay;
    [SerializeField] private float castTime;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject attackMaker;
    [SerializeField] private UnityEvent onGrounded = new UnityEvent();

    private int damage;
    private GameObject instigator;

    public void Setup(int damage, GameObject instigator)
    {
        this.damage = damage;
        this.instigator = instigator;
        GetComponent<SpriteRenderer>().flipX = Random.Range(0, 2) == 0 ? true : false;

        StartCoroutine("Attack");
    }


    private IEnumerator Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, groundLayer);
        if (!hit)
        {
            Destroy(gameObject);
            yield break;
        }

        Vector3 newPos = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        attackMaker.transform.position = newPos;
        attackMaker.SetActive(true);

        yield return new WaitForSeconds(warningTime);

        attackMaker.SetActive(false);

        yield return new WaitForSeconds(delay);

        Vector3 originPos = transform.position;
        float current = 0;
        while (current < castTime)
        {
            current += Time.deltaTime;
            transform.position = Vector3.Lerp(originPos, newPos, current / castTime);
            yield return null;
        }

        onGrounded.Invoke();
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Entity>().TakeDamage(damage);
            Movement movement = collision.GetComponent<Movement>();
            if (movement != null)
                movement.Knockback((transform.position - transform.position).normalized);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
