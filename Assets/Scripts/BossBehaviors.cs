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
            self.IsAttacking = true;
            self.Entity.SetAttackTimer("DefaultAttack", self.defaultAttackCooldown);
            self.Entity.Status.SetValue(StatusType.MeleeAttackDamage, self.defaultAttackDamage);
            self.Entity.Status.SetValue(StatusType.MeleeAttackDelay, self.defaultAttackStopTime);
            self.GetComponent<Animator>().Play("DefaultAttack_Prepare");
            self.WaitAndPlayAnim("DefaultAttack", self.defaultAttackPrepareTime);
            
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
            self.IsAttacking = true;
            self.Entity.SetAttackTimer("ChargeAttack", self.chargeAttackCooldown);
            self.Entity.Status.SetValue(StatusType.MeleeAttackDamage, self.chargeAttackDamage);
            self.Entity.Status.SetValue(StatusType.MeleeAttackDelay, self.chargeAttackStopTime);
            self.GetComponent<Animator>().Play("ChargeAttack_Prepare");
            self.WaitAndPlayAnim("ChargeAttack", self.chargeAttackPrepareTime);

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
            self.IsAttacking = true;
            self.Entity.SetAttackTimer("JumpAttack", self.jumpAttackCooldown);
            self.Entity.Status.SetValue(StatusType.MeleeAttackDamage, self.jumpAttackDamage);
            self.Entity.Status.SetValue(StatusType.MeleeAttackDelay, self.jumpAttackStopTime);
            self.GetComponent<Animator>().Play("JumpAttack_Prepare");
            self.WaitAndPlayAnim("JumpAttack", self.jumpAttackPrepareTime);

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
            self.IsAttacking = true;
            self.Entity.SetAttackTimer("FirstAttack", self.firstAttackCooldown);
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
            self.IsAttacking = true;
            self.Entity.SetAttackTimer("SecondAttack", self.secondAttackCooldown);
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
            self.IsAttacking = true;
            self.Entity.SetAttackTimer("ThirdAttack", self.thirdAttackCooldown);
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
}
