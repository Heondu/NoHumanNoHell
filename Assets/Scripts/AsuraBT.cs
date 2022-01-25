using BT;

public class AsuraBT : BehaviorTree
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Sequence sequenceRandomAttack = new Sequence();

    public override void Init(EnemyAI enemyAI)
    {
        root.AddChild(new IsStop(enemyAI, true));
        root.AddChild(selector);

        selector.AddChild(sequenceRandomAttack);

        sequenceRandomAttack.AddChild(new AsuraRandomAttack(enemyAI));
    }

    public override void BTUpdate()
    {
        root.Invoke();
    }
}
