using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.CalculatorErrorPopup
{
    public sealed class CalculatorErrorPopupView : MonoBehaviour
    {
        [SerializeField]
        private Button _closeButton;
        
        private ICalculatorErrorPopupPresenter _presenter;

        [Inject]
        public void Construct(ICalculatorErrorPopupPresenter presenter)
        {
            _presenter = presenter;
        }
        
        public void Show()
        {
            _closeButton.onClick.AddListener(Hide);
            
            gameObject.SetActive(true);
            _presenter.OnShow();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            _presenter.OnClose();
            
            _closeButton.onClick.RemoveListener(Hide);
        }
    }
}