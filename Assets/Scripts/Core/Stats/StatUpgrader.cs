namespace Core
{
    public sealed class StatUpgrader
    {
        private readonly StatContainer _container;
        
        public StatUpgrader(StatContainer container)
        {
            _container = container;
        }

        public void Upgrade<T>(IUpgradeStat<T> statUpgrade) where T : class, IStat
        {
            var stat = _container.Get<T>();
            statUpgrade.Upgrade(stat);
        }
    }
}