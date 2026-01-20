using Core;
using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "ScriptableObjects/HealthSO", order = 1)]
public sealed class HealthSO : StatSO
{
    [SerializeField] private int _max;
    [SerializeField] private int _current;
    public override IStat Create()
    {
        return new Health(_max, _current);
    }
}
