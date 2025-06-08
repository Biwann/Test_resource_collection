using UnityEngine;

public sealed class DeliverResourceToBaseStrategy : BehaviorStrategyBase
{
    [SerializeField] private float _deliverRange = 1f;
    [SerializeField] private ParticleSystem _deliveryEffect;

    public override void Enter(IBehaviorStrategyParameter parameter = null)
    {
        _homeBase = transform.parent;
        if (_homeBase == null)
        {
            Debug.LogError("Drone has no home base");
            return;
        }
        
        _targetPosition = _homeBase.position;
    }

    public override BehaviorStrategyState Execute()
    {
        if (_homeBase == null)
        {
            return BehaviorStrategyState.Failed;
        }

        var isMoved = TryMoveToBase();
        if (!isMoved)
        {
            return BehaviorStrategyState.Success;
        }

        return BehaviorStrategyState.InProcess;
    }

    public override IBehaviorStrategyParameter Exit()
    {
        return null;
    }

    private bool TryMoveToBase()
    {
        var distance = Vector3.Distance(transform.position, _homeBase.transform.position);

        if (distance < _deliverRange)
        {
            return false;
        }

        // navmesh
        return true;
    }

    private Transform _homeBase;
    private Vector3 _targetPosition;
}