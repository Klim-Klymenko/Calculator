using System.Text;
using Common;
using Common.CreationFeature;
using UI.CalculatorWindow.Presentation;
using UI.CalculatorWindow.View;
using UnityEngine;
using Zenject;

namespace UI.CalculatorWindow
{
    internal sealed class CalculatorWindowInstaller : MonoInstaller
    {
        [SerializeField]
        private SceneContext _sceneContext;
        
        [Header("String Builder")]
        
        [SerializeField]
        private int _sbPoolSize = 5;
        
        [Header("Calculator Window View")]
        
        [SerializeField]
        private CalculatorWindowView _viewPrefab;
        
        [SerializeField]
        private Transform _viewParentTransform;
        
        [Header("Calculator Operation View")]
        
        [SerializeField]
        private int _viewPoolSize = 10;
        
        [SerializeField]
        private CalculatorWindowOperationView _operationViewPrefab;
        
        [SerializeField]
        private Transform _operationViewContainer;
        
        public override void InstallBindings()
        {
            BindPools();
            
            CalculatorWindowView view = Container.InstantiatePrefabForComponent<CalculatorWindowView>(_viewPrefab, _viewParentTransform);
            BindPresenter(view);
        }

        private void BindPools()
        {
            Container.Bind<IPool<StringBuilder>>().To<PlainPool<StringBuilder>>().AsSingle().WithArguments(_sbPoolSize);

            Container.Bind<IPool<CalculatorWindowOperationView>>().To<ObjectPool<CalculatorWindowOperationView>>()
                .AsSingle().WithArguments(_viewPoolSize, _operationViewPrefab, _operationViewContainer);
        }

        private void BindPresenter(CalculatorWindowView view)
        {
            Container.BindInterfacesTo<CalculatorWindowPresenter>().AsCached().WithArguments(view);
        }
    }
}