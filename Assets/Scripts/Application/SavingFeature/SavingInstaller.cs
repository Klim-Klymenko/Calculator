using PlayFab;
using Zenject;

namespace Application.SavingFeature
{
    internal sealed class SavingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindClientAPI();
            BindClient();
        }
        
        private void BindClientAPI()
        {
            PlayFabClientInstanceAPI clientAPI = new(PlayFabSettings.staticSettings);
            Container.Bind<PlayFabClientInstanceAPI>().FromInstance(clientAPI).AsSingle();
        }
        
        private void BindClient()
        {
            string titleId = PlayFabSettings.TitleId;
            string secretKey = PlayFabSettings.DeveloperSecretKey;
            Container.Bind<Client>().AsSingle().WithArguments(titleId, secretKey);
        }
    }
}