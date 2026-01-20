using Core;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackDamage", menuName = "ScriptableObjects/AttackDamageSO", order = 1)]
public sealed class AttackDamageSO : StatSO
{
    [SerializeField] private int _value;
    public override IStat Create()
    {
        return new AttackDamage(_value);
    }
}