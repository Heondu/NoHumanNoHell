using BT;

public class CowBT : BehaviorTree
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Selector selectorAttack = new Selector();
    private Sequence sequenceAttack = new Sequence();
    private Sequence sequenceDefaultAttack = new Sequence();
    private Sequence sequenceChargeAttack = new Sequence();
    private Sequence sequenceJumpAttack = new Sequence();
    private Sequence sequenceChase = new Sequence();
    private Sequence sequenceIdle = new Sequence();

    public override void Init(EnemyAI enemyAI)
    {
        root.AddChild(new IsStop(enemyAI, true));
        root.AddChild(selector);
        
        selector.AddChild(sequenceAttack);
        
        sequenceAttack.AddChild(new IsDetect(enemyAI));
        sequenceAttack.AddChild(selectorAttack);
        
        selectorAttack.AddChild(sequenceDefaultAttack);
        selectorAttack.AddChild(sequenceChargeAttack);
        selectorAttack.AddChild(sequenceJumpAttack);
        
        selector.AddChild(sequenceChase);
        selector.AddChild(sequenceIdle);
        
        sequenceDefaultAttack.AddChild(new IsTargetInAttackRange(enemyAI, ((CowAI)enemyAI).defaultAttackRange));
        sequenceDefaultAttack.AddChild(new CanAttack(enemyAI, "DefaultAttack"));
        sequenceDefaultAttack.AddChild(new LookAtTarget(enemyAI));
        sequenceDefaultAttack.AddChild(new DefaultAttack(enemyAI));
        
        sequenceChargeAttack.AddChild(new IsTargetInAttackRange(enemyAI, ((CowAI)enemyAI).chargeAttackRange));
        sequenceChargeAttack.AddChild(new CanAttack(enemyAI, "ChargeAttack"));
        sequenceChargeAttack.AddChild(new LookAtTarget(enemyAI));
        sequenceChargeAttack.AddChild(new ChargeAttack(enemyAI));
        
        sequenceJumpAttack.AddChild(new IsTargetInAttackRange(enemyAI, ((CowAI)enemyAI).jumpAttackRange));
        sequenceJumpAttack.AddChild(new CanAttack(enemyAI, "JumpAttack"));
        sequenceJumpAttack.AddChild(new LookAtTarget(enemyAI));
        sequenceJumpAttack.AddChild(new JumpAttack(enemyAI));
        
        sequenceChase.AddChild(new CanAttack(enemyAI));
        sequenceChase.AddChild(new IsDetect(enemyAI));
        sequenceChase.AddChild(new Chase(enemyAI));
        
        sequenceIdle.AddChild(new Idle(enemyAI));
    }

    public override void BTUpdate()
    {
        root.Invoke();
    }
}
