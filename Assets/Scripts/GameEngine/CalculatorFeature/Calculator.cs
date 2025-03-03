namespace GameEngine.CalculatorFeature
{
    public sealed class Calculator
    {
        public OperationData CurrentOperation { get; set; }
        public OperationData[] CompletedOperations { get; set; }
    }
}