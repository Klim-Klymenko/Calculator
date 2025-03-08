namespace Application.GameCycleFeature
{
    public interface IFinishable : IGameListener
    {
        void OnFinish();
    }
}