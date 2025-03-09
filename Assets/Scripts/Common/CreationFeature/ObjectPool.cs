using JetBrains.Annotations;
using UnityEngine;

namespace Common.CreationFeature
{
    [UsedImplicitly]
    public sealed class ObjectPool<T> : BasePool<T>
        where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        
        public ObjectPool(int poolSize, T prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;

            Reserve(poolSize);
        }

        private protected override T Instantiate()
        {
            return Object.Instantiate(_prefab, _parent);
        }

        private protected override void SetActive(T obj, bool active)
        {
            obj.gameObject.SetActive(active);
        }

        private protected override void OnObjectPut(T obj)
        {
            if (_parent != null)
                obj.transform.SetParent(_parent);
        }
    }
}
