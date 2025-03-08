using UnityEngine;
using Zenject;

namespace Application.GameCycleFeature
{
    internal sealed class GameCycleInstaller : MonoInstaller
    {
        [SerializeField]
        private GameCycleManager _gameCycleManager;

        [SerializeField]
        private SceneContext _sceneContext;
        
        private GameCycleManagerInstaller _gameCycleManagerInstaller;
        
        public override void InstallBindings()
        { 
            BindGameCycleManager();
            BindGameCycleManagerInstaller();
            
            _sceneContext.PostResolve += InstallListeners;
        }

        private void BindGameCycleManager()
        {
            Container.Bind<GameCycleManager>().FromInstance(_gameCycleManager).AsSingle();
        }

        private void BindGameCycleManagerInstaller()
        {
            Container.Bind<GameCycleManagerInstaller>().AsSingle()
                .OnInstantiated<GameCycleManagerInstaller>((_, it) => _gameCycleManagerInstaller = it).NonLazy();
        }

        private void InstallListeners()
        {
            _gameCycleManagerInstaller.InstallListeners();
            _sceneContext.PostResolve -= InstallListeners;
        }
    }
}