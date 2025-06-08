using System.Collections;
using UnityEngine;

public sealed class SearchingResourceStrategy : BehaviorStrategyBase
{
    public override void Enter(IBehaviorStrategyParameter parameter = null)
    {
        _foundResource = null;
        FindNearestNonReservedResource();
    }

    public override BehaviorStrategyState Execute()
    {
        if (_foundResource != null)
        {
            if (_findCoroutine != null)
            {
                StopCoroutine(_findCoroutine);
                _findCoroutine = null;
            }

            return BehaviorStrategyState.Success;
        }

        if (_findCoroutine == null)
        {
            _findCoroutine = StartCoroutine(WaitForResource());
        }

        return BehaviorStrategyState.InProcess;
    }

    public override IBehaviorStrategyParameter Exit()
    {
        return new ResourceParameter(_foundResource);

    }

    private void FindNearestNonReservedResource()
    {
        var currentPosition = transform.position;
        var resources = FindObjectsByType<Resource>(FindObjectsSortMode.None);

        Resource resource = null; 
        var minDistance = Mathf.Infinity;

        foreach (var r in resources)
        {
            var distance = Vector3.Distance(currentPosition, r.transform.position);
            if (!r.IsReserved && distance < minDistance)
            {
                resource = r;
                minDistance = distance;
            }
        }

        _foundResource = resource;
    }

    private IEnumerator WaitForResource()
    {
        while (_foundResource == null)
        {
            Debug.LogWarning("Couldnt find resource. Waiting 0.5 sec");
            yield return new WaitForSeconds(0.5f);

            FindNearestNonReservedResource(); 
        }
    }

    private Resource _foundResource;
    private Coroutine _findCoroutine;
}