using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New FactionBaseData", menuName = "Faction Base Data", order = 52)]
public class FactionBaseData : ScriptableObject
{
    [SerializeField] private int _startResources = 0;
    [SerializeField] private int _resources = 0;

    public event Action<int> ResourcesChanged = _ => { };
    public int Resources => _resources;

    public void AddResource()
    {
        _resources++;
        ResourcesChanged(_resources);
    }

    public void Initialize()
    {
        _resources = _startResources;
        ResourcesChanged(_resources);
    }
}
