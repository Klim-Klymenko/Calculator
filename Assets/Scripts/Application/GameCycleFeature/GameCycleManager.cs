using System.Collections.Generic;
using UnityEngine;

namespace Application.GameCycleFeature
{
    public sealed class GameCycleManager : MonoBehaviour
    {
        private GameState _gameState;

        private readonly List<IInitializable> _initializableListeners = new();
        private readonly List<IStartable> _startableListeners = new();
        private readonly List<IUpdatable> _updatableListeners = new();
        private readonly List<IFinishable> _finishableListeners = new();
        private readonly List<IQuittable> _quittableListeners = new();

        private void Awake()
        {
            if (_gameState != GameState.None) 
                return;

            for (int i = 0; i < _initializableListeners.Count; i++)
            {
                _initializableListeners[i].OnInitialize();
            }
            
            _gameState = GameState.Initialized;
        }

        private void Start()
        {
            if (_gameState != GameState.Initialized) 
                return;

            for (int i = 0; i < _startableListeners.Count; i++)
            {
                _startableListeners[i].OnStart();
            }
            
            _gameState = GameState.Active;
        }
        
        private void Update()
        {
            if (_gameState != GameState.Active) 
                return;

            for (int i = 0; i < _updatableListeners.Count; i++)
            {
                _updatableListeners[i].OnUpdate();
            }
        }
        
        private void OnDestroy()
        {
            if (_gameState == GameState.Finished)
                return;

            for (int i = 0; i < _finishableListeners.Count; i++)
            {
                _finishableListeners[i].OnFinish();
            }
            
            _gameState = GameState.Finished;
        }

        private void OnApplicationQuit()
        {
            if (_gameState == GameState.None)
                return;

            for (int i = 0; i < _quittableListeners.Count; i++)
            {
                _quittableListeners[i].OnQuit();
            }
        }
        
        public void AddListener(IGameListener listener)
        {
            if (listener is IInitializable initializable)
            {
                if (!_initializableListeners.Contains(initializable))
                {
                    _initializableListeners.Add(initializable);

                    if (_gameState is GameState.Initialized or GameState.Active)
                        initializable.OnInitialize();
                }
            }
            
            if (listener is IStartable startable)
            {
                if (!_startableListeners.Contains(startable))
                {
                    _startableListeners.Add(startable);
                    
                    if (_gameState == GameState.Active)
                        startable.OnStart();
                }
            }
        
            if (listener is IUpdatable updatable)
            {
                if (!_updatableListeners.Contains(updatable))
                    _updatableListeners.Add(updatable);
            }
            
            if (listener is IFinishable finishable)
            {
                if (!_finishableListeners.Contains(finishable))
                    _finishableListeners.Add(finishable);
            }

            if (listener is IQuittable quittable)
            {
                if (!_quittableListeners.Contains(quittable))
                    _quittableListeners.Add(quittable);
            }
        }
        
        public void RemoveListener(IGameListener listener)
        {
            if (listener is IInitializable initializable)
            {
                if (_initializableListeners.Contains(initializable))
                    _initializableListeners.Remove(initializable);
            }

            if (listener is IStartable startable)
            {
                if (_startableListeners.Contains(startable))
                    _startableListeners.Remove(startable);
            }
            
            if (listener is IUpdatable updatable)
            {
                if (_updatableListeners.Contains(updatable))
                    _updatableListeners.Remove(updatable);
            }

            if (listener is IFinishable finishable)
            {
                if (_finishableListeners.Contains(finishable))
                    _finishableListeners.Remove(finishable);
                
                if (_gameState != GameState.None && _gameState != GameState.Finished)
                    finishable.OnFinish();
            }
            
            if (listener is IQuittable quittable)
            {
                if (_quittableListeners.Contains(quittable))
                    _quittableListeners.Remove(quittable);
            }
        }
    }
}