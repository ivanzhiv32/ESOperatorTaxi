using System.ComponentModel.DataAnnotations.Schema;

namespace ESOperatorTaxi
{
    /// <summary>
    /// Правило для определения базовой ставки стоимости проезда за Км
    /// </summary>

    [Table("price_rules")]
    class PriceRule : Entity
    {
        private int startRange;
        private int endRange;
        private OrderClass orderClass;
        private decimal pricePerKm;
        private int boarding;

        [Column("StartRange")]
        public int StartRange { get => startRange; set => Set(ref startRange, value); }
        [Column("EndRange")]
        public int EndRange { get => endRange; set => Set(ref endRange, value); }
        [Column("OrderClassId")]
        public OrderClass OrderClass { get => orderClass; set => Set(ref orderClass, value); }
        [Column("PricePerKm")]
        public decimal PricePerKm { get => pricePerKm; set => Set(ref pricePerKm, value); }
        [Column("Boarding")]
        public int Boarding { get => boarding; set => Set(ref boarding, value); }
    }
}
