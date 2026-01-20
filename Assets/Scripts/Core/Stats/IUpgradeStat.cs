namespace Core
{
    public interface IUpgradeStat<T> where T : class, IStat
    {
        public void Upgrade(T stat);
    }

}