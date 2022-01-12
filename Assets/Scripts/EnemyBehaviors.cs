using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class IsDetect : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            return reverse ? !enemyAI.IsDetect() : enemyAI.IsDetect();
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
            enemyAI.Patrol();
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

    public class CanAttack : BTNode
    {
        private EnemyAI enemyAI;
        private string key;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public void Init(EnemyAI enemyAI, string key)
        {
            this.enemyAI = enemyAI;
            this.key = key;
        }

        public override bool Invoke()
        {
            if (key == null)
                return reverse ? !enemyAI.CanAttack() : enemyAI.CanAttack();
            return reverse ? !enemyAI.CanAttack(key) : enemyAI.CanAttack(key);
        }
    }

    public class IsTargetInAttackRange : BTNode
    {
        private EnemyAI enemyAI;
        private float range;
        private bool useDefaultRange;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
            useDefaultRange = true;
        }

        public void Init(EnemyAI enemyAI, float range)
        {
            this.enemyAI = enemyAI;
            this.range = range;
            useDefaultRange = false;
        }

        public override bool Invoke()
        {
            if (useDefaultRange)
                return reverse ? !enemyAI.IsTargetInAttackRange() : enemyAI.IsTargetInAttackRange();
            return reverse ? !enemyAI.IsTargetInAttackRange(range) : enemyAI.IsTargetInAttackRange(range);
        }
    }

    public class Idle : BTNode
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

    public class IsClosePatrolPos : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            return reverse ? !enemyAI.IsClosePatolPos() : enemyAI.IsClosePatolPos();
        }
    }

    public class IsReachablePos : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            return reverse ? !enemyAI.IsReachablePos() : enemyAI.IsReachablePos();
        }
    }

    public class FindPatrolPos : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            enemyAI.FindPatrolPos();
            return reverse ? false : true;
        }
    }

    public class IsStop : BTNode
    {
        private EnemyAI enemyAI;

        public void Init(EnemyAI enemyAI)
        {
            this.enemyAI = enemyAI;
        }

        public override bool Invoke()
        {
            return reverse ? !enemyAI.IsStop : enemyAI.IsStop;
        }
    }
}