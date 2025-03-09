using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Common.CreationFeature
{
    [UsedImplicitly]
    public sealed class ObjectFactory<T> : IFactory<T>
    where T : Object
    {
        private readonly DiContainer _diContainer;
        private readonly T _prefab;
        private readonly Transform _parent;

        public ObjectFactory(DiContainer diContainer, T prefab, Transform parent = null)
        {
            _diContainer = diContainer;
            _prefab = prefab;
            _parent = parent;
        }

        T IFactory<T>.Create()
        {
            return _diContainer.InstantiatePrefabForComponent<T>(_prefab, _parent);
        }
    }
    
    [UsedImplicitly]
    public sealed class ObjectFactory<TR, TArg> : IFactory<TR, TArg>
        where TR : Object
    {
        private readonly DiContainer _diContainer;
        private readonly TR _prefab;
        private readonly Transform _parent;

        public ObjectFactory(DiContainer diContainer, TR prefab, Transform parent = null)
        {
            _diContainer = diContainer;
            _prefab = prefab;
            _parent = parent;
        }
        
        TR IFactory<TR, TArg>.Create(TArg arg)
        {
            object[] args = { arg };
            return _diContainer.InstantiatePrefabForComponent<TR>(_prefab, _parent, args);
        }
    }
    
    [UsedImplicitly]
    public sealed class ObjectFactory<TR, TArg1, TArg2> : IFactory<TR, TArg1, TArg2>
        where TR : Object
    {
        private readonly DiContainer _diContainer;
        private readonly TR _prefab;
        private readonly Transform _parent;

        public ObjectFactory(DiContainer diContainer, TR prefab, Transform parent = null)
        {
            _diContainer = diContainer;
            _prefab = prefab;
            _parent = parent;
        }

        TR IFactory<TR, TArg1, TArg2>.Create(TArg1 arg1, TArg2 arg2)
        {
            object[] args = { arg1, arg2 };
            return _diContainer.InstantiatePrefabForComponent<TR>(_prefab, _parent, args);
        }
    }
}