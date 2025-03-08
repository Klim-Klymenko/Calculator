namespace Application.GameCycleFeature
{
    public interface IUpdatable : IGameListener
    {
        void OnUpdate();
    }
}