using UnityEngine;

public class DetectBox : MonoBehaviour
{
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private LayerMask groundLayer;
    private Entity target;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (detectLayer != (detectLayer | (1 << collision.gameObject.layer)))
            return;
        if (IsWallBetweenTarget(collision.transform))
            return;

        target = collision.GetComponent<Entity>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target == null) 
            return;
        if (collision.gameObject != target.gameObject) 
            return;

        target = null;
    }

    public bool IsDetect()
    {
        return target;
    }

    private bool IsWallBetweenTarget(Transform target)
    {
        Vector3 origin = transform.position;
        Vector3 direction = (target.position - origin).normalized;
        float distance = Vector3.Distance(origin, target.position);
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, groundLayer);
        if (hit)
            return true;
        else
            return false;
    }
}
