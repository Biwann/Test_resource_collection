using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class FactionBase : StaticUnitBase
{
    [SerializeField] private FactionBaseData _data;
    [SerializeField] private TypeToPrefabData _unitTypeToPrefabData;
    [SerializeField] private GameObject _spawnPoint;

    public override void InitializeUnit(Color? factionColor)
    {
        base.InitializeUnit(factionColor);

        _data.Initialize();

        var color = FactionColor ?? Color.white;
        GetComponent<SpriteRenderer>().material.color = color;

        SpawnUnit(MovableUnitType.DroneCollector);
    }

    public void SpawnUnit(MovableUnitType unitType)
    {
        if (!_unitTypeToPrefabData.TypeToPrefabDictionary.TryGetValue(unitType, out var unitPrefab))
        {
            Debug.LogError($"Cant create {unitType}");
            return;
        }

        var unit = Instantiate(unitPrefab, _spawnPoint.transform.position, transform.rotation);

        unit.transform.SetParent(transform);
        var unitComponent = unit.GetComponent<MovableUnitBase>();
        unitComponent.InitializeUnit(FactionColor);

        _unitList.Add(unitComponent);
        unitComponent.OnUnitDestroyed += OnUnitDestroyed;

        void OnUnitDestroyed()
        {
            _unitList.Remove(unitComponent);
            unitComponent.OnUnitDestroyed -= OnUnitDestroyed;
        }
    }

    public void AddResource()
    {
        _data.AddResource();
    }

    private List<MovableUnitBase> _unitList = new();
}