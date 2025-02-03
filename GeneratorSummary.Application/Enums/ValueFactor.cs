using Ardalis.SmartEnum;

namespace GeneratorSummary.Application.Enums
{
    public class ValueFactor : SmartEnum<ValueFactor>
    {
        private ValueFactor(string name, int value)
            : base(name, value)
        {
        }

        public static readonly ValueFactor Low = new("Low", 1);
        public static readonly ValueFactor Medium = new("Medium", 2);
        public static readonly ValueFactor High = new("High", 3);
        public static readonly ValueFactor NA = new("N/A", 4);
    }
}
