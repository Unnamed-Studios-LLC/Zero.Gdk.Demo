namespace Zero.Demo.World.Component
{
    public struct ResourceComponent
    {
        public int Wood;
        public int MaxWood;
        public int SentWood;
        public int WoodRunningTotal;
        public readonly float SpawnTime;

        public ResourceComponent(int wood, int maxWood, float spawnTime)
        {
            Wood = wood;
            MaxWood = maxWood;
            SentWood = -1;
            WoodRunningTotal = 0;
            SpawnTime = spawnTime;
        }

        public int AvailableWoodSpace => MaxWood - Wood;

        public void Give(int amount)
        {
            Wood += amount;
            WoodRunningTotal += amount;
            if (Wood > MaxWood)
            {
                Wood = MaxWood;
            }
        }

        public int Take(int amount)
        {
            if (amount >= Wood)
            {
                return TakeAll();
            }

            Wood -= amount;
            return amount;
        }

        public int TakeAll()
        {
            var amount = Wood;
            Wood = 0;
            return amount;
        }
    }
}
