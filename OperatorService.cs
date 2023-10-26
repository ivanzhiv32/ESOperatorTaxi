using System;
using System.Collections.Generic;
using System.Linq;

namespace ESOperatorTaxi
{
    /// <summary>
    /// Служба поддержки принятия решений для оператора
    /// </summary>
    class OperatorService
    {
        private IEnumerable<PriceRule> priceRules;
        private IEnumerable<DriverSelectionRule> driverSelectionRules;
        private IEnumerable<Driver> drivers;
        private IEnumerable<DriverRating> driverRatings;

        public OperatorService(
            IEnumerable<PriceRule> priceRules, 
            IEnumerable<DriverSelectionRule> driverSelectionRules, 
            IEnumerable<Driver> drivers,
            IEnumerable<DriverRating> driverRatings) 
        {
            this.priceRules = priceRules;
            this.driverSelectionRules = driverSelectionRules;
            this.drivers = drivers;
            this.driverRatings = driverRatings;
        }

        /// <summary>
        /// Получить актуальный рейтинг водителя
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        private double GetActualDriverRating(Driver driver) 
        {
            double rating = 0;
            DriverRating driverRating = driverRatings.Where(d => d.Driver == driver).OrderByDescending(d => d.Date).FirstOrDefault();
            if (driverRating != null)
                rating = driverRating.Rating;

            return rating;
        }

        /// <summary>
        /// Рассчитать базовую стоимость поездки
        /// </summary>
        /// <param name="geoPointFrom"></param>
        /// <param name="geoPointTo"></param>
        /// <param name="orderClass"></param>
        /// <returns></returns>
        public decimal CalculateBasePrice(Geopoint geoPointFrom, Geopoint geoPointTo, OrderClass orderClass) 
        {
            if (geoPointFrom is null)
                throw new ArgumentNullException(nameof(geoPointFrom));
            if (geoPointTo is null)
                throw new ArgumentNullException(nameof(geoPointTo));

            double distance = geoPointFrom.GetDistanceTo(geoPointTo);
            decimal price = 0; // Какую базовую ставку по-умолчанию указать и нужно ли ее указывать?
            bool isRuleFound = false;
            foreach (PriceRule rule in priceRules)
            {
                if (distance > rule.StartRange && distance < rule.EndRange && rule.OrderClass == orderClass) 
                {
                    price = rule.PricePerKm * (decimal)distance;
                    isRuleFound = true;
                    break;
                }
            }

            if (!isRuleFound)
                throw new Exception("Не найдено правило для определения базовой ставки.");

            return price;
        }

        /// <summary>
        /// Подобрать водителей для заявки
        /// </summary>
        /// <param name="geoPointFrom"></param>
        /// <param name="orderClass"></param>
        /// <returns>Выбранные водители в формате: водитель - степень соответствия</returns>
        public Dictionary<Driver, DegreeCompliance> ChooseDrivers(Geopoint geoPointFrom, OrderClass orderClass) 
        {
            if (geoPointFrom is null)
                throw new ArgumentNullException(nameof(geoPointFrom));

            var result = new Dictionary<Driver, DegreeCompliance>();
            foreach (var driver in drivers.Where(d => d.IsFree))
            {
                double distance = driver.GetLocation().GetDistanceTo(geoPointFrom);
                double rating = GetActualDriverRating(driver);

                // Выбор подходящих правил
                var rules = driverSelectionRules.Where(rule => 
                {
                    return rule.CarClass == driver.Car.CarClass &&
                    rule.OrderClass == orderClass &&
                    distance <= rule.MaxDistanceToStartAddress &&
                    rating >= rule.MinDriverRating;
                });

                if (rules.Count() > 0) 
                {
                    // Выбор наилучшей степени соответствия
                    DegreeCompliance degreeCompliance = rules.Min(r => r.DegreeCompliance);
                    result.Add(driver, degreeCompliance);
                }
            }

            return result;
        }
    }
}
