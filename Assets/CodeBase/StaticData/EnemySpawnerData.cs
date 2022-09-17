using System;
namespace CodeBase.StaticData
{
    [Serializable]
    public struct EnemySpawnerData
    {
        public MonsterTypeId monsterTypeId;
        public float secondsToSpawn;
        public float monsterCap;

        public EnemySpawnerData(MonsterTypeId monsterTypeId, float secondsToSpawn, float monsterCap)
        {
            this.monsterTypeId = monsterTypeId;
            this.secondsToSpawn = secondsToSpawn;
            this.monsterCap = monsterCap;
        }
    }
}
