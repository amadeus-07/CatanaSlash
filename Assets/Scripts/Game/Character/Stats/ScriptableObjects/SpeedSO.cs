using Core;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed", menuName = "ScriptableObjects/SpeedSO", order = 1)]
public sealed class SpeedSO : StatSO
{
    [SerializeField] private int _value;
    public override IStat Create()
    {
        return new Speed(_value);
    }
}