using UnityEngine;

/// <summary>
/// 플레이어의 애니메이션을 제어하는 클래스
/// </summary>
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
        //이동 애니메이션
        animator.SetBool("isMove", movement.GetMoveDirection().x != 0 ? true : false);

        //좌우 반전
        if (playerAttack.GetAttackDirection().x != 0) spriteRenderer.flipX = playerAttack.GetAttackDirection().x < 0 ? true : false;
        else if (movement.GetMoveDirection().x != 0) spriteRenderer.flipX = movement.GetMoveDirection().x < 0 ? true : false;

        //점프 애니메이션
        animator.SetBool("isInAir", !movement.IsGrounded());
    }

    public void Play(string name)
    {
        animator.Rebind();
        animator.Play(name);
    }
}
