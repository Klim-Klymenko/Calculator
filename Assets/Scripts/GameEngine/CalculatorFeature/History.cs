using System.Collections.Generic;

namespace GameEngine.CalculatorFeature
{
    public sealed class History
    {
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