
namespace ESOperatorTaxi
{
    /// <summary>
    /// Правило для определения базовой ставки стоимости проезда за Км
    /// </summary>
    class PriceRule : Entity
    {
        private int startRange;
        private int endRange;
        private OrderClass orderClass;
        private decimal pricePerKm;

        public int StartRange { get => startRange; set => Set(ref startRange, value); }
        public int EndRange { get => endRange; set => Set(ref endRange, value); }
        public OrderClass OrderClass { get => orderClass; set => Set(ref orderClass, value); }
        public decimal PricePerKm { get => pricePerKm; set => Set(ref pricePerKm, value); }
    }
}
