using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new ();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new ();

        public async Task Instantiate()
        {
            await Addressables.InitializeAsync().Task;
            //AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "materials"));
        }

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCashOnComplete(
                Addressables.LoadAssetAsync<T>(assetReference),
                assetReference.AssetGUID);
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCashOnComplete(
                Addressables.LoadAssetAsync<T>(address),
                address);
        }

        public async Task<List<Task<T>>> LoadAll<T>(string label) where T : class
        {
            if (_completedCache.TryGetValue(label, out AsyncOperationHandle completedHandle))
                return (List<Task<T>>)completedHandle.Result;

            IList<IResourceLocation> handle = await Addressables.LoadResourceLocationsAsync(label, typeof(T)).Task;

            List<Task<T>> tasks = new ();

            foreach (IResourceLocation location in handle)
            {
                T task = await RunWithCashOnComplete(Addressables.LoadAssetAsync<T>(location), label);
                tasks.Add(Task.FromResult(task));
            }
            
            return tasks;
        }

        public Task<GameObject> Instantiate(string address, Vector3 at)
        {
            return Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;
        }

        public Task<GameObject> Instantiate(string address)
        {
            return Addressables.InstantiateAsync(address).Task;
        }

        public void Cleanup()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
                foreach (AsyncOperationHandle handle in resourceHandles)
                    Addressables.Release(handle);

            _completedCache.Clear();
            _handles.Clear();
        }

        private async Task<T> RunWithCashOnComplete<T>(AsyncOperationHandle<T> handle, string cashKey) where T : class
        {
            handle.Completed += operationHandle =>
                _completedCache[cashKey] = operationHandle;

            AddHandle(cashKey, handle);

            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandle))
            {
                resourceHandle = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandle;
            }

            resourceHandle.Add(handle);
        }
    }
}
