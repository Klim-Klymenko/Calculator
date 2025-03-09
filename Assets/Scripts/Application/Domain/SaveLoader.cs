using Application.SavingFeature;

namespace Application.Domain
{
    public abstract class SaveLoader<TData> : ISaveLoader
    {
        private readonly IRepository _repository;

        private protected SaveLoader(IRepository repository)
        {
            _repository = repository;
        }

        void ISaveLoader.Save()
        {
            TData data = ConvertData();
            _repository.SetData(data);
        }

        void ISaveLoader.Load()
        {
            if (_repository.TryGetData(out TData data))
                ApplyData(data);
        }
        
        private protected abstract TData ConvertData();
        private protected abstract void ApplyData(TData data);
    }
}