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

    private IsDetect isDetect = new IsDetect();
    private IsDetect isNotDetect = new IsDetect();
    private Attack attack = new Attack();
    private CanAttack canAttack = new CanAttack();
    private IsTargetInAttackRange isTargetInMeleeRange = new IsTargetInAttackRange();
    private IsTargetInAttackRange isNotTargetInMeleeRange = new IsTargetInAttackRange();
    private LookAtTarget lookAtTarget = new LookAtTarget();
    private Chase chase = new Chase();
    private Patrol patrol = new Patrol();
    private Idle idle = new Idle();
    private IsClosePatrolPos isNotClosePatrolPos = new IsClosePatrolPos();
    private IsReachablePos isReachablePos = new IsReachablePos();
    private FindPatrolPos findPatrolPos = new FindPatrolPos();

    public void Init(EnemyAI enemyAI)
    {
        root.AddChild(selector);

        selector.AddChild(seqAttack);
        selector.AddChild(seqChase);
        selector.AddChild(seqPatrol);
        selector.AddChild(seqIdle);

        seqAttack.AddChild(isDetect);
        seqAttack.AddChild(isTargetInMeleeRange);
        seqAttack.AddChild(canAttack);
        seqAttack.AddChild(lookAtTarget);
        seqAttack.AddChild(attack);

        seqChase.AddChild(isDetect);
        seqChase.AddChild(isNotTargetInMeleeRange);
        seqChase.AddChild(chase);

        seqPatrol.AddChild(isNotDetect);
        seqPatrol.AddChild(findPatrolPos);
        seqPatrol.AddChild(isReachablePos);
        seqPatrol.AddChild(isNotClosePatrolPos);
        seqPatrol.AddChild(patrol);

        seqIdle.AddChild(idle);

        isDetect.Init(enemyAI);
        isNotDetect.Init(enemyAI);
        isNotDetect.reverse = true;
        attack.Init(enemyAI);
        canAttack.Init(enemyAI);
        isTargetInMeleeRange.Init(enemyAI);
        isNotTargetInMeleeRange.Init(enemyAI);
        isNotTargetInMeleeRange.reverse = true;
        lookAtTarget.Init(enemyAI);
        chase.Init(enemyAI);
        patrol.Init(enemyAI);
        idle.Init(enemyAI);
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
