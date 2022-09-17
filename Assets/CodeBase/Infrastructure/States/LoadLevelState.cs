using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
using UnityEngine.SceneManagement;
namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayLoadedState<string>
    {
        private readonly IGameFactory _gameFactory;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly SceneLoader _sceneLoader;
        private readonly GameStateMachine _stateMachine;
        private readonly IStaticDataService _staticData;
        private readonly IUIFactory _uiFactory;

        public LoadLevelState(
            GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain loadingCurtain,
            IGameFactory gameFactory,
            IStaticDataService staticData,
            IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _staticData = staticData;
            _uiFactory = uiFactory;
        }

        public async void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.Cleanup();
            await _gameFactory.WarmUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private async void OnLoaded()
        {
            await InitUIRoot();
            await InitGameWorld();

            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitUIRoot()
        {
            await _uiFactory.CreateUIRoot();
        }

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            await InitHero(levelData);

            await InitSpawners(levelData);

            await InitHud();
        }

        private LevelStaticData LevelStaticData()
        {
            return _staticData.ForLevel(SceneManager.GetActiveScene().name);
        }

        private async Task InitSpawners(LevelStaticData levelStaticData)
        {
            foreach (EnemySpawnerData spawnerData in levelStaticData.enemySpawners)
                await _gameFactory.CreateSpawner(
                    spawnerData, levelStaticData.battleFieldData);
        }

        private async Task InitHero(LevelStaticData levelStaticData)
        {
            await _gameFactory.CreateHero(levelStaticData.initialHeroPosition);
        }

        private async Task InitHud()
        {
            await _gameFactory.CreateHud();
        }
    }
}
