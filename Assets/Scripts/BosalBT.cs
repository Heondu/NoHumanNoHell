using BT;

public class BosalBT : BehaviorTree
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Sequence sequenceFirstAttack = new Sequence();
    private Sequence sequenceSecondAttack = new Sequence();
    private Sequence sequenceThirdAttack = new Sequence();

    public override void Init(EnemyAI enemyAI)
    {
        root.AddChild(new IsStop(enemyAI, true));
        root.AddChild(new BosalRandomAttack(enemyAI));
        //root.AddChild(selector);

        //selector.AddChild(sequenceFirstAttack);
        //selector.AddChild(sequenceSecondAttack);
        //selector.AddChild(sequenceThirdAttack);

        //sequenceFirstAttack.AddChild(new CanAttack(enemyAI, "FirstAttack"));
        //sequenceFirstAttack.AddChild(new BosalFirstAttack(enemyAI));
        //
        //sequenceSecondAttack.AddChild(new CanAttack(enemyAI, "SecondAttack"));
        //sequenceSecondAttack.AddChild(new BosalSecondAttack(enemyAI));
        //
        //sequenceThirdAttack.AddChild(new CanAttack(enemyAI, "ThirdAttack"));
        //sequenceThirdAttack.AddChild(new BosalThirdAttack(enemyAI));
    }

    public override void BTUpdate()
    {
        root.Invoke();
    }
}
