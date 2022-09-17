using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataMonstersLabel = "Monsters";
        private const string StaticDataGunsLabel = "Guns";
        private const string StaticDataLevelsLabel = "Levels";

        private readonly IAssetProvider _assetProvider;
        private Dictionary<GunTypeId, GunStaticData> _guns;
        private Dictionary<string, LevelStaticData> _levels;

        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async Task Load()
        {
            _monsters = await LoadMonsters();
            _guns = await LoadGuns();
            _levels = await LoadLevels();
            _windowConfigs = await LoadWindows();
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId)
        {
            return _monsters.TryGetValue(typeId, out MonsterStaticData staticData)
                ? staticData
                : null;
        }

        public GunStaticData ForGun(GunTypeId typeId)
        {
            return _guns.TryGetValue(typeId, out GunStaticData staticData)
                ? staticData
                : null;
        }

        public LevelStaticData ForLevel(string sceneKey)
        {
            return _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;
        }

        public WindowConfig ForWindow(WindowId windowId)
        {
            return _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig)
                ? windowConfig
                : null;
        }

        private async Task<Dictionary<string, LevelStaticData>> LoadLevels()
        {
            List<Task<LevelStaticData>> tasks = await _assetProvider.LoadAll<LevelStaticData>(StaticDataLevelsLabel);
            return tasks.ToDictionary(key => key.Result.levelKey, data => data.Result);
        }
        
        private async Task<Dictionary<MonsterTypeId, MonsterStaticData>> LoadMonsters()
        {
            List<Task<MonsterStaticData>> tasks =
                await _assetProvider.LoadAll<MonsterStaticData>(StaticDataMonstersLabel);
            return tasks.ToDictionary(key => key.Result.monsterTypeId, data => data.Result);
        }

        private async Task<Dictionary<GunTypeId, GunStaticData>> LoadGuns()
        {
            List<Task<GunStaticData>> tasks = await _assetProvider.LoadAll<GunStaticData>(StaticDataGunsLabel);
            return tasks.ToDictionary(key => key.Result.gunTypeId, data => data.Result);
        }


        private async Task<Dictionary<WindowId, WindowConfig>> LoadWindows()
        {
            WindowStaticData tasks = await _assetProvider.Load<WindowStaticData>(AssetAddress.WindowStaticData);
            return tasks.configs.ToDictionary(key => key.windowId, data => data);
        }
    }
}
