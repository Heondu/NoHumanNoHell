using UnityEngine;

namespace BT
{
    public class IsDetect : BTNode
    {
        private EnemyAI self;

        public IsDetect(EnemyAI self, bool reverse = false)
        {
            this.self = self;
            this.reverse = reverse;
        }

        public override bool Invoke()
        {
            bool value = self.GetComponentInChildren<DetectBox>().IsDetect();
            return reverse ? !value : value;
        }
    }

    public class Patrol : BTNode
    {
        private EnemyAI self;
        private Timer patrolTimer;

        public Patrol(EnemyAI self)
        {
            this.self = self;
            patrolTimer = new Timer(0);
        }

        public override bool Invoke()
        {
            if (patrolTimer.IsTimeEnd())
            {
                patrolTimer.SetTimer(self.PatrolTime);
                return true;
            }
            return false;
        }
    }

    public class Chase : BTNode
    {
        private EnemyAI self;

        public Chase(EnemyAI self)
        {
            this.self = self;
        }

        public override bool Invoke()
        {
            Vector3 direction = (self.Target.transform.position - self.transform.position).normalized;
            direction.y = 0;
            self.GetComponent<Movement>().Move(direction);
            self.MovePosition = self.Target.transform.position;
            return true;
        }
    }

    public class Attack : BTNode
    {
        private EnemyAI self;

        public Attack(EnemyAI self)
        {
            this.self = self;
        }

        public override bool Invoke()
        {
            self.Attack();
            return true;
        }
    }

    public class LookAtTarget : BTNode
    {
        private EnemyAI self;

        public LookAtTarget(EnemyAI self)
        {
            this.self = self;
        }

        public override bool Invoke()
        {
            Vector3 direction = (self.Target.transform.position - self.transform.position).normalized;
            float x = Mathf.Sign(direction.x) * Mathf.Abs(self.transform.localScale.x);
            self.transform.localScale = new Vector3(x, self.transform.localScale.y, self.transform.localScale.z);
            return true;
        }
    }

    public class CanAttack : BTNode
    {
        private EnemyAI self;
        private string key;

        public CanAttack(EnemyAI self, bool reverse = false)
        {
            this.self = self;
            this.reverse = reverse;
        }

        public CanAttack(EnemyAI self, string key, bool reverse = false)
        {
            this.self = self;
            this.key = key;
            this.reverse = reverse;
        }

        public override bool Invoke()
        {
            bool value;
            if (self.IsAttacking)
                return reverse ? true : false;
            if (key == "")
                value = self.Entity.CanAttack(self.Entity.AttackType.ToString());
            else
                value = self.Entity.CanAttack(key);

            return reverse ? !value : value;
        }
    }

    public class IsTargetInAttackRange : BTNode
    {
        private EnemyAI self;
        private float range;
        private bool useDefaultRange;

        public IsTargetInAttackRange(EnemyAI self, bool reverse = false)
        {
            this.self = self;
            this.reverse = reverse;
            useDefaultRange = true;
        }

        public IsTargetInAttackRange(EnemyAI self, float range, bool reverse = false)
        {
            this.self = self;
            this.range = range;
            this.reverse = reverse;
            useDefaultRange = false;
        }

        public override bool Invoke()
        {
            if (useDefaultRange)
                return reverse ? !IsInRange() : IsInRange();
            return reverse ? !IsInRange(range) : IsInRange(range);
        }

        public bool IsInRange()
        {
            float range = self.Entity.AttackType == AttackType.MeleeAttack ? self.Entity.Status.GetValue(StatusType.MeleeAttackRange) : self.Entity.Status.GetValue(StatusType.RangedAttackRange);
            return IsInRange(range);
        }

        public bool IsInRange(float range)
        {
            float distance = Vector3.SqrMagnitude(self.Target.transform.position - self.transform.position);
            return distance <= range * range;
        }
    }

    public class Idle : BTNode
    {
        private EnemyAI self;

        public Idle(EnemyAI self)
        {
            this.self = self;
        }

        public override bool Invoke()
        {
            self.GetComponent<Movement>().Move(Vector3.zero);

            return true;
        }
    }

    public class IsReachablePos : BTNode
    {
        private EnemyAI self;

        public IsReachablePos(EnemyAI self, bool reverse = false)
        {
            this.self = self;
            this.reverse = reverse;
        }

        public override bool Invoke()
        {
            Vector3 direction = (self.MovePosition - self.transform.position).normalized;

            LayerMask EnemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            LayerMask groundLayer = 1 << LayerMask.NameToLayer("Ground");

            Bounds bounds = self.GetComponent<CapsuleCollider2D>().bounds;
            RaycastHit2D[] hits = Physics2D.RaycastAll(bounds.center, direction, 0.5f, EnemyLayer + groundLayer);
            foreach (RaycastHit2D hit in hits)
                if (hit.collider.gameObject != self.gameObject)
                    return reverse ? true : false;

            RaycastHit2D hitGround = Physics2D.Raycast(self.transform.position + direction * 0.5f, Vector2.down, 0.1f, groundLayer);
            if (!hitGround)
                return reverse ? true : false;

            return reverse ? false : true;
        }
    }

    public class FindPatrolPos : BTNode
    {
        private EnemyAI self;

        public FindPatrolPos(EnemyAI self)
        {
            this.self = self;
        }

        public override bool Invoke()
        {
            float x = Random.Range(self.OriginPos.x - self.PatrolRange, self.OriginPos.x + self.PatrolRange);
            self.MovePosition = new Vector3(x, self.OriginPos.y, self.OriginPos.z);
            return true;
        }
    }

    public class MoveToPosition : BTNode
    {
        private EnemyAI self;

        public MoveToPosition(EnemyAI self)
        {
            this.self = self;
        }

        public override bool Invoke()
        {
            Vector3 direction = (self.MovePosition - self.transform.position).normalized;
            self.GetComponent<Movement>().Move(direction);
            return true;
        }
    }

    public class LookAtMoveDirection : BTNode
    {
        private EnemyAI self;

        public LookAtMoveDirection(EnemyAI self)
        {
            this.self = self;
        }

        public override bool Invoke()
        {
            Vector3 direction = (self.MovePosition - self.transform.position).normalized;
            float x = Mathf.Sign(direction.x) * Mathf.Abs(self.transform.localScale.x);
            self.transform.localScale = new Vector3(x, self.transform.localScale.y, self.transform.localScale.z);
            return true;
        }
    }

    public class IsStop : BTNode
    {
        private EnemyAI self;

        public IsStop(EnemyAI self, bool reverse = false)
        {
            this.self = self;
            this.reverse = reverse;
        }

        public override bool Invoke()
        {
            return reverse ? !self.IsStop : self.IsStop;
        }
    }
}

public class Timer
{
    private float lastTime;
    private float timer;

    public Timer(float timer)
    {
        SetTimer(timer);
    }

    public void SetTimer(float timer)
    {
        this.timer = timer;
        lastTime = Time.time;
    }

    public bool IsTimeEnd()
    {
        return Time.time - lastTime >= timer;
    }
}