using UnityEngine;

public sealed class CollectingResourcesBehavior : BehaviorControllerBase
{
    [SerializeField] private BehaviorStrategyBase _searchingStrategy;
    [SerializeField] private BehaviorStrategyBase _collectingStrategy;
    [SerializeField] private BehaviorStrategyBase _returningStrategy;

    public override void Enter()
    {
        _currentStrategy = _searchingStrategy;
        Debug.Log("Enter behavior");
    }

    public override void Execute()
    {
        if (_currentStrategy == null)
        {
            return;
        }

        var state = _currentStrategy.Execute();

        if (state == BehaviorStrategyState.InProcess)
        {
            return;
        }

        var parameter = _currentStrategy.Exit();
        if (state == BehaviorStrategyState.Success)
        {
            SuccessStateStrategyChange();
        }
        else
        {
            FailStateStrategyChange();
        }
        _currentStrategy.Enter(parameter);
    }

    public override void Exit()
    {
        _currentStrategy.Exit();
        _currentStrategy = null;
        Debug.Log("Exit behavior");
    }

    private void SuccessStateStrategyChange()
    {
        if (_currentStrategy == _searchingStrategy)
        {
            _currentStrategy = _collectingStrategy;
        }
        else if (_currentStrategy == _collectingStrategy)
        {
            _currentStrategy = _returningStrategy;
        }
        else if (_currentStrategy == _returningStrategy)
        {
            _currentStrategy = _searchingStrategy;
        }

        Debug.Log($"Success: Switched behavior strategy, now: '{_currentStrategy.GetType()}'");
    }

    private void FailStateStrategyChange()
    {
        _currentStrategy = _searchingStrategy;

        Debug.Log($"Fail: Switched behavior strategy, now: '{_currentStrategy.GetType()}'");
    }

    private BehaviorStrategyBase _currentStrategy;
}