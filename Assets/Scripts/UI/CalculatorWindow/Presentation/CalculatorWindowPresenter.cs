using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Application.GameCycleFeature;
using JetBrains.Annotations;
using Common;
using GameEngine.CalculatorFeature;
using UI.CalculatorWindow.View;

namespace UI.CalculatorWindow.Presentation
{
    [UsedImplicitly]
    public sealed class CalculatorWindowPresenter : IInitializable, IFinishable
    {
        private const char Plus = '+';
        private const string Pattern = @"^\d+(?:\+\d+)*$";
        
        private readonly Regex _regex = new(Pattern, RegexOptions.Compiled);

        private readonly CalculatorWindowView _view;
        private readonly Calculator _calculator;
        private readonly IPool<StringBuilder> _pool;

        public CalculatorWindowPresenter(CalculatorWindowView view, Calculator calculator, IPool<StringBuilder> pool)
        {
            _view = view;
            _calculator = calculator;
            _pool = pool;
        }

        void IInitializable.OnInitialize()
        {
            _view.OnResultButtonClicked += OnCalculationInput;
            _calculator.OnOperationCompleted += OnOperationCompleted;
        }

        void IFinishable.OnFinish()
        {
            _view.OnResultButtonClicked -= OnCalculationInput;
            _calculator.OnOperationCompleted -= OnOperationCompleted;
        }

        private void OnCalculationInput(string input)
        {
            string trimmedInput = input.Replace(" ", string.Empty);

            if (_regex.IsMatch(trimmedInput))
            {
                StringBuilder sb = _pool.Get();
                List<int> operands = new();

                for (int i = 0; i < trimmedInput.Length; i++)
                {
                    char currentChar = trimmedInput[i];

                    if (currentChar == Plus)
                    {
                        ProcessOperand(sb, operands);
                        continue;
                    }

                    sb.Append(currentChar);
                }

                if (sb.Length > 0)
                    ProcessOperand(sb, operands);

                _pool.Put(sb);
                _calculator.Calculate(operands);
                
                _view.ClearInputField();
                return;
            }
            
            //TODO: Call view's method to add an invalid operation + show the popup with popup shower
        }

        private void ProcessOperand(StringBuilder sb, List<int> operands)
        {
            int operand = int.Parse(sb.ToString());
            operands.Add(operand);
                        
            sb.Clear();
        }
        
        private void OnOperationCompleted(OperationData operationData)
        {
            StringBuilder sb = _pool.Get();
            
            sb.AppendJoin(Plus, operationData.Operands);
            sb.Append('=');
            sb.Append(operationData.Result);
            
            _view.CreateOperationView(sb.ToString());
            
            sb.Clear();
            _pool.Put(sb);
        }
    }
}