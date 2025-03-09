using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CalculatorWindow.View
{
    public sealed class CalculatorWindowView : MonoBehaviour
    {
        public event Action<string> OnResultButtonClicked;
        
        [SerializeField] 
        private TMP_InputField _inputField;

        [SerializeField] 
        private Button _resultButton;

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

        public void CreateOperationView(string operation)
        {
            
        }
        
        public void ClearInputField()
        {
            _inputField.text = string.Empty;
        }
    }
}