using TMPro;
using UnityEngine;

namespace UI.CalculatorWindow.View
{
    public sealed class CalculatorWindowOperationView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _operationText;
        
        public void SetOperationText(string operation)
        {
            _operationText.text = operation;
        }
    }
}