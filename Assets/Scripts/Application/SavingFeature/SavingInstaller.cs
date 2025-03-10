using Zenject;

namespace Application.SavingFeature
{
    internal sealed class SavingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindEncryptor();
            BindRepository();
            BindSaveSystem();
            BindSaveSystemController();
        }
        
        private void BindEncryptor()
        {
            Container.Bind<AesEncryptor>().AsSingle();
        }

        private void BindRepository()
        {
            Container.Bind<IRepository>().To<Repository>().AsSingle();
        }
        
        private void BindSaveSystem()
        {
            Container.Bind<SaveSystem>().AsSingle();
        }
        
        private void BindSaveSystemController()
        {
            Container.BindInterfacesTo<SaveSystemController>().AsCached();
        }
    }
}