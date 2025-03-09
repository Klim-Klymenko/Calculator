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
        private readonly IPool<StringBuilder> _pool;

        internal CalculatorWindowOperationPresenter(CalculatorWindowOperationView view, Calculator calculator, IPool<StringBuilder> pool)
        {
            _view = view;
            _calculator = calculator;
            _pool = pool;
        }

        void IInitializable.OnInitialize()
        {
            _calculator.OnOperationSucceeded += OnOperationSucceed;
            _calculator.OnOperationFailed += OnOperationFail;
        }

        void IFinishable.OnFinish()
        {
            _calculator.OnOperationSucceeded -= OnOperationSucceed;
            _calculator.OnOperationFailed -= OnOperationFail;
        }

        private void OnOperationSucceed(OperationData operationData)
        {
            StringBuilder sb = _pool.Get();
            
            AppendOperands(sb, in operationData);
            sb.Append(operationData.Result);
            OnViewTextSet(sb);
        }
        
        private void OnOperationFail(OperationData operationData)
        {
            StringBuilder sb = _pool.Get();
            
            AppendOperands(sb, in operationData);
            sb.Append(Error);
            OnViewTextSet(sb);
        }
        
        private void AppendOperands(StringBuilder sb, in OperationData operationData)
        {
            sb.AppendJoin(Plus, operationData.Operands);
            sb.Append(Equal);
        }
        
        private void OnViewTextSet(StringBuilder sb)
        {
            _view.SetOperationText(sb.ToString());
            
            sb.Clear();
            _pool.Put(sb);
        }
    }
}