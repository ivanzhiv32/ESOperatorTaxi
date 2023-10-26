
namespace ESOperatorTaxi
{
    /// <summary>
    /// Правило для определения базовой ставки стоимости проезда за Км
    /// </summary>
    class PriceRule : Entity
    {
        private uint startRange;
        private uint endRange;
        private OrderClass orderClass;
        private decimal pricePerKm;

        public uint StartRange { get => startRange; set => Set(ref startRange, value); }
        public uint EndRange { get => endRange; set => Set(ref endRange, value); }
        public OrderClass OrderClass { get => orderClass; set => Set(ref orderClass, value); }
        public decimal PricePerKm { get => pricePerKm; set => Set(ref pricePerKm, value); }
    }
}
