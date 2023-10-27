using System.ComponentModel.DataAnnotations.Schema;

namespace ESOperatorTaxi
{
    /// <summary>
    /// Правило для определения базовой ставки стоимости проезда за Км
    /// </summary>

    [Table("price_rules")]
    class PriceRule : Entity
    {
        private uint startRange;
        private uint endRange;
        private OrderClass orderClass;
        private decimal pricePerKm;
        private uint boarding;

        [Column("StartRange")]
        public uint StartRange { get => startRange; set => Set(ref startRange, value); }
        [Column("EndRange")]
        public uint EndRange { get => endRange; set => Set(ref endRange, value); }
        [Column("OrderClassId")]
        public OrderClass OrderClass { get => orderClass; set => Set(ref orderClass, value); }
        [Column("PricePerKm")]
        public decimal PricePerKm { get => pricePerKm; set => Set(ref pricePerKm, value); }
        [Column("Boarding")]
        public uint Boarding { get => boarding; set => Set(ref boarding, value); }
    }
}
