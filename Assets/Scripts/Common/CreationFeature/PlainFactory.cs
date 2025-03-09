using Application.GameCycleFeature;
using JetBrains.Annotations;
using Zenject;

namespace Common.CreationFeature
{
    [UsedImplicitly]
    public sealed class PlainFactory<T> : IFactory<T>
    where T : class
    {
        private readonly DiContainer _diContainer;
        private readonly GameCycleManager _gameCycleManager;

        public PlainFactory(DiContainer diContainer, GameCycleManager gameCycleManager)
        {
            _diContainer = diContainer;
            _gameCycleManager = gameCycleManager;
        }

        T IFactory<T>.Create()
        {
            T instance = _diContainer.Instantiate<T>();
            
            if (instance is IGameListener listener)
                _gameCycleManager.AddListener(listener);

            return instance;
        }
    }
    
    [UsedImplicitly]
    public sealed class PlainFactory<TR, TArg> : IFactory<TR, TArg>
        where TR : class
    {
        private readonly DiContainer _diContainer;
        private readonly GameCycleManager _gameCycleManager;

        public PlainFactory(DiContainer diContainer, GameCycleManager gameCycleManager)
        {
            _diContainer = diContainer;
            _gameCycleManager = gameCycleManager;
        }
        
        TR IFactory<TR, TArg>.Create(TArg arg)
        {
            object[] args = { arg };
            TR instance = _diContainer.Instantiate<TR>(args);
            
            if (instance is IGameListener listener)
                _gameCycleManager.AddListener(listener);

            return instance;
        }
    }
    
    [UsedImplicitly]
    public sealed class PlainFactory<TR, TArg1, TArg2> : IFactory<TR, TArg1, TArg2>
        where TR : class
    {
        private readonly DiContainer _diContainer;
        private readonly GameCycleManager _gameCycleManager;

        public PlainFactory(DiContainer diContainer, GameCycleManager gameCycleManager)
        {
            _diContainer = diContainer;
            _gameCycleManager = gameCycleManager;
        }

        TR IFactory<TR, TArg1, TArg2>.Create(TArg1 arg1, TArg2 arg2)
        {
            object[] args = { arg1, arg2 };
            TR instance = _diContainer.Instantiate<TR>(args);
            
            if (instance is IGameListener listener)
                _gameCycleManager.AddListener(listener);

            return instance;
        }
    }
}