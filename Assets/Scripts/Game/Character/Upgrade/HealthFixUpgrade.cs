using Core;

public sealed class HealthFixUpgrade : IUpgradeStat<Health>
{
    public HealthFixUpgrade(int value)
    {
        _value = value;
    }

    private readonly int _value;
    public void Upgrade(Health stat)
    {
        stat.Max.Value += _value;
    }
}