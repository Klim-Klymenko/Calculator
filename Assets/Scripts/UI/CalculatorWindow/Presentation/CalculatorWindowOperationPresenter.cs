using System.Text;
using Application.GameCycleFeature;
using Common;
using GameEngine.CalculatorFeature;
using JetBrains.Annotations;
using UI.CalculatorWindow.View;

namespace UI.CalculatorWindow.Presentation
{
    [UsedImplicitly]
    public sealed class CalculatorWindowOperationPresenter : IInitializable, IFinishable
    {
        private const char Plus = '+';
        private const char Equal = '=';
        private const string Error = "ERROR";
        
        private readonly CalculatorWindowOperationView _view;
        private readonly Calculator _calculator;
        private readonly History _history;
        private readonly IPool<StringBuilder> _pool;

        internal CalculatorWindowOperationPresenter(CalculatorWindowOperationView view, Calculator calculator,
            History history, IPool<StringBuilder> pool)
        {
            _view = view;
            _calculator = calculator;
            _history = history;
            _pool = pool;
        }

        void IInitializable.OnInitialize()
        {
            _calculator.OnOperationSucceeded += OnOperationSucceed;
            _view.OnOperationTextSet += _history.AddOperation;
        }

        void IFinishable.OnFinish()
        {
            _calculator.OnOperationSucceeded -= OnOperationSucceed;
            _view.OnOperationTextSet -= _history.AddOperation;
        }

        private void OnOperationSucceed(OperationData operationData)
        {
            StringBuilder sb = _pool.Get();
            
            sb.AppendJoin(Plus, operationData.Operands);
            sb.Append(Equal);
            sb.Append(operationData.Result);
            
            OnViewTextSet(sb);
        }
        
        public void OnOperationFail(string input)
        {
            StringBuilder sb = _pool.Get();

            sb.Append(input);
            sb.Append(Equal);
            sb.Append(Error);
            
            OnViewTextSet(sb);
        }
        
        private void OnViewTextSet(StringBuilder sb)
        {
            _view.SetOperationText(sb.ToString());
            
            sb.Clear();
            _pool.Put(sb);
        }
    }
}