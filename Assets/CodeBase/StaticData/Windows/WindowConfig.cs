using System;
using CodeBase.UI.Services.Windows;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData.Windows
{
    [Serializable]
    public class WindowConfig
    {
        public WindowId windowId;

        public AssetReferenceGameObject windowPrefabReference;
    }
}
