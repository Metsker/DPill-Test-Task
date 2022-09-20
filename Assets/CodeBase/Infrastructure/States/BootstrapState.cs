using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PlayerData;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialScene = "Initial";
        private const string MainScene = "Main"; //До появления выбора уровня/загрузки из сохранения
        
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        private readonly GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public async void Enter()
        {
            await LoadStaticData();
            await _services.Single<IAssetProvider>().Instantiate();
            
            _sceneLoader.Load(InitialScene, EnterLoadLevel);
        }

        public void Exit() {}

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(MainScene);
        }

        private void RegisterServices()
        {
            _services.RegisterSingle(InputService());
            _services.RegisterSingle(InitedAssetService());

            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            _services.RegisterSingle<IStaticDataService>(new StaticDataService(_services.Single<IAssetProvider>()));
            _services.RegisterSingle<IPlayerProgressService>(new PlayerProgressService());

            _services.RegisterSingle<IUIFactory>(new UIFactory(
                _services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IGameStateMachine>()));

            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));

            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPlayerProgressService>(),
                _services.Single<IWindowService>()));
        }

        private async Task LoadStaticData()
        {
            await _services.Single<IStaticDataService>().Load();
        }

        private static IAssetProvider InitedAssetService()
        {
            IAssetProvider assetProvider = new AssetProvider();
            //assetProvider.Instantiate();
            return assetProvider;
        }

        private static IInputService InputService()
        {
            return Application.isEditor ? new StandaloneInputService() : new MobileInputService();
        }
    }
}
