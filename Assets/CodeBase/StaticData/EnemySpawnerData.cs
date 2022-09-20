using System;
namespace CodeBase.StaticData
{
    [Serializable]
    public struct EnemySpawnerData
    {
        public MonsterTypeId monsterTypeId;
        public float spawnDelay;
        public float monsterCap;

        public EnemySpawnerData(MonsterTypeId monsterTypeId, float spawnDelay, float monsterCap)
        {
            this.monsterTypeId = monsterTypeId;
            this.spawnDelay = spawnDelay;
            this.monsterCap = monsterCap;
        }
    }
}
