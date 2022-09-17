using System;
namespace CodeBase.Infrastructure.Data
{
    public class LootData
    {
        public int collectedInField { get; private set; }
        public int collectedInBank { get; private set; }

        public event Action Changed;

        public void Collect(Loot loot)
        {
            collectedInField += loot.value;
            Changed?.Invoke();
        }

        public void MoveToBank()
        {
            collectedInBank += collectedInField;
            collectedInField = 0;
            Changed?.Invoke();
        }

        public void Reset()
        {
            collectedInBank = 0;
            collectedInField = 0;
            Changed?.Invoke();
        }
    }
}
