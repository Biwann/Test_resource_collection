using System;
using UnityEngine;

public abstract class GameUnitBase : MonoBehaviour
{
    public virtual bool IsInitialized { get; set; } = false;
    public event Action OnUnitDestroyed = () => { };
    public Color? FactionColor { get; protected set; }

    private void OnDestroy()
    {
        OnUnitDestroyed();
    }

    public virtual void InitializeUnit(Color? factionColor)
    {
        FactionColor = factionColor;
        IsInitialized = true;
    }
}
