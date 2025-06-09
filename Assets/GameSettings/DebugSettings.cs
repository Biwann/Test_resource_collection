using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New DebugSettings", menuName = "Debug Settings", order = 54)]
public class DebugSettings : ScriptableObject
{
    [SerializeField] private int _dronesPerFaction = 3;
    [SerializeField] private bool _showDronePath = true;

    public event Action DebugSettingsChanged = () => { };

    public int DronesPerFaction 
    { 
        get => _dronesPerFaction;
        set
        {
            _dronesPerFaction = value;
            DebugSettingsChanged();
        }
    }

    public bool ShowDronePath
    {
        get => _showDronePath;
        set
        {
            _showDronePath = value;
            DebugSettingsChanged();
        }
    }
}
