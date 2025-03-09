using System;
using TMPro;
using UnityEngine;

namespace UI.CalculatorWindow.View
{
    public sealed class CalculatorWindowOperationView : MonoBehaviour
    {
        public event Action<string> OnOperationTextSet; 
        
        [SerializeField]
        private TextMeshProUGUI _operationText;
        
        public void SetOperationText(string operation)
        {
            _operationText.text = operation;
            OnOperationTextSet?.Invoke(operation);
        }
    }
}