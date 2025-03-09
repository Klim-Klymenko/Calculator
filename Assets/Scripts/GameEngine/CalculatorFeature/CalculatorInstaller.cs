using Zenject;

namespace GameEngine.CalculatorFeature
{
    internal sealed class CalculatorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Calculator>().AsSingle();
            Container.Bind<History>().AsSingle();
        }
    }
}