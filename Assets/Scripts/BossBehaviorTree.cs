using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class DefaultAttack : BTNode
    {
        private CowAI cowAI;

        public void Init(CowAI cowAI)
        {
            this.cowAI = cowAI;
        }

        public override bool Invoke()
        {
            cowAI.DefaultAttack();
            return reverse ? false : true;
        }
    }

    public class ChargeAttack : BTNode
    {
        private CowAI cowAI;

        public void Init(CowAI cowAI)
        {
            this.cowAI = cowAI;
        }

        public override bool Invoke()
        {
            cowAI.ChargeAttack();
            return reverse ? false : true;
        }
    }

    public class JumpAttack : BTNode
    {
        private CowAI cowAI;

        public void Init(CowAI cowAI)
        {
            this.cowAI = cowAI;
        }

        public override bool Invoke()
        {
            cowAI.JumpAttack();
            return reverse ? false : true;
        }
    }
}
