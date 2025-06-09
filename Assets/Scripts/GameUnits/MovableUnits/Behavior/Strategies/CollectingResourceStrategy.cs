using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public sealed class CollectingResourceStrategy : BehaviorStrategyBase
{
    [SerializeField] private float _collectingRange = 0.5f;
    [SerializeField] private float _collectingTimeSeconds = 2f;
    [SerializeField] private ParticleSystem _collectionEffect;
    [SerializeField] private DebugSettings _debugSettings;

    public override void Enter(IBehaviorStrategyParameter parameter = null)
    {
        var resourceParameter = parameter as ResourceParameter;
        _resourceParameter = resourceParameter;
        _reserved = false;
        _collectingCoroutine = null;

        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = _collectingRange;

        var movableParams = GetComponent<MovableUnitBase>().MovableUnitParameters;
        _agent.speed = movableParams.MoveSpeed;
        _agent.angularSpeed = movableParams.RotationsSpeed;
        _agent.updateUpAxis = false;
        _agent.isStopped = false;
    }

    public override BehaviorStrategyState Execute()
    {
        if (_resourceParameter == null)
        {
            return BehaviorStrategyState.Failed;
        }

            if (_isCollected)
        {
            return BehaviorStrategyState.Success;
        }

        if (_resourceParameter.Resource == null 
            || _resourceParameter.Resource.IsReserved != _reserved)
        {
            return BehaviorStrategyState.Failed;
        }

        if (!_reserved)
        {
            _reserved = true;
            _resourceParameter.Resource.IsReserved = true;
        }

        var isMoved = TryMoveToResource();
        if (!isMoved)
        {
            if (_collectingCoroutine == null)
            {
                _collectingCoroutine = StartCoroutine(CollectResource());
            }
        }
        else
        {
            if (_collectingCoroutine != null)
            {
                StopCoroutine(_collectingCoroutine);
                _collectingCoroutine = null;
            }
        }

        return BehaviorStrategyState.InProcess;
    }

    public override IBehaviorStrategyParameter Exit()
    {
        return null;
    }

    private bool TryMoveToResource()
    {
        var resource = _resourceParameter.Resource;

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            return false;
        }
         
        if (!_agent.hasPath)
        {
            _agent.SetDestination(resource.transform.position);
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

    private IEnumerator CollectResource()
    {
        Debug.Log("Collecting");
        yield return new WaitForSeconds(_collectingTimeSeconds);

        _resourceParameter.Resource.Collect();
        _isCollected = true;

        Debug.Log("Collected");
    }

    private ResourceParameter _resourceParameter;
    private bool _reserved;
    private bool _isCollected;
    private Coroutine _collectingCoroutine;
    private NavMeshAgent _agent;
}