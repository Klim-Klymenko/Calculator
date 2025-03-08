using JetBrains.Annotations;
using UnityEngine;

namespace Application.GameCycleFeature
{
    [UsedImplicitly]
    internal sealed class GameCycleManagerInstaller
    {
        private readonly GameCycleManager _gameCycleManager;
        private readonly IGameListener[] _gameListeners;
        
        internal GameCycleManagerInstaller(GameCycleManager gameCycleManager, IGameListener[] gameListeners)
        {
            _gameCycleManager = gameCycleManager;
            _gameListeners = gameListeners;
        }

        internal void InstallListeners()
        {
            InstallSystemListeners();
            InstallSceneListeners();
        }
        
        private void InstallSystemListeners()
        {
            for (int i = 0; i < _gameListeners.Length; i++)
            {
                _gameCycleManager.AddListener(_gameListeners[i]);
            }
        }
        
        private void InstallSceneListeners()
        {
            MonoBehaviour[] sceneComponents = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            for (int i = 0; i < sceneComponents.Length; i++)
            {
                if (sceneComponents[i] is IGameListener gameListener)
                    _gameCycleManager.AddListener(gameListener);
            }
        }
    }
}