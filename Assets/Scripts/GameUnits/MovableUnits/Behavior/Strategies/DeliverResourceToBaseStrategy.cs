using UnityEngine;
using UnityEngine.AI;

public sealed class DeliverResourceToBaseStrategy : BehaviorStrategyBase
{
    [SerializeField] private float _deliverRange = 1f;
    [SerializeField] private ParticleSystem _deliveryEffect;
    [SerializeField] private DebugSettings _debugSettings;

    public override void Enter(IBehaviorStrategyParameter parameter = null)
    {
        _homeBase = transform.parent;
        if (_homeBase == null)
        {
            Debug.LogError("Drone has no home base");
            return;
        }
        
        _targetPosition = _homeBase.position;

        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = _deliverRange;

        var movableParams = GetComponent<MovableUnitBase>().MovableUnitParameters;
        _agent.speed = movableParams.MoveSpeed;
        _agent.angularSpeed = movableParams.RotationsSpeed;
        _agent.updateUpAxis = false;
        _agent.isStopped = false;
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
            var factionBase = _homeBase.GetComponent<FactionBase>();
            factionBase.AddResource();
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
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.isStopped = true;
            return false;
        }

        if (!_agent.hasPath)
        {
            _agent.SetDestination(_targetPosition);
        }
        else if (_debugSettings.ShowDronePath)
        {
            var path = _agent.path;
            for (int i = 1; i < path.corners.Length; i++)
            {
                Debug.DrawLine(path.corners[i - 1], path.corners[i], Color.green);
            }
        }

        return true;
    }

    private Transform _homeBase;
    private Vector3 _targetPosition;
    private NavMeshAgent _agent;
}