using UnityEngine;

[CreateAssetMenu(fileName = "New MovableUnitParameters", menuName = "Movable Data", order = 51)]
public class MovableUnitParameters : ScriptableObject
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _rotationSpeed = 120;

    public float MoveSpeed => _moveSpeed;
    public float RotationsSpeed => _rotationSpeed;
}
