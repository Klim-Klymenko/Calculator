using System;
using System.Text.RegularExpressions;
using GameEngine.CalculatorFeature;
using UI.CalculatorWindow.View;

namespace UI.CalculatorWindow.Presentation
{
    public sealed class CalculatorWindowPresenter
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

        private void OnCalculationInput(string input)
        {
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
    }
}