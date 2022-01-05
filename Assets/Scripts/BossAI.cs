using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI
{
    [SerializeField] private float chargeAttackCastTime;
    [SerializeField] private float chargeAttackDelay;

    public void ChargeAttack()
    {
        StartCoroutine("ChargeAttackCo");
    }

    private IEnumerator ChargeAttackCo()
    {
        isAttacking = true;

        animator.Play("Charging");

        yield return new WaitForSeconds(chargeAttackCastTime);

        animator.Play("ChargeAttack");

        yield return new WaitForSeconds(chargeAttackDelay);

        isAttacking = false;
    }

    public void DashToTarget()
    {
        LookAtTarget();
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;
        movement.Dash(direction);
    }
}
