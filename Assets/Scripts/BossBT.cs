using UnityEngine;
using BehaviorTree;

public class BossBT : MonoBehaviour, IBehaviorTree
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Sequence seqAttack = new Sequence();
    private Sequence seqChase = new Sequence();
    private Sequence seqIdle = new Sequence();

    private Detect detect = new Detect();
    private ChargeAttack chargeAttack = new ChargeAttack();
    private CanAttack canAttack = new CanAttack();
    private HasTarget hasTarget = new HasTarget();
    private IsTargetInAttackRange isTargetInAttackRange = new IsTargetInAttackRange();
    private IsTargetInAttackRange isNotTargetInAttackRange = new IsTargetInAttackRange();
    private LookAtTarget lookAtTarget = new LookAtTarget();
    private Chase chase = new Chase();
    private Idle idle = new Idle();

    public void Init(EnemyAI enemyAI)
    {
        root.AddChild(detect);
        root.AddChild(selector);

        selector.AddChild(seqAttack);
        selector.AddChild(seqChase);
        selector.AddChild(seqIdle);

        seqAttack.AddChild(hasTarget);
        seqAttack.AddChild(isTargetInAttackRange);
        seqAttack.AddChild(canAttack);
        seqAttack.AddChild(lookAtTarget);
        seqAttack.AddChild(chargeAttack);

        seqChase.AddChild(hasTarget);
        seqChase.AddChild(isNotTargetInAttackRange);
        seqChase.AddChild(chase);

        seqIdle.AddChild(idle);

        detect.Init(enemyAI);
        chargeAttack.Init((BossAI)enemyAI);
        canAttack.Init(enemyAI);
        hasTarget.Init(enemyAI);
        isTargetInAttackRange.Init(enemyAI);
        isNotTargetInAttackRange.Init(enemyAI);
        isNotTargetInAttackRange.reverse = true;
        lookAtTarget.Init(enemyAI);
        chase.Init(enemyAI);
        idle.Init(enemyAI);
    }

    public void BTUpdate()
    {
        root.Invoke();
    }
}
