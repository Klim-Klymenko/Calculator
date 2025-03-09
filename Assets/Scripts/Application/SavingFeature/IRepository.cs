using Cysharp.Threading.Tasks;

namespace Application.SavingFeature
{
    public interface IRepository
    {
        UniTask LoadState();
        UniTask SaveState();
        bool TryGetData<T>(out T data);
        void SetData<T>(T data);
    }
}