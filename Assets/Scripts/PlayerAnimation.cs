using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Movement movement;
    private PlayerAttack playerAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        animator.SetBool("isMove", movement.GetMoveDirection().x != 0 ? true : false);
        if (playerAttack.GetAttackDirection().x != 0) spriteRenderer.flipX = playerAttack.GetAttackDirection().x < 0 ? true : false;
        else if (movement.GetMoveDirection().x != 0) spriteRenderer.flipX = movement.GetMoveDirection().x < 0 ? true : false;

        animator.SetBool("isInAir", !movement.IsGrounded());
    }

    public void Play(string name)
    {
        animator.Rebind();
        animator.Play(name);
    }
}
