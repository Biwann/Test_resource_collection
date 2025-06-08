using UnityEngine;

public abstract class BehaviorStrategyBase : MonoBehaviour
{
    public abstract void Enter(IBehaviorStrategyParameter parameter = null);

    public abstract BehaviorStrategyState Execute();

    public abstract IBehaviorStrategyParameter Exit();
}