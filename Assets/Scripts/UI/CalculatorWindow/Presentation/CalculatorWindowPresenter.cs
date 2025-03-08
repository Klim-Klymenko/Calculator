using System;
using System.Text;
using System.Text.RegularExpressions;
using GameEngine.CalculatorFeature;
using UI.CalculatorWindow.View;
using Zenject;

namespace UI.CalculatorWindow.Presentation
{
    public sealed class CalculatorWindowPresenter : IInitializable, IDisposable
    {
        private const char Plus = '+';
        private const string Pattern = @"^\d+(?:\+\d+)*$";
        
        private readonly Regex _regex = new(Pattern, RegexOptions.Compiled);

        private readonly CalculatorWindowView _view;
        private readonly Calculator _calculator;

        public CalculatorWindowPresenter(CalculatorWindowView view, Calculator calculator)
        {
            _view = view;
            _calculator = calculator;
        }

        void IInitializable.Initialize()
        {
            _view.OnResultButtonClicked += OnCalculationInput;
            _calculator.OnOperationCompleted += OnOperationCompleted;
        }
        
        void IDisposable.Dispose()
        {
            _view.OnResultButtonClicked -= OnCalculationInput;
            _calculator.OnOperationCompleted -= OnOperationCompleted;
        }
        
        private void OnCalculationInput(string input)
        {
            //StringBuilder sb = new();
            //TODO: optimize with the StringBuilder
            if (_regex.IsMatch(input))
            {
                string trimmedInput = input.Trim();
                string[] operands = trimmedInput.Split(Plus);
                
                int[] parsedOperands = Array.ConvertAll(operands, int.Parse);
                _calculator.Calculate(parsedOperands);
                
                return;
            }
            
            //TODO: Call view's method to add an invalid operation + show the popup with popup shower
        }

        private void OnOperationCompleted(OperationData operationData)
        {
            string operation = string.Join(Plus, operationData.Operands) + "=" + operationData.Result;
            _view.CreateOperationView(operation);
        }
    }
}