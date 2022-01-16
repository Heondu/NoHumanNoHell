using System.Collections;
using UnityEngine;

public class CowAI : EnemyAI
{
    [Header("Default Attack")]
    [SerializeField] private float defaultAttackDamage;
    [SerializeField] private float defaultAttackRange;
    [SerializeField] private float defaultAttackPrepareTime;
    [SerializeField] private float defaultAttackStopTime;
    [SerializeField] private float defaultAttackCooldown;
    [Header("Charge Attack")]
    [SerializeField] private float chargeAttackDamage;
    [SerializeField] private float chargeAttackRange;
    [SerializeField] private float chargeAttackPrepareTime;
    [SerializeField] private float chargeAttackStopTime;
    [SerializeField] private float chargeAttackCooldown;
    [Header("Jump Attack")]
    [SerializeField] private float jumpAttackDamage;
    [SerializeField] private float jumpAttackRange;
    [SerializeField] private float jumpAttackPrepareTime;
    [SerializeField] private float jumpAttackStopTime;
    [SerializeField] private float jumpAttackCooldown;
    [SerializeField] private float jumpAttackForce;
    [SerializeField] private AnimationCurve jumpAttackCurve;

    public float DefaultAttackRange => defaultAttackRange;
    public float ChargeAttackRange => chargeAttackRange;
    public float JumpAttackRange => jumpAttackRange;

    public void DefaultAttack()
    {
        isAttacking = true;
        entity.SetAttackTimer("DefaultAttack", defaultAttackCooldown);
        entity.Status.SetValue(StatusType.MeleeAttackDamage, defaultAttackDamage);
        entity.Status.SetValue(StatusType.MeleeAttackDelay, defaultAttackStopTime);
        animator.Play("DefaultAttack_Prepare");
        WaitAndPlayAnim("DefaultAttack", defaultAttackPrepareTime);
    }

    public void ChargeAttack()
    {
        isAttacking = true;
        entity.SetAttackTimer("ChargeAttack", chargeAttackCooldown);
        entity.Status.SetValue(StatusType.MeleeAttackDamage, chargeAttackDamage);
        entity.Status.SetValue(StatusType.MeleeAttackDelay, chargeAttackStopTime);
        animator.Play("ChargeAttack_Prepare");
        WaitAndPlayAnim("ChargeAttack", chargeAttackPrepareTime);
    }

    public void DashToTarget()
    {
        LookAtTarget();
        Vector3 direction = new Vector3(Mathf.Sign(target.Position.x - entity.Position.x), 0, 0);
        movement.Dash(direction);
    }

    public void JumpAttack()
    {
        isAttacking = true;
        entity.SetAttackTimer("JumpAttack", jumpAttackCooldown);
        entity.Status.SetValue(StatusType.MeleeAttackDamage, jumpAttackDamage);
        entity.Status.SetValue(StatusType.MeleeAttackDelay, jumpAttackStopTime);
        animator.Play("JumpAttack_Prepare");
        WaitAndPlayAnim("JumpAttack", jumpAttackPrepareTime);
    }

    public void JumpToTarget()
    {
        StopCoroutine("JumpAttackCo");
        StartCoroutine("JumpAttackCo");
    }

    private IEnumerator JumpAttackCo()
    {
        yield return null;

        Vector3 start = transform.position;
        Vector3 end = target.transform.position;
        float percent = 0;
        while (percent < 0.99f)
        {
            percent = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float height = jumpAttackCurve.Evaluate(percent) * jumpAttackForce;
            transform.position = Vector2.Lerp(start, end, percent) + new Vector2(0, height);
            
            yield return null;
        }
        animator.Play("JumpAttack_After");
    }
}
