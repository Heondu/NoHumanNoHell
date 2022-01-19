using BT;

public class EnemyBT : BehaviorTree
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Selector selectorPatrol = new Selector();
    private Sequence sequenceAttack = new Sequence();
    private Sequence sequenceChase = new Sequence();
    private Sequence sequencePatrol = new Sequence();
    private Sequence sequenceFindPatrolPos = new Sequence();
    private Sequence sequenceIdle = new Sequence();

    public override void Init(EnemyAI enemyAI)
    {
        root.AddChild(selector);
        
        selector.AddChild(sequenceAttack);
        selector.AddChild(sequenceChase);
        selector.AddChild(selectorPatrol);
        selector.AddChild(sequenceIdle);

        selectorPatrol.AddChild(sequenceFindPatrolPos);
        selectorPatrol.AddChild(sequencePatrol);
        
        sequenceAttack.AddChild(new IsDetect(enemyAI));
        sequenceAttack.AddChild(new IsTargetInAttackRange(enemyAI));
        sequenceAttack.AddChild(new CanAttack(enemyAI));
        sequenceAttack.AddChild(new LookAtTarget(enemyAI));
        sequenceAttack.AddChild(new Attack(enemyAI));
        
        sequenceChase.AddChild(new IsDetect(enemyAI));
        sequenceChase.AddChild(new IsTargetInAttackRange(enemyAI, true));
        sequenceChase.AddChild(new LookAtMoveDirection(enemyAI));
        sequenceChase.AddChild(new Chase(enemyAI));
        
        sequenceFindPatrolPos.AddChild(new Patrol(enemyAI));
        sequenceFindPatrolPos.AddChild(new FindPatrolPos(enemyAI));

        sequencePatrol.AddChild(new IsDetect(enemyAI, true));
        sequencePatrol.AddChild(new IsReachablePos(enemyAI));
        sequencePatrol.AddChild(new LookAtMoveDirection(enemyAI));
        sequencePatrol.AddChild(new MoveToPosition(enemyAI));
        
        sequenceIdle.AddChild(new Idle(enemyAI));
    }

    public override void BTUpdate()
    {
        root.Invoke();
    }
}
