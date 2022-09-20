using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy.LootLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services;
using CodeBase.Logic.EnemySpawn;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        public GameObject heroGameObject { get; }
        public List<Transform> monsters { get; }
        public List<Spawner> spawners { get; }
        public List<GameObject> loot { get; }
        Task WarmUp();
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Vector3 position);
        Task<Projectile> CreateProjectile(AssetReferenceGameObject assetReferenceGameObject);
        Task<LootPiece> CreateLoot();
        Task CreateSpawner(EnemySpawnerData spawnerData, BattleFieldData battleFieldData);
        void Cleanup();
    }
}
