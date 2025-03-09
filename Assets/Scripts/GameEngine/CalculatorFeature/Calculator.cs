using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace GameEngine.CalculatorFeature
{
    [UsedImplicitly]
    public sealed class Calculator
    {
        public event Action<OperationData> OnOperationSucceeded; 
        public event Action<OperationData> OnOperationFailed; 
        
        public OperationData CurrentOperation { get; set; }
        public List<OperationData> CompletedOperations { get; set; } = new();
        
        public void Calculate(IReadOnlyList<int> operands)
        {
            int sum = operands.Sum();
            
            OperationData completedOperation = new OperationData
            {
                Operands = operands,
                Result = sum
            };
            
            CompletedOperations.Add(completedOperation);
            OnOperationSucceeded?.Invoke(completedOperation);
        }
        
        public void FailOperation(IReadOnlyList<int> operands)
        {
            OperationData completedOperation = new OperationData
            {
                Operands = operands,
                Result = -1
            };
            
            CompletedOperations.Add(completedOperation);
            OnOperationFailed?.Invoke(completedOperation);
        }
    }
}