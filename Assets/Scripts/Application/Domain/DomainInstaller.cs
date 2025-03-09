using Application.SavingFeature;
using Zenject;

namespace Application.Domain
{
    internal sealed class DomainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISaveLoader>().To<CalculatorSaveLoader>().AsSingle();
        }
    }
}