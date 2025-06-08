using System.Collections;
using UnityEngine;

public sealed class CollectingResourceStrategy : BehaviorStrategyBase
{
    [SerializeField] private float _collectingRange = 0.5f;
    [SerializeField] private float _collectingTimeSeconds = 2f;
    [SerializeField] private ParticleSystem _collectionEffect;

    public override void Enter(IBehaviorStrategyParameter parameter = null)
    {
        var resourceParameter = parameter as ResourceParameter;
        _resourceParameter = resourceParameter;
        _reserved = false;
        _collectingCoroutine = null;
    }

    public override BehaviorStrategyState Execute()
    {
        if (_isCollected)
        {
            return BehaviorStrategyState.Success;
        }

        if (_resourceParameter == null 
            || _resourceParameter.Resource == null 
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
        var distance = Vector3.Distance(transform.position, resource.transform.position);

        if (distance < _collectingRange)
        {
            return false;
        }

        // navmesh
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
}