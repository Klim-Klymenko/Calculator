using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameEngine.CalculatorFeature
{
    [UsedImplicitly]
    public sealed class History
    {
        public string UncompletedOperation { get; set; }
        private readonly List<string> _completedOperations = new();
        
        public void AddOperation(string operation)
        {
            _completedOperations.Add(operation);
        }
        
        public IReadOnlyList<string> GetOperations()
        {
            return _completedOperations;
        }
    }
}