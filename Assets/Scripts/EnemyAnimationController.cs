using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private Movement movement;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        animator.SetBool("isMove", movement.GetMoveDirection().x != 0 ? true : false);
        animator.SetBool("isInAir", !movement.IsGrounded());
    }
}
