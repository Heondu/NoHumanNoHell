using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Movement movement;

    private void Awake()
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
        GetComponent<PlayerController>().onJump.AddListener(OnJump);
    }

    private void Update()
    {
        animator.SetBool("isMove", movement.GetMoveDirection().x != 0 ? true : false);
        animator.SetBool("isInAir", !movement.IsGrounded());
    }

    private void OnAttack(Entity entity, AttackType attackType, int comboCount)
    {
        switch (attackType)
        {
            case AttackType.MeleeAttack: animator.Play("MeleeAttack" + comboCount); break;
            case AttackType.StrongMeleeAttack: animator.Play("MeleeAttack_Strong"); break;
            case AttackType.RangedAttack: animator.Play("RangedAttack"); break;
        }  
    }

    private void OnJump()
    {
        animator.Rebind();
    }
}
