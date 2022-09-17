using UnityEngine;
using UnityEngine.AddressableAssets;
namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "GunData", menuName = "StaticData/Gun", order = 0)]
    public class GunStaticData : ScriptableObject
    {
        public GunTypeId gunTypeId;

        public float damage = 10;
        public float projectileSpeed = 5;
        public float range = 10;
        public float cooldown = 1;

        [Space] public AssetReferenceGameObject projectilePrefabReference;
    }
}
