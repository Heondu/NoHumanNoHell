using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Detect : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            enemyAI.Detect();
            return reverse ? false : true;
        }
    }

    public class HasTarget : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            return reverse ? !enemyAI.HasTarget() : enemyAI.HasTarget();
        }
    }

    public class Patrol : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            enemyAI.Idle();
            return reverse ? false : true;
        }
    }

    public class Chase : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            enemyAI.Chase();
            return reverse ? false : true;
        }
    }

    public class Attack : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            enemyAI.Attack();
            return reverse ? false : true;
        }
    }

    public class LookAtTarget : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            enemyAI.LookAtTarget();
            return reverse ? false :  true;
        }
    }

    public class IsAttacking : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            return reverse ? !enemyAI.IsAttacking() : enemyAI.IsAttacking();
        }
    }

    public class IsTargetInMeleeRange : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            bool flag = reverse ? !enemyAI.IsTargetInMeleeRange() : enemyAI.IsTargetInMeleeRange();
            return flag;
        }
    }

    public class ChargeAttack : BTNode
    {
        private BossAI bossAI;

        public void Init(BossAI bossAI)
        {
            this.bossAI = bossAI;
        }

        public override bool Invoke()
        {
            bossAI.ChargeAttack();
            return reverse ? false : true;
        }
    }
}