using Application.GameCycleFeature;
using JetBrains.Annotations;

namespace Application.SavingFeature
{
    [UsedImplicitly]
    internal sealed class SaveSystemController : IInitializable, IQuittable
    {
        private readonly SaveSystem _saveSystem;

        internal SaveSystemController(SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }

        async void IInitializable.OnInitialize()
        {
            await _saveSystem.Load();
        }

        async void IQuittable.OnQuit()
        {
            await _saveSystem.Save();
        }
    }
}