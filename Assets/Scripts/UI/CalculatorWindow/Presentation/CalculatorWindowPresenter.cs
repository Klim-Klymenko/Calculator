using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Application.GameCycleFeature;
using JetBrains.Annotations;
using Common;
using Common.CreationFeature;
using GameEngine.CalculatorFeature;
using UI.CalculatorErrorPopup;
using UI.CalculatorWindow.View;

namespace UI.CalculatorWindow.Presentation
{
    [UsedImplicitly]
    public sealed class CalculatorWindowPresenter : IInitializable, IStartable, IFinishable
    {
        private const char Plus = '+';
        private const string WhiteSpace = " ";
        private const string Pattern = @"^\d+(?:\+\d+)*$";
        
        private readonly Regex _regex = new(Pattern, RegexOptions.Compiled);

        private readonly CalculatorWindowView _view;
        private readonly Calculator _calculator;
        private readonly History _history;
        private readonly CalculatorErrorPopupShower _errorPopupShower;
        private readonly IPool<StringBuilder> _pool;
        private readonly IFactory<CalculatorWindowOperationPresenter, CalculatorWindowOperationView> _factory;
        
        public CalculatorWindowPresenter(CalculatorWindowView view, Calculator calculator, History history, 
            CalculatorErrorPopupShower errorPopupShower, IPool<StringBuilder> pool,
            IFactory<CalculatorWindowOperationPresenter, CalculatorWindowOperationView> factory)
        {
            _view = view;
            _calculator = calculator;
            _history = history;
            _errorPopupShower = errorPopupShower;
            _pool = pool;
            _factory = factory;
            _errorPopupShower = errorPopupShower;
        }

        void IInitializable.OnInitialize()
        {
            _view.OnResultButtonClicked += OnCalculationInput;
        }

        void IStartable.OnStart()
        {
            string uncompletedOperation = _history.UncompletedOperation;
            
            if (!string.IsNullOrEmpty(uncompletedOperation))
                _view.SetInputField(uncompletedOperation);
        }

        void IFinishable.OnFinish()
        {
            _view.OnResultButtonClicked -= OnCalculationInput;
        }

        private void OnCalculationInput(string input)
        {
            string trimmedInput = input.Replace(WhiteSpace, string.Empty);
            IReadOnlyList<int> operands = ParseOperands(trimmedInput);
            
            CalculatorWindowOperationView operationView = _view.CreateOperationView();
            CalculatorWindowOperationPresenter presenter = _factory.Create(operationView);
            
            if (_regex.IsMatch(trimmedInput))
            {
                _view.ClearInputField();
                _calculator.Calculate(operands);

                return;
            }
            
            presenter.OnOperationFail(trimmedInput);
            _errorPopupShower.Show(_view.Hide, _view.Show);
        }

        private IReadOnlyList<int> ParseOperands(string input)
        {
            StringBuilder sb = _pool.Get();
            List<int> operands = new();

            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];

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
            return operands;
        }

        private void ProcessOperand(StringBuilder sb, List<int> operands)
        {
            string operandString = sb.ToString();
            sb.Clear();
            
            if (int.TryParse(operandString, out int operand))
                operands.Add(operand);
        }
    }
}