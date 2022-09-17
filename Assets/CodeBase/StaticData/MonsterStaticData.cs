using UnityEngine;
using UnityEngine.AddressableAssets;
namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
    public class MonsterStaticData : ScriptableObject
    {
        public MonsterTypeId monsterTypeId;

        [Min(0)] public int minLoot;

        [Min(0)] public int maxLoot;

        [Range(0, 1)] public float lootChance;

        [Space] public AssetReferenceGameObject prefabReference;
    }
}
