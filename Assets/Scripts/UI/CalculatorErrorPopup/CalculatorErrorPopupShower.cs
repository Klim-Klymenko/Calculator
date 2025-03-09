using System;
using Common.CreationFeature;
using JetBrains.Annotations;

namespace UI.CalculatorErrorPopup
{
    [UsedImplicitly]
    public sealed class CalculatorErrorPopupShower
    {
        private CalculatorErrorPopupView _view;
        
        private readonly IFactory<CalculatorErrorPopupView, ICalculatorErrorPopupPresenter> _viewFactory;
        private readonly IFactory<CalculatorErrorPopupPresenter, Action, Action> _presenterFactory;

        public CalculatorErrorPopupShower(IFactory<CalculatorErrorPopupView, ICalculatorErrorPopupPresenter> viewFactory,
            IFactory<CalculatorErrorPopupPresenter, Action, Action> presenterFactory)
        {
            _viewFactory = viewFactory;
            _presenterFactory = presenterFactory;
        }

        public void Show(Action onShow, Action onHide)
        {
            if (_view == null)
            {
                ICalculatorErrorPopupPresenter presenter = _presenterFactory.Create(onShow, onHide);
                _view = _viewFactory.Create(presenter);
            }
            
            _view.Show();
        }
    }
}