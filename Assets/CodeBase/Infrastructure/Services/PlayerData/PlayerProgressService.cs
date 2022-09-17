using CodeBase.Infrastructure.Data;
namespace CodeBase.Infrastructure.Services.PlayerData
{
    public class PlayerProgressService : IPlayerProgressService
    {
        public PlayerProgressService()
        {
            lootData = new LootData();
        }

        public LootData lootData { get; }
    }
}
