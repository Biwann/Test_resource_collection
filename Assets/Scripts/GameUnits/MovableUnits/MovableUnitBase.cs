using UnityEngine;

[RequireComponent(typeof(BehaviorStrategyBase))]
public abstract class MovableUnitBase : GameUnitBase
{
    [SerializeField] public MovableUnitParameters MovableUnitParameters { get; }

    public override void InitializeUnit(Color? factionColor)
    {
        base.InitializeUnit(factionColor);

        BehaviorController = GetComponent<BehaviorControllerBase>();
        BehaviorController.Enter();
    }

    private void Update()
    {
        BehaviorController.Execute();
    }

    private void OnDisable()
    {
        BehaviorController.Exit();
    }

    protected BehaviorControllerBase BehaviorController { get; private set;  }
}

