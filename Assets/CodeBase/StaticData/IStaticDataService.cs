using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
namespace CodeBase.StaticData
{
    public interface IStaticDataService : IService
    {
        Task Load();
        MonsterStaticData ForMonster(MonsterTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
        WindowConfig ForWindow(WindowId windowId);
        GunStaticData ForGun(GunTypeId typeId);
    }
}
