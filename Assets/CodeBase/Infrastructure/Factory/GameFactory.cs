using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Enemy.Components;
using CodeBase.Enemy.LootLogic;
using CodeBase.Hero;
using CodeBase.Hero.Components;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PlayerData;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawn;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IPlayerProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly IWindowService _windowService;

        public GameFactory(
            IAssetProvider assetProvider,
            IStaticDataService staticData,
            IPlayerProgressService progressService,
            IWindowService windowService)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _progressService = progressService;
            _windowService = windowService;
        }

        public List<Transform> monsters { get; } = new ();
        public List<GameObject> loot { get; } = new ();

        public GameObject heroGameObject { get; private set; }

        private GameObject _hud;

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(AssetAddress.Spawner);
            await _assetProvider.Load<GameObject>(AssetAddress.Loot);
            await _assetProvider.Load<GameObject>(AssetAddress.Projectile);
        }

        public async Task<GameObject> CreateHero(Vector3 at)
        {
            GameObject hero = await InstantiateAsync(AssetAddress.Hero, at);
            heroGameObject = hero;

            ConstructHeroComponents(hero);

            CameraFollow(hero);

            return hero;
        }

        public async Task<GameObject> CreateHud()
        {
            _hud = await InstantiateAsync(AssetAddress.Hud);

            InitHud();

            foreach (OpenWindowButton openWindowButton in _hud.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Construct(_windowService);

            return _hud;
        }
        public void InitHud()
        {
            _hud
                .GetComponentInChildren<HudLootCounter>()
                .Construct(_progressService.lootData, heroGameObject.GetComponent<HeroZoneInformer>());
        }

        public async Task<LootPiece> CreateLoot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetAddress.Loot);

            LootPiece lootPiece = Object.Instantiate(prefab).GetComponent<LootPiece>();
            lootPiece.Construct(_progressService.lootData);

            loot.Add(lootPiece.gameObject);
            lootPiece.Destroyed += () => loot.Remove(lootPiece.gameObject);

            return lootPiece;
        }

        public async Task CreateSpawner(EnemySpawnerData spawnerData, BattleFieldData battleFieldData)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AssetAddress.Spawner);
            Spawner spawner = Object.Instantiate(prefab)
                .GetComponent<Spawner>();

            spawner.Construct(this, spawnerData, battleFieldData, heroGameObject.GetComponent<HeroDeath>());

            spawner.StartSpawning();
        }

        public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Vector3 at)
        {
            MonsterStaticData monsterData = _staticData.ForMonster(typeId);

            GameObject prefab = await _assetProvider.Load<GameObject>(monsterData.prefabReference);
            GameObject monster = Object.Instantiate(prefab, at, Quaternion.identity);

            monsters.Add(monster.transform);
            monster.GetComponent<EnemyDeath>().Happened += () => monsters.Remove(monster.transform);

            ConstructMonsterComponents(monster);
            DefineLoot(monster, monsterData);

            return monster;
        }

        public async Task<Projectile> CreateProjectile(AssetReferenceGameObject assetReferenceGameObject)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(assetReferenceGameObject);

            Projectile projectile = Object.Instantiate(prefab)
                .GetComponent<Projectile>();

            return projectile;
        }

        public void Cleanup()
        {
            monsters.Clear();
            loot.Clear();

            _assetProvider.Cleanup();
        }

        private void ConstructHeroComponents(GameObject hero)
        {
            hero.GetComponent<ActorUI>().Construct(hero.GetComponent<IHealth>());
            hero.GetComponent<HeroLootCounter>().Construct(_progressService.lootData);
            HeroAttack heroAttack = hero.GetComponent<HeroAttack>();
            heroAttack.Construct(this);

            _windowService.Subscribe(hero.GetComponent<HeroDeath>());

            GunStaticData startGunData = _staticData.ForGun(GunTypeId.StartGun);
            heroAttack.SetGun(startGunData);
        }

        private void ConstructMonsterComponents(GameObject monster)
        {
            monster.GetComponent<ActorUI>().Construct(monster.GetComponent<IHealth>());
            monster.GetComponent<EnemyHeroChaser>().Construct(heroGameObject.transform);
            monster.GetComponent<EnemyHeroObserver>().Construct(heroGameObject.transform);
            monster.GetComponent<EnemyAttack>().Construct(heroGameObject.transform);
            monster.GetComponent<EnemyAggro>().Construct(heroGameObject.GetComponent<HeroZoneInformer>());
            monster.GetComponent<EnemyWinner>().Construct(heroGameObject.GetComponent<HeroDeath>());
        }

        private void DefineLoot(GameObject monster, MonsterStaticData monsterData)
        {
            LootSpawner lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            if (Random.value <= monsterData.lootChance)
            {
                lootSpawner.SetLoot(monsterData.minLoot, monsterData.maxLoot);
                lootSpawner.Construct(this);
            }
            else
            {
                lootSpawner.enabled = false;
            }
        }

        private static void CameraFollow(GameObject followTarget)
        {
            Camera.main
                .GetComponent<CameraFollower>()
                .Follow(followTarget);
        }

        private async Task<GameObject> InstantiateAsync(string prefabPath)
        {
            return await _assetProvider.Instantiate(prefabPath);
        }

        private async Task<GameObject> InstantiateAsync(string prefabPath, Vector3 at)
        {
            return await _assetProvider.Instantiate(prefabPath, at);
        }
    }
}
