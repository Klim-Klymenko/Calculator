﻿using System;
using System.Text;
using Application.GameCycleFeature;
using Common;
using GameEngine.CalculatorFeature;
using JetBrains.Annotations;
using UI.CalculatorWindow.View;

namespace UI.CalculatorWindow.Presentation
{
    [UsedImplicitly]
    public sealed class CalculatorWindowOperationPresenter : IInitializable, IDisposable
    {
        private const char Plus = '+';
        private const char Equal = '=';
        private const string Error = "ERROR";
        
        private CalculatorWindowOperationView _view;
        
        private readonly Calculator _calculator;
        private readonly History _history;
        private readonly IPool<StringBuilder> _pool;

        internal CalculatorWindowOperationPresenter(CalculatorWindowOperationView view, 
            Calculator calculator, History history, IPool<StringBuilder> pool)
        {
            _view = view;
            _calculator = calculator;
            _history = history;
            _pool = pool;
        }

        public void SetView(CalculatorWindowOperationView view)
        {
            _view = view;
        }

        public void OnInitialize()
        {
            _calculator.OnOperationSucceeded += OnOperationSucceed;
            _view.OnOperationTextSet += AddOperation;
        }

        public void Dispose()
        {
            _calculator.OnOperationSucceeded -= OnOperationSucceed;
            _view.OnOperationTextSet -= AddOperation;
        }

        private void OnOperationSucceed(OperationData operationData)
        {
            StringBuilder sb = _pool.Get();
            
            sb.AppendJoin(Plus, operationData.Operands);
            sb.Append(Equal);
            sb.Append(operationData.Result);
            
            OnViewTextSet(sb);
        }
        
        private void AddOperation(string operation)
        {
            _history.AddOperation(operation);
        }
        
        public void OnOperationFail(string input)
        {
            StringBuilder sb = _pool.Get();

            sb.Append(input);
            sb.Append(Equal);
            sb.Append(Error);
            
            OnViewTextSet(sb);
        }

        public void OnPastOperation(string operation)
        {
            _view.SetOperationText(operation);
        }
        
        private void OnViewTextSet(StringBuilder sb)
        {
            _view.SetOperationText(sb.ToString());
            
            sb.Clear();
            _pool.Put(sb);
        }
    }
}