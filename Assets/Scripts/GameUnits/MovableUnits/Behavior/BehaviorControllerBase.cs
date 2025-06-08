using UnityEngine;

[RequireComponent(typeof(MovableUnitBase))]
public abstract class BehaviorControllerBase : MonoBehaviour
{
    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();
}