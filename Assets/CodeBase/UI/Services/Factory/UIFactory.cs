using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;

        private Transform _uiRoot;

        public UIFactory(
            IAssetProvider assetProvider,
            IStaticDataService staticDataService,
            IGameStateMachine stateMachine)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _stateMachine = stateMachine;
        }

        public async Task CreateUIRoot()
        {
            GameObject root = await _assetProvider.Instantiate(AssetAddress.UIRoot);
            _uiRoot = root.transform;
        }

        public async Task<RestartWindow> CreateRestart()
        {
            WindowConfig config = _staticDataService.ForWindow(WindowId.Restart);
            
            GameObject restartWindowObject = await Addressables.InstantiateAsync(config.windowPrefabReference, _uiRoot).Task;
            
            RestartWindow restartWindow = restartWindowObject.GetComponent<RestartWindow>();
            restartWindow.Construct(_stateMachine);
            return restartWindow;
        }
    }
}
