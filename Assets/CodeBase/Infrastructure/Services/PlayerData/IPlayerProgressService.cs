using CodeBase.Infrastructure.Data;
namespace CodeBase.Infrastructure.Services.PlayerData
{
    public interface IPlayerProgressService : IService
    {
        LootData lootData { get; }
    }
}
