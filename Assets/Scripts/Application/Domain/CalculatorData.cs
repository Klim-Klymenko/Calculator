using System.Collections.Generic;

namespace Application.Domain
{
    internal struct CalculatorData
    {
        internal string UncompletedOperation { get; init; }
        internal IReadOnlyList<string> CompletedOperations { get; init; }
    }
}