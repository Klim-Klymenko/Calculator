using System.Collections.Generic;
using Application.SavingFeature;
using GameEngine.CalculatorFeature;
using JetBrains.Annotations;

namespace Application.Domain
{
    [UsedImplicitly]
    internal sealed class CalculatorSaveLoader : SaveLoader<CalculatorData>
    {
        private readonly History _history;
        
        internal CalculatorSaveLoader(History history, IRepository repository) : base(repository)
        {
            _history = history;
        }

        private protected override CalculatorData ConvertData()
        {
            return new CalculatorData
            {
                UncompletedOperation = _history.UncompletedOperation,
                CompletedOperations = _history.GetOperations()
            };
        }

        private protected override void ApplyData(CalculatorData data)
        {
            IReadOnlyList<string> completedOperations = data.CompletedOperations;
            
            for (int i = 0; i < completedOperations.Count; i++)
            {
                _history.AddOperation(completedOperations[i]);
            }
            
            _history.UncompletedOperation = data.UncompletedOperation;
        }
    }
}