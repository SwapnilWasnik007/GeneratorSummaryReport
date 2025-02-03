using Ardalis.SmartEnum;

namespace GeneratorSummary.Application.Enums
{
    public class GeneratorType : SmartEnum<GeneratorType>
    {
        public ValueFactor ValueFactor { get; }
        public EmissionFactor EmissionFactor { get; }

        private GeneratorType(string name, int value, ValueFactor valueFactor, EmissionFactor emissionFactor)
            : base(name, value)
        {
            ValueFactor = valueFactor;
            EmissionFactor = emissionFactor;
        }

        public static readonly GeneratorType OffshoreWind = new("Offshore Wind", 1, ValueFactor.Low, EmissionFactor.NA);
        public static readonly GeneratorType OnshoreWind = new("Onshore Wind", 2, ValueFactor.High, EmissionFactor.NA);
        public static readonly GeneratorType Gas = new("Gas", 3, ValueFactor.Medium, EmissionFactor.Medium);
        public static readonly GeneratorType Coal = new("Coal", 4, ValueFactor.Medium, EmissionFactor.High);
    }
}
