using UnityEngine;
using BehaviorTree;

public class BossBT : MonoBehaviour, IBehaviorTree
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Sequence seqAttack = new Sequence();
    private Sequence seqChase = new Sequence();
    private Sequence seqPatrol = new Sequence();

    private Detect detect = new Detect();
    private ChargeAttack chargeAttack = new ChargeAttack();
    private IsAttacking isAttacking = new IsAttacking();
    private IsAttacking isNotAttacking = new IsAttacking();
    private HasTarget hasTarget = new HasTarget();
    private IsTargetInMeleeRange isTargetInMeleeRange = new IsTargetInMeleeRange();
    private IsTargetInMeleeRange isNotTargetInMeleeRange = new IsTargetInMeleeRange();
    private Chase chase = new Chase();
    private Patrol patrol = new Patrol();

    public void Init(EnemyAI enemyAI)
    {
        root.AddChild(detect);
        root.AddChild(selector);

        selector.AddChild(seqAttack);
        selector.AddChild(seqChase);
        selector.AddChild(seqPatrol);

        seqAttack.AddChild(hasTarget);
        seqAttack.AddChild(isTargetInMeleeRange);
        seqAttack.AddChild(isNotAttacking);
        seqAttack.AddChild(chargeAttack);

        seqChase.AddChild(hasTarget);
        seqChase.AddChild(isNotTargetInMeleeRange);
        seqChase.AddChild(chase);

        seqPatrol.AddChild(patrol);

        detect.Init(enemyAI);
        chargeAttack.Init((BossAI)enemyAI);
        isAttacking.Init(enemyAI);
        isNotAttacking.Init(enemyAI);
        isNotAttacking.reverse = true;
        hasTarget.Init(enemyAI);
        isTargetInMeleeRange.Init(enemyAI);
        isNotTargetInMeleeRange.Init(enemyAI);
        isNotTargetInMeleeRange.reverse = true;
        chase.Init(enemyAI);
        patrol.Init(enemyAI);
    }

    public void BTUpdate()
    {
        root.Invoke();
    }
}
