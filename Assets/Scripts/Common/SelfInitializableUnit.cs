using UnityEngine;

public sealed class SelfInitializableUnit : MonoBehaviour
{
    [SerializeField] private Color FactionColor;

    public void Awake()
    {
        if (TryGetComponent<GameUnitBase>(out var unit))
        {
            unit.InitializeUnit(FactionColor);
        }
    }
}