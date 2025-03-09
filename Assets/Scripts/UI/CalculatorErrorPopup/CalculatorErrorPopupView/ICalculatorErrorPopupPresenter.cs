using System;

namespace UI.CalculatorErrorPopup
{
    public interface ICalculatorErrorPopupPresenter
    {
        Action OnShow { get; }
        Action OnClose { get; }
    }
}