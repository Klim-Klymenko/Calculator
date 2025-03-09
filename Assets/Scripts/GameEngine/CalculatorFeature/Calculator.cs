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
        
        public void Calculate(IReadOnlyList<int> operands)
        {
            int sum = operands.Sum();
            
            OperationData completedOperation = new OperationData
            {
                Operands = operands,
                Result = sum
            };
            
            OnOperationSucceeded?.Invoke(completedOperation);
        }
    }
}