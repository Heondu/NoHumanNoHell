using UnityEngine;

public class MeleeAttackBox : MonoBehaviour
{
    [SerializeField] private Entity owner;

    [SerializeField] LayerMask groundLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity targetEntity = collision.GetComponent<Entity>();
        if (!targetEntity || targetEntity.CompareTag(owner.tag))
            return;

        Vector3 origin = owner.transform.position;
        Vector3 direction = (collision.transform.position - origin).normalized;
        float distance = Vector3.Distance(origin, collision.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, groundLayer);
        if (hit)
            return;

        targetEntity.TakeDamage(owner.gameObject, owner.Status.GetValue(StatusType.MeleeAttackDamage));
    }
}
