namespace GameEngine.CalculatorFeature
{
    internal sealed class Calculator : ICalculator
    {
        private OperationData _currentOperation;
        private OperationData[] _completedOperations;
    }
}