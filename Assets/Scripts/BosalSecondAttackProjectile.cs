using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosalSecondAttackProjectile : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float castTime;
    [SerializeField] private float distance;

    private int damage;
    private GameObject instigator;
    private Transform target;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    public void Setup(int damage, GameObject instigator, Transform target)
    {
        this.damage = damage;
        this.instigator = instigator;
        this.target = target;

        leftHand.transform.position = new Vector2(transform.position.x - distance, transform.position.y);
        rightHand.transform.position = new Vector2(transform.position.x + distance, transform.position.y);

        StartCoroutine("Attack");
    }

    private IEnumerator Attack()
    {
        float current = 0;
        while (current < delay)
        {
            current += Time.deltaTime;

            leftHand.transform.position = new Vector2(target.position.x - distance, leftHand.transform.position.y);
            rightHand.transform.position = new Vector2(target.position.x + distance, rightHand.transform.position.y);

            yield return null;
        }

        current = 0;
        while (current < castTime)
        {
            current += Time.deltaTime;

            float leftX = Mathf.Lerp(target.position.x - distance, target.position.x, current / castTime);
            float rightX = Mathf.Lerp(target.position.x + distance, target.position.x, current / castTime);
            leftHand.transform.position = new Vector2(leftX, leftHand.transform.position.y);
            rightHand.transform.position = new Vector2(rightX, rightHand.transform.position.y);

            yield return null;
        }

        leftHand.GetComponent<BoxCollider2D>().enabled = false;
        rightHand.GetComponent<BoxCollider2D>().enabled = false;

        Destroy(leftHand, 1f);
        Destroy(rightHand, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Entity>().TakeDamage(instigator, damage / 2);
            leftHand.GetComponent<BoxCollider2D>().enabled = false;
            rightHand.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
