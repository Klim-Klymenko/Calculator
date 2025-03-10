using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Application.GameCycleFeature;
using Application.SavingFeature;
using JetBrains.Annotations;
using Common;
using Common.CreationFeature;
using GameEngine.CalculatorFeature;
using UI.CalculatorErrorPopup;
using UI.CalculatorWindow.View;

namespace UI.CalculatorWindow.Presentation
{
    [UsedImplicitly]
    public sealed class CalculatorWindowPresenter : IInitializable, IFinishable
    {
        private const char Plus = '+';
        private const string WhiteSpace = " ";
        private const string Pattern = @"^\d+(?:\+\d+)*$";
        
        private readonly Regex _regex = new(Pattern, RegexOptions.Compiled);

        private CalculatorWindowOperationPresenter _presenter;
        
        private readonly CalculatorWindowView _view;
        private readonly Calculator _calculator;
        private readonly History _history;
        private readonly CalculatorErrorPopupShower _errorPopupShower;
        private readonly SaveSystem _saveSystem;
        private readonly IPool<StringBuilder> _pool;
        private readonly IFactory<CalculatorWindowOperationPresenter, CalculatorWindowOperationView> _factory;
        
        public CalculatorWindowPresenter(CalculatorWindowView view, Calculator calculator, History history, 
            CalculatorErrorPopupShower errorPopupShower, SaveSystem saveSystem, IPool<StringBuilder> pool,
            IFactory<CalculatorWindowOperationPresenter, CalculatorWindowOperationView> factory)
        {
            _view = view;
            _calculator = calculator;
            _history = history;
            _errorPopupShower = errorPopupShower;
            _saveSystem = saveSystem;
            _pool = pool;
            _factory = factory;
            _errorPopupShower = errorPopupShower;
        }

        void IInitializable.OnInitialize()
        {
            _view.OnResultButtonClicked += OnCalculationInput;
            _view.OnInputFieldChanged += OnInputFieldChanged;
            _saveSystem.OnLoaded += DisplaySavings;
        }

        void IFinishable.OnFinish()
        {
            _view.OnResultButtonClicked -= OnCalculationInput;
            _view.OnInputFieldChanged -= OnInputFieldChanged;
            _saveSystem.OnLoaded -= DisplaySavings;
        }

        private void OnCalculationInput(string input)
        {
            string trimmedInput = input.Replace(WhiteSpace, string.Empty);
            IReadOnlyList<int> operands = ParseOperands(trimmedInput);

            CalculatorWindowOperationView operationView = _view.CreateOperationView();
            
            if (_presenter == null)
                _presenter = _factory.Create(operationView);
            else
                _presenter.SetView(operationView);
            
            _presenter.OnInitialize();
            
            if (_regex.IsMatch(trimmedInput))
            {
                _view.ClearInputField();
                _calculator.Calculate(operands);
                
                _presenter.Dispose();
                return;
            }
            
            _presenter.OnOperationFail(trimmedInput);
            _errorPopupShower.Show(_view.Hide, _view.Show);
            
            _presenter.Dispose();
        }

        private void OnInputFieldChanged(string input)
        {
            _history.UncompletedOperation = input;
        }
        
        private void DisplaySavings()
        {
            string uncompletedOperation = _history.UncompletedOperation;
            _view.SetInputField(uncompletedOperation);
            
            IReadOnlyList<string> operations = _history.GetOperations();
            
            if (operations.Count == 0)
                return;
            
            CalculatorWindowOperationView operationView = _view.CreateOperationView();
            _presenter = _factory.Create(operationView);
            
            int operationsCount = operations.Count;
            for (int i = 0; i < operationsCount; i++)
            {
                _presenter.OnPastOperation(operations[i]);
                
                if (i >= operationsCount - 1)
                    break;
                
                CalculatorWindowOperationView view = _view.CreateOperationView();
                _presenter.SetView(view);
            }
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