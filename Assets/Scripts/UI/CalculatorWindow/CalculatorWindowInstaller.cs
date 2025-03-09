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
        private int _sbPoolSize = 5;
        
        [SerializeField]
        private CalculatorWindowView _view;
        
        
        public override void InstallBindings()
        {
            Container.Bind<IPool<StringBuilder>>().To<PlainPool<StringBuilder>>().AsSingle().WithArguments(_sbPoolSize);
            Container.BindInterfacesTo<CalculatorWindowPresenter>().AsCached().WithArguments(_view);
        }
    }
}