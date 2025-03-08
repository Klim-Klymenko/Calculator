namespace Application.GameCycleFeature
{
    public interface IInitializable : IGameListener
    {
        void OnInitialize();
    }
}