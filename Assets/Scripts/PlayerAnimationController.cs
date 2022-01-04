using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Movement movement;

    private void Start()
    {
        ComponentSetup();
        EventSetup();
    }
    
    private void ComponentSetup()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
    }

    private void EventSetup()
    {
        GetComponent<PlayerController>().onAttack.AddListener(OnAttack);
    }

    private void Update()
    {
        animator.SetBool("isMove", movement.GetMoveDirection().x != 0 ? true : false);
        animator.SetBool("isInAir", !movement.IsGrounded());
    }

    private void OnAttack(Entity entity, AttackType attackType, int comboCount)
    {
        if (attackType == AttackType.MeleeAttack)
            animator.Play("Player_MeleeAttack" + comboCount);
        else
            animator.Play("Player_RangedAttack");
    }
}
