using UnityEngine;
using BehaviorTree;

public class EnemyBT : MonoBehaviour, IBehaviorTree
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Sequence seqAttack = new Sequence();
    private Sequence seqChase = new Sequence();
    private Sequence seqPatrol = new Sequence();
    private Sequence seqIdle = new Sequence();

    private Detect detect = new Detect();
    private Attack attack = new Attack();
    private CanAttack canAttack = new CanAttack();
    private HasTarget hasTarget = new HasTarget();
    private IsTargetInAttackRange isTargetInMeleeRange = new IsTargetInAttackRange();
    private IsTargetInAttackRange isNotTargetInMeleeRange = new IsTargetInAttackRange();
    private LookAtTarget lookAtTarget = new LookAtTarget();
    private Chase chase = new Chase();
    private Patrol patrol = new Patrol();
    private Idle idle = new Idle();
    private HasTarget hasNotTarget = new HasTarget();
    private IsClosePatrolPos isNotClosePatrolPos = new IsClosePatrolPos();
    private IsReachablePos isReachablePos = new IsReachablePos();
    private FindPatrolPos findPatrolPos = new FindPatrolPos();

    public void Init(EnemyAI enemyAI)
    {
        root.AddChild(detect);
        root.AddChild(selector);

        selector.AddChild(seqAttack);
        selector.AddChild(seqChase);
        selector.AddChild(seqPatrol);
        selector.AddChild(seqIdle);

        seqAttack.AddChild(hasTarget);
        seqAttack.AddChild(isTargetInMeleeRange);
        seqAttack.AddChild(canAttack);
        seqAttack.AddChild(lookAtTarget);
        seqAttack.AddChild(attack);

        seqChase.AddChild(hasTarget);
        seqChase.AddChild(isNotTargetInMeleeRange);
        seqChase.AddChild(chase);

        seqPatrol.AddChild(hasNotTarget);
        seqPatrol.AddChild(findPatrolPos);
        seqPatrol.AddChild(isReachablePos);
        seqPatrol.AddChild(isNotClosePatrolPos);
        seqPatrol.AddChild(patrol);

        seqIdle.AddChild(idle);

        detect.Init(enemyAI);
        attack.Init(enemyAI);
        canAttack.Init(enemyAI);
        hasTarget.Init(enemyAI);
        isTargetInMeleeRange.Init(enemyAI);
        isNotTargetInMeleeRange.Init(enemyAI);
        isNotTargetInMeleeRange.reverse = true;
        lookAtTarget.Init(enemyAI);
        chase.Init(enemyAI);
        patrol.Init(enemyAI);
        idle.Init(enemyAI);
        hasNotTarget.Init(enemyAI);
        hasNotTarget.reverse = true;
        isNotClosePatrolPos.Init(enemyAI);
        isNotClosePatrolPos.reverse = true;
        isReachablePos.Init(enemyAI);
        findPatrolPos.Init(enemyAI);
    }

    public void BTUpdate()
    {
        root.Invoke();
    }
}
