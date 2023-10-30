using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESOperatorTaxi
{
    /// <summary>
    /// Павило для подбора водителя
    /// </summary>

    [Table("driver_selection_rule")]
    class DriverSelectionRule : Entity
    {
        private int maxDistanceToStartAddress;
        private int minDriverRating;
        private OrderClass orderClass;
        private CarClass carClass;
        private DegreeCompliance degreeCompliance;

        [Column("MaxDistanceToStartAddress")]
        public int MaxDistanceToStartAddress { get => maxDistanceToStartAddress; set => Set(ref maxDistanceToStartAddress, value); }
        [Column("MinDriverRating")]
        public int MinDriverRating { get => minDriverRating; set => Set(ref minDriverRating, value); }
        [Column("OrderClassId")]
        public OrderClass OrderClass { get => orderClass; set => Set(ref orderClass, value); }
        [Column("CarClassId")]
        public CarClass CarClass { get => carClass; set => Set(ref carClass, value); }
        [Column("DegreeComplianceId")]
        public DegreeCompliance DegreeCompliance { get => degreeCompliance; set => Set(ref degreeCompliance, value); }
    }
}
