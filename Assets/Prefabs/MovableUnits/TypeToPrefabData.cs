using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TypeToPrefabData", menuName = "Type To Prefab Data", order = 53)]
public class TypeToPrefabData : ScriptableObject
{
    [SerializeField] private SerializableDictionary<MovableUnitType, GameObject> _typeToPrefabDictionary;

    public Dictionary<MovableUnitType, GameObject> TypeToPrefabDictionary => _typeToPrefabDictionary;
}
