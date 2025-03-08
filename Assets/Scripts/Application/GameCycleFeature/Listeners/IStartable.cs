namespace Application.GameCycleFeature
{
    public interface IStartable : IGameListener
    {
        void OnStart();
    }
}