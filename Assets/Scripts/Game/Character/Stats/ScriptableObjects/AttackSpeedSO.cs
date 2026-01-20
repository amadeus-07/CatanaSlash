using Core;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeed", menuName = "ScriptableObjects/AttackSpeedSO", order = 1)]
public sealed class AttackSpeedSO : StatSO
{
    [SerializeField] private float _value;
    public override IStat Create()
    {
        return new AttackSpeed(_value);
    }
}