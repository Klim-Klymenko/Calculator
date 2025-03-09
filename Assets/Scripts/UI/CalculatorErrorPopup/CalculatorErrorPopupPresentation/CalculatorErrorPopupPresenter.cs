using System;
using JetBrains.Annotations;

namespace UI.CalculatorErrorPopup
{
    [UsedImplicitly]
    public sealed class CalculatorErrorPopupPresenter : ICalculatorErrorPopupPresenter
    {
        public Action OnShow { get; }
        public Action OnClose { get; }

        public CalculatorErrorPopupPresenter(Action onShow, Action onClose)
        {
            OnShow = onShow;
            OnClose = onClose;
        }
    }
}