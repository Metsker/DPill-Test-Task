using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PlayerData;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace CodeBase.Infrastructure.States
{
    public class RestartState : IState
    {
        private readonly IGameFactory _gameFactory;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IPlayerProgressService _progressService;
        private readonly IStaticDataService _staticData;

        private GameObject _heroGameObject;

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

            _heroGameObject = _gameFactory.heroGameObject;

            ResetEnvironment();
            await ResetHero();
            ResetHud();
            ResetLoot();

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
            Object.Destroy(_heroGameObject);
            await _gameFactory.CreateHero(LevelStaticData().initialHeroPosition);
        }
        private void ResetHud()
        {
            _gameFactory.InitHud();
        }

        private void ResetLoot()
        {
            _progressService.lootData.Reset();
        }

        private LevelStaticData LevelStaticData()
        {
            return _staticData.ForLevel(SceneManager.GetActiveScene().name);
        }
    }
}
