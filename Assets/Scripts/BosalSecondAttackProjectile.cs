using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BosalSecondAttackProjectile : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float castTime;
    [SerializeField] private float distance;

    private int damage;
    private GameObject instigator;
    private new Transform camera;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private UnityEvent onStartMove;

    public void Setup(int damage, GameObject instigator)
    {
        this.damage = damage;
        this.instigator = instigator;
        camera = Camera.main.transform;

        leftHand.transform.position = new Vector2(camera.position.x - distance, transform.position.y);
        rightHand.transform.position = new Vector2(camera.position.x + distance, transform.position.y);

        StartCoroutine("Attack");
    }

    private IEnumerator Attack()
    {
        float current = 0;
        while (current < delay)
        {
            current += Time.deltaTime;

            leftHand.transform.position = new Vector2(camera.position.x - distance, leftHand.transform.position.y);
            rightHand.transform.position = new Vector2(camera.position.x + distance, rightHand.transform.position.y);

            yield return null;
        }

        onStartMove.Invoke();

        current = 0;
        while (current < castTime)
        {
            current += Time.deltaTime;

            float leftX = Mathf.Lerp(camera.position.x - distance, camera.position.x, current / castTime);
            float rightX = Mathf.Lerp(camera.position.x + distance, camera.position.x, current / castTime);
            leftHand.transform.position = new Vector2(leftX, leftHand.transform.position.y);
            rightHand.transform.position = new Vector2(rightX, rightHand.transform.position.y);

            yield return null;
        }

        leftHand.GetComponent<BoxCollider2D>().enabled = false;
        rightHand.GetComponent<BoxCollider2D>().enabled = false;

        instigator.GetComponent<EnemyAI>().onAttackEnd.Invoke();

        Destroy(leftHand, 1f);
        Destroy(rightHand, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Entity>().TakeDamage(damage / 2);
            Movement movement = collision.GetComponent<Movement>();
            if (movement != null)
                movement.Knockback((transform.position - transform.position).normalized);
            leftHand.GetComponent<BoxCollider2D>().enabled = false;
            rightHand.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
