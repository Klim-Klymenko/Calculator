using System;
using Common.CreationFeature;
using UnityEngine;
using Zenject;

namespace UI.CalculatorErrorPopup
{
    internal sealed class CalculatorErrorPopupInstaller : MonoInstaller
    {
        [SerializeField]
        private CalculatorErrorPopupView _prefab;

        [SerializeField] 
        private Transform _container;
        
        public override void InstallBindings()
        {
            BindFactories();
            BindShower();
        }

        private void BindFactories()
        {
            Container.Bind<Common.CreationFeature.IFactory<CalculatorErrorPopupView, ICalculatorErrorPopupPresenter>>()
                .To<ObjectFactory<CalculatorErrorPopupView, ICalculatorErrorPopupPresenter>>().AsSingle().WithArguments(_prefab, _container);

            Container.Bind<Common.CreationFeature.IFactory<CalculatorErrorPopupPresenter, Action, Action>>()
                .To<PlainFactory<CalculatorErrorPopupPresenter, Action, Action>>().AsSingle();
        }

        private void BindShower()
        {
            Container.Bind<CalculatorErrorPopupShower>().AsSingle();
        }
    }
}