using System;
using Application.GameCycleFeature;
using JetBrains.Annotations;

namespace Common.CreationFeature
{
    [UsedImplicitly]
    public sealed class PlainPool<T> : BasePool<T>
    where T : class
    {
        private readonly GameCycleManager _gameCycleManager;

        internal PlainPool(int poolSize,  GameCycleManager gameCycleManager) : base(poolSize)
        {
            _gameCycleManager = gameCycleManager;
        }

        private protected override T Instantiate()
        {
            return Activator.CreateInstance<T>();
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