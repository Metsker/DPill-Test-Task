using System.Threading.Tasks;
using CodeBase.Hero.Components;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PlayerData;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawn;
using CodeBase.StaticData;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
namespace CodeBase.Infrastructure.States
{
    public class RestartState : IState
    {
        private readonly IGameFactory _gameFactory;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IPlayerProgressService _progressService;
        private readonly IStaticDataService _staticData;

        public RestartState(IGameStateMachine gameStateMachine, IStaticDataService staticData,
            IPlayerProgressService progressService, LoadingCurtain loadingCurtain, IGameFactory gameFactory)
        {
            _staticData = staticData;
            _progressService = progressService;
            _gameStateMachine = gameStateMachine;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
        }

        public async void Enter()
        {
            _loadingCurtain.Show();

            ResetEnvironment();
            
            await ResetHero();
            
            ResetLoot();

            ResetSpawners();

            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void ResetEnvironment()
        {
            foreach (Transform monster in _gameFactory.monsters)
                Object.Destroy(monster.gameObject);

            foreach (GameObject loot in _gameFactory.loot)
                Object.Destroy(loot);

            _gameFactory.monsters.Clear();
            _gameFactory.loot.Clear();
        }

        private async Task ResetHero()
        {
            GameObject heroGameObject = _gameFactory.heroGameObject;

            await MoveHeroToStartPosition(heroGameObject);

            foreach (IResettableOnRestart component in heroGameObject.GetComponents<IResettableOnRestart>())
                component.Reset();
        }
        
        private async Task MoveHeroToStartPosition(GameObject heroGameObject)
        {
            heroGameObject.GetComponent<HeroMovement>().Disable();

            await heroGameObject.transform
                .DOMove(LevelStaticData().initialHeroPosition, 0)
                .AsyncWaitForCompletion();
            
            heroGameObject.transform.rotation = Quaternion.identity;
        }

        private void ResetLoot()
        {
            _progressService.lootData.Reset();
        }
        
        private void ResetSpawners()
        {
            foreach (Spawner spawner in _gameFactory.spawners)
                spawner.RestartSpawning();
        }

        private LevelStaticData LevelStaticData()
        {
            return _staticData.ForLevel(SceneManager.GetActiveScene().name);
        }
    }
}
