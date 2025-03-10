using System;
using System.Collections.Generic;

namespace Application.Domain
{
    [Serializable]
    public struct CalculatorData
    {
        public string UncompletedOperation { get; init; }
        public string[] CompletedOperations { get; init; }
    }
}