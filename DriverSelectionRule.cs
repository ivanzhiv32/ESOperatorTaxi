using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOperatorTaxi
{
    /// <summary>
    /// Павило для подбора водителя
    /// </summary>
    class DriverSelectionRule : Entity
    {
        private uint maxDistanceToStartAddress;
        private uint minDriverRating;
        private OrderClass orderClass;
        private CarClass carClass;
        private DegreeCompliance degreeCompliance;

        public uint MaxDistanceToStartAddress { get => maxDistanceToStartAddress; set => Set(ref maxDistanceToStartAddress, value); }
        public uint MinDriverRating { get => minDriverRating; set => Set(ref minDriverRating, value); }
        public OrderClass OrderClass { get => orderClass; set => Set(ref orderClass, value); }
        public CarClass CarClass { get => carClass; set => Set(ref carClass, value); }
        public DegreeCompliance DegreeCompliance { get => degreeCompliance; set => Set(ref degreeCompliance, value); }
    }
}
