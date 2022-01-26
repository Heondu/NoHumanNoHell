using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class DefaultAttack : BTNode
    {
        private CowAI self;

        public DefaultAttack(EnemyAI self)
        {
            this.self = (CowAI)self;
        }

        public override bool Invoke()
        {
            self.DefaultAttack();
            return true;
        }
    }

    public class ChargeAttack : BTNode
    {
        private CowAI self;

        public ChargeAttack(EnemyAI self)
        {
            this.self = (CowAI)self;
        }

        public override bool Invoke()
        {
            self.ChargeAttack();
            return true;
        }
    }

    public class JumpAttack : BTNode
    {
        private CowAI self;

        public JumpAttack(EnemyAI self)
        {
            this.self = (CowAI)self;
        }

        public override bool Invoke()
        {
            self.JumpAttack();
            return true;
        }
    }

    public class BosalFirstAttack : BTNode
    {
        private BosalAI self;

        public BosalFirstAttack(EnemyAI self)
        {
            this.self = (BosalAI)self;
        }

        public override bool Invoke()
        {
            self.FirstAttack();

            return true;
        }
    }

    public class BosalSecondAttack : BTNode
    {
        private BosalAI self;

        public BosalSecondAttack(EnemyAI self)
        {
            this.self = (BosalAI)self;
        }

        public override bool Invoke()
        {
            self.SecondAttack();

            return true;
        }
    }

    public class BosalThirdAttack : BTNode
    {
        private BosalAI self;

        public BosalThirdAttack(EnemyAI self)
        {
            this.self = (BosalAI)self;
        }

        public override bool Invoke()
        {
            self.ThirdAttack();

            return true;
        }
    }

    public class BosalRandomAttack : BTNode
    {
        private BosalAI self;

        public BosalRandomAttack(EnemyAI self)
        {
            this.self = (BosalAI)self;
        }

        public override bool Invoke()
        {
            if (self.IsAttacking)
                return false;

                int rand = Random.Range(0, 3);
            if (rand == 0)
            {
                if (self.Entity.CanAttack("FirstAttack"))
                    self.FirstAttack();
            }
            else if (rand == 1)
            {
                if (self.Entity.CanAttack("SecondAttack"))
                    self.SecondAttack();
            }
            else if (rand == 2)
            {
                if (self.Entity.CanAttack("ThirdAttack"))
                    self.ThirdAttack();
            }

            return true;
        }
    }

    public class AsuraRandomAttack : BTNode
    {
        private AsuraAI self;

        public AsuraRandomAttack(EnemyAI self)
        {
            this.self = (AsuraAI)self;
        }

        public override bool Invoke()
        {
            if (self.IsAttacking)
                return false;

            int rand = Random.Range(0, 100);
            if (rand < 20)
            {
                if (self.Entity.CanAttack("FirstAttack"))
                    self.FirstAttack();
            }
            else if (rand < 60)
            {
                if (self.Entity.CanAttack("SecondAttack"))
                    self.SecondAttack();
            }
            else if (rand < 100)
            {
                if (self.Entity.CanAttack("ThirdAttack"))
                    self.ThirdAttack();
            }

            return true;
        }
    }
}
