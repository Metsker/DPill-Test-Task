using System.Collections.Generic;
using UnityEngine;
namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string levelKey;

        public List<EnemySpawnerData> enemySpawners;

        public BattleFieldData battleFieldData;

        public Vector3 initialHeroPosition;
    }
}
