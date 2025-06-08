using UnityEngine;

public sealed class DroneCollector : MovableUnitBase
{
    public override void InitializeUnit(Color? factionColor)
    {
        base.InitializeUnit(factionColor);

        foreach (var c in GetComponentsInChildren<SpriteRenderer>())
        {
            c.material.color = FactionColor ?? Color.white;
        }

        Debug.Log("Drone inited");
    }
}