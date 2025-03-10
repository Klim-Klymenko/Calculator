using JetBrains.Annotations;
using Zenject;

namespace Common.CreationFeature
{
    [UsedImplicitly]
    public sealed class PlainFactory<T> : IFactory<T>
    where T : class
    {
        private readonly DiContainer _diContainer;

        public PlainFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        T IFactory<T>.Create()
        {
            return _diContainer.Instantiate<T>();
        }
    }
    
    [UsedImplicitly]
    public sealed class PlainFactory<TR, TArg> : IFactory<TR, TArg>
        where TR : class
    {
        private readonly DiContainer _diContainer;

        public PlainFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        TR IFactory<TR, TArg>.Create(TArg arg)
        {
            object[] args = { arg };
            return _diContainer.Instantiate<TR>(args);
        }
    }
    
    [UsedImplicitly]
    public sealed class PlainFactory<TR, TArg1, TArg2> : IFactory<TR, TArg1, TArg2>
        where TR : class
    {
        private readonly DiContainer _diContainer;

        public PlainFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        TR IFactory<TR, TArg1, TArg2>.Create(TArg1 arg1, TArg2 arg2)
        {
            object[] args = { arg1, arg2 };
            return _diContainer.Instantiate<TR>(args);
        }
    }
}