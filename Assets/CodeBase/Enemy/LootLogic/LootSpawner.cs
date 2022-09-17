using CodeBase.Enemy.Components;
using CodeBase.Infrastructure.Data;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
namespace CodeBase.Enemy.LootLogic
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath enemyDeath;

        private IGameFactory _factory;
        private int _lootMax;
        private int _lootMin;

        private void Start()
        {
            enemyDeath.Happened += SpawnLoot;
        }

        public void Construct(IGameFactory factory)
        {
            _factory = factory;
        }

        private async void SpawnLoot()
        {
            LootPiece loot = await _factory.CreateLoot();
            loot.transform.position = gameObject.transform.position;

            Loot lootItem = GenerateLoot();
            loot.Initialize(lootItem);
        }

        private Loot GenerateLoot()
        {
            return new Loot
            {
                value = Random.Range(_lootMin, _lootMax)
            };
        }

        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }
    }
}
