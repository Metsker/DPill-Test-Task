using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        Task Instantiate();
        Task<GameObject> Instantiate(string address);
        Task<GameObject> Instantiate(string address, Vector3 at);
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        void Cleanup();
        Task<List<Task<T>>> LoadAll<T>(string label) where T : class;
    }
}
