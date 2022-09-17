using System;
namespace CodeBase.StaticData
{
    [Serializable]
    public struct BattleFieldData
    {
        public float minX;
        public float maxX;
        public float minZ;
        public float maxZ;

        public BattleFieldData(float minX, float maxX, float minZ, float maxZ)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minZ = minZ;
            this.maxZ = maxZ;
        }
    }
}
