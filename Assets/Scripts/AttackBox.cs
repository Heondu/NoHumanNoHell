using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    [SerializeField] protected Entity owner;
    [SerializeField] protected LayerMask groundLayer;

    protected bool IsWallBetweenTarget(Transform target)
    {
        Vector3 origin = owner.transform.position;
        Vector3 direction = (target.position - origin).normalized;
        float distance = Vector3.Distance(origin, target.position);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, groundLayer);
        if (hit)
            return true;
        else
            return false;
    }

    public Entity GetOwner()
    {
        return owner;
    }
}
