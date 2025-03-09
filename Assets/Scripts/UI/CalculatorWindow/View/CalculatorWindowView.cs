using System;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.CalculatorWindow.View
{
    public sealed class CalculatorWindowView : MonoBehaviour
    {
        public event Action<string> OnResultButtonClicked;
        
        [SerializeField] 
        private TMP_InputField _inputField;

        [SerializeField] 
        private Button _resultButton;

        [SerializeField]
        private Transform _parentTransform;
        
        private IPool<CalculatorWindowOperationView> _pool;
        
        [Inject]
        public void Construct(IPool<CalculatorWindowOperationView> pool)
        {
            _pool = pool;
        }
        
        private void OnEnable()
        {
            _resultButton.onClick.AddListener(OnResultButtonClick);
        }

        private void OnDisable()
        {
            _resultButton.onClick.RemoveListener(OnResultButtonClick);
        }

        private void OnResultButtonClick()
        {
            OnResultButtonClicked?.Invoke(_inputField.text);
        }

        public void ClearInputField()
        {
            _inputField.text = string.Empty;
        }
        
        public CalculatorWindowOperationView CreateOperationView()
        {
            CalculatorWindowOperationView view = _pool.Get();
            view.transform.SetParent(_parentTransform);
            
            return view;
        }
    }
}