using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    public abstract void Init(EnemyAI enemyAI);

    public abstract void BTUpdate();
}
