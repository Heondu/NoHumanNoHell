using System.Collections;
using UnityEngine;

public class CowAI : EnemyAI
{
    [Header("Default Attack")]
    public float defaultAttackDamage;
    public float defaultAttackRange;
    public float defaultAttackPrepareTime;
    public float defaultAttackStopTime;
    public float defaultAttackCooldown;
    [Header("Charge Attack")]
    public float chargeAttackDamage;
    public float chargeAttackRange;
    public float chargeAttackPrepareTime;
    public float chargeAttackStopTime;
    public float chargeAttackCooldown;
    [Header("Jump Attack")]
    public float jumpAttackDamage;
    public float jumpAttackRange;
    public float jumpAttackPrepareTime;
    public float jumpAttackStopTime;
    public float jumpAttackCooldown;
    public float jumpAttackForce;
    [SerializeField] private AnimationCurve jumpAttackCurve;

    public void DefaultAttack()
    {
        IsAttacking = true;
        entity.SetAttackTimer("DefaultAttack", defaultAttackCooldown);
        entity.Status.SetValue(StatusType.MeleeAttackDamage, defaultAttackDamage);
        entity.Status.SetValue(StatusType.MeleeAttackDelay, defaultAttackStopTime);
        animator.Play("DefaultAttack_Prepare");
        WaitAndPlayAnim("DefaultAttack", defaultAttackPrepareTime);
    }

    public void ChargeAttack()
    {
        IsAttacking = true;
        entity.SetAttackTimer("ChargeAttack", chargeAttackCooldown);
        entity.Status.SetValue(StatusType.MeleeAttackDamage, chargeAttackDamage);
        entity.Status.SetValue(StatusType.MeleeAttackDelay, chargeAttackStopTime);
        animator.Play("ChargeAttack_Prepare");
        WaitAndPlayAnim("ChargeAttack", chargeAttackPrepareTime);
    }

    public void JumpAttack()
    {
        IsAttacking = true;
        entity.SetAttackTimer("JumpAttack", jumpAttackCooldown);
        entity.Status.SetValue(StatusType.MeleeAttackDamage, jumpAttackDamage);
        entity.Status.SetValue(StatusType.MeleeAttackDelay, jumpAttackStopTime);
        animator.Play("JumpAttack_Prepare");
        WaitAndPlayAnim("JumpAttack", jumpAttackPrepareTime);
    }

    public void DashToTarget()
    {
        LookAtTarget();
        Vector3 direction = new Vector3(Mathf.Sign(target.Position.x - entity.Position.x), 0, 0);
        movement.Dash(direction);
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

    public override void OnDead()
    {
        base.OnDead();
        target.GetComponent<Entity>().CanBeDamaged = false;
    }
}
