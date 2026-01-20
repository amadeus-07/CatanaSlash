using Core;
using UniRx;

public class Speed : ReactiveProperty<float>, IStat
{
    public Speed(float value)
    {
        Value = value;
    }
}