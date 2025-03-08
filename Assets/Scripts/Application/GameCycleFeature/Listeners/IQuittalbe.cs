namespace Application.GameCycleFeature
{
    public interface IQuittable : IGameListener
    {
        void OnQuit();
    }
}