using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Application.SavingFeature
{
    [UsedImplicitly]
    public sealed class SaveSystem
    {
        private readonly IRepository _repository;
        private readonly ISaveLoader _saveLoader;

        internal SaveSystem(IRepository repository, ISaveLoader saveLoader)
        {
            _repository = repository;
            _saveLoader = saveLoader;
        }

        public async UniTask Save()
        {
            _saveLoader.Save();
            await _repository.SaveState();
        }

        public async UniTask Load()
        {
            await _repository.LoadState();
            _saveLoader.Load();
        }
    }
}