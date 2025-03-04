﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.CalculatorFeature
{
    public sealed class Calculator
    {
        public event Action<OperationData> OnOperationCompleted; 
        
        public OperationData CurrentOperation { get; set; }
        public List<OperationData> CompletedOperations { get; set; }
        
        public void Calculate(IReadOnlyList<int> operands)
        {
            int sum = operands.Sum();
            
            OperationData completedOperation = new OperationData
            {
                Operands = operands,
                Result = sum
            };
            
            CompletedOperations.Add(completedOperation);
            OnOperationCompleted?.Invoke(completedOperation);
        }
    }
}