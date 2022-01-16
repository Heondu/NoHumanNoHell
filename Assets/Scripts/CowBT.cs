using BehaviorTree;
using UnityEngine;

public class CowBT : MonoBehaviour, IBehaviorTree
{
    private Sequence root = new Sequence();
    private Selector selector = new Selector();
    private Selector selectorAttack = new Selector();
    private Sequence seqAttack = new Sequence();
    private Sequence seqDefaultAttack = new Sequence();
    private Sequence seqChargeAttack = new Sequence();
    private Sequence seqJumpAttack = new Sequence();
    private Sequence seqChase = new Sequence();
    private Sequence seqIdle = new Sequence();

    private IsStop isNotStop = new IsStop();
    private IsDetect isDetect = new IsDetect();
    private DefaultAttack defaultAttack = new DefaultAttack();
    private ChargeAttack chargeAttack = new ChargeAttack();
    private JumpAttack jumpAttack = new JumpAttack();
    private CanAttack canDefaultAttack = new CanAttack();
    private CanAttack canChargeAttack = new CanAttack();
    private CanAttack canJumpAttack = new CanAttack();
    private CanAttack canAttack = new CanAttack();
    private IsTargetInAttackRange isTargetInDefaultAttackRange = new IsTargetInAttackRange();
    private IsTargetInAttackRange isTargetInChargeAttackRange = new IsTargetInAttackRange();
    private IsTargetInAttackRange isTargetInJumpAttackRange = new IsTargetInAttackRange();
    private LookAtTarget lookAtTarget = new LookAtTarget();
    private Chase chase = new Chase();
    private Idle idle = new Idle();

    public void Init(EnemyAI enemyAI)
    {
        root.AddChild(isNotStop);
        root.AddChild(selector);

        selector.AddChild(seqAttack);

        seqAttack.AddChild(isDetect);
        seqAttack.AddChild(selectorAttack);
        
        selectorAttack.AddChild(seqDefaultAttack);
        selectorAttack.AddChild(seqChargeAttack);
        selectorAttack.AddChild(seqJumpAttack);
        
        selector.AddChild(seqChase);
        selector.AddChild(seqIdle);

        seqDefaultAttack.AddChild(isTargetInDefaultAttackRange);
        seqDefaultAttack.AddChild(canDefaultAttack);
        seqDefaultAttack.AddChild(lookAtTarget);
        seqDefaultAttack.AddChild(defaultAttack);

        seqChargeAttack.AddChild(isTargetInChargeAttackRange);
        seqChargeAttack.AddChild(canChargeAttack);
        seqChargeAttack.AddChild(lookAtTarget);
        seqChargeAttack.AddChild(chargeAttack);

        seqJumpAttack.AddChild(isTargetInJumpAttackRange);
        seqJumpAttack.AddChild(canJumpAttack);
        seqJumpAttack.AddChild(lookAtTarget);
        seqJumpAttack.AddChild(jumpAttack);

        seqChase.AddChild(canAttack);
        seqChase.AddChild(isDetect);
        seqChase.AddChild(chase);

        seqIdle.AddChild(idle);

        isNotStop.Init(enemyAI);
        isNotStop.reverse = true;
        isDetect.Init(enemyAI);
        defaultAttack.Init((CowAI)enemyAI);
        chargeAttack.Init((CowAI)enemyAI);
        jumpAttack.Init((CowAI)enemyAI);
        canDefaultAttack.Init(enemyAI, "DefaultAttack");
        canChargeAttack.Init(enemyAI, "ChargeAttack");
        canJumpAttack.Init(enemyAI, "JumpAttack");
        canAttack.Init(enemyAI);
        isTargetInDefaultAttackRange.Init(enemyAI, ((CowAI)enemyAI).DefaultAttackRange);
        isTargetInChargeAttackRange.Init(enemyAI, ((CowAI)enemyAI).ChargeAttackRange);
        isTargetInJumpAttackRange.Init(enemyAI, ((CowAI)enemyAI).JumpAttackRange);
        lookAtTarget.Init(enemyAI);
        chase.Init(enemyAI);
        idle.Init(enemyAI);
    }

    public void BTUpdate()
    {
        root.Invoke();
    }
}
