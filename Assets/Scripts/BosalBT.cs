using BT;

public class BosalBT : BehaviorTree
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Sequence sequenceSecondAttack = new Sequence();
    private Sequence sequenceRandomAttack = new Sequence();

    public override void Init(EnemyAI enemyAI)
    {
        root.AddChild(new IsStop(enemyAI, true));
        root.AddChild(sequenceRandomAttack);
        //root.AddChild(selector);

        //selector.AddChild(sequenceSecondAttack);
        //selector.AddChild(sequenceRandomAttack);

        //sequenceSecondAttack.AddChild(new CanAttack(enemyAI, "SecondAttack"));
        //sequenceSecondAttack.AddChild(new BosalSecondAttack(enemyAI));

        sequenceRandomAttack.AddChild(new BosalRandomAttack(enemyAI));
    }

    public override void BTUpdate()
    {
        root.Invoke();
    }
}
