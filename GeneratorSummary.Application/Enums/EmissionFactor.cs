using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorSummary.Application.Enums
{
    public class EmissionFactor : SmartEnum<EmissionFactor>
    {
        private EmissionFactor(string name, int value)
            : base(name, value)
        {
        }

        public static readonly EmissionFactor Low = new("Low", 1);
        public static readonly EmissionFactor Medium = new("Medium", 2);
        public static readonly EmissionFactor High = new("High", 3);
        public static readonly EmissionFactor NA = new("N/A", 4);
    }
}
