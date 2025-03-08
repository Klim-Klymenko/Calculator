using System;
using Application.GameCycleFeature;
using JetBrains.Annotations;

namespace Common.CreationFeature
{
    [UsedImplicitly]
    public sealed class PlainPool<T> : BasePool<T>
    where T : class
    {
        private readonly Type _objectType;
        private readonly GameCycleManager _gameCycleManager;

        internal PlainPool(int poolSize, Type objectType, GameCycleManager gameCycleManager) : base(poolSize)
        {
            _objectType = objectType;
            _gameCycleManager = gameCycleManager;
        }

        private protected override T Instantiate()
        {
            return (T) Activator.CreateInstance(_objectType);
        }

        private protected override void SetActive(T obj, bool active)
        {
            if (active)
            {
                if (obj is IGameListener addableListener)
                    _gameCycleManager.AddListener(addableListener);
                
                return;
            }
            
            if (obj is IGameListener removableListener)
                _gameCycleManager.RemoveListener(removableListener);
        }
    }
}