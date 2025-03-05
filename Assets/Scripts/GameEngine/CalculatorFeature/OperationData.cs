using System.Collections.Generic;

namespace GameEngine.CalculatorFeature
{
    public struct OperationData
    {
        public IReadOnlyList<int> Operands { get; init; }
        public int Result { get; init; }
    }
}