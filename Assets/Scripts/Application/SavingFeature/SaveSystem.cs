using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Application.SavingFeature
{
    [UsedImplicitly]
    public sealed class SaveSystem
    {
        public event Action OnSaved;
        public event Action OnLoaded;
        
        private readonly IRepository _repository;
        private readonly ISaveLoader _saveLoader;

        internal SaveSystem(IRepository repository, ISaveLoader saveLoader)
        {
            _repository = repository;
            _saveLoader = saveLoader;
        }

        internal async UniTask Save()
        {
            _saveLoader.Save();
            await _repository.SaveState();
            
            OnSaved?.Invoke();
        }

        internal async UniTask Load()
        {
            await _repository.LoadState();
            _saveLoader.Load();
            
            OnLoaded?.Invoke();
        }
    }
}