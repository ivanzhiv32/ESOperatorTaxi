using System;
using System.Collections.Generic;

namespace ESOperatorTaxi
{
    class OperatorService
    {
        private IEnumerable<PriceRule> priceRules;
        private IEnumerable<Driver> drivers;

        public OperatorService(IEnumerable<PriceRule> priceRules, IEnumerable<Driver> drivers) 
        {
            this.priceRules = priceRules;
            this.drivers = drivers;
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
        public Dictionary<Driver, double> ChooseDrivers(Geopoint geoPointFrom, OrderClass orderClass) 
        {
            if (geoPointFrom is null)
                throw new ArgumentNullException(nameof(geoPointFrom));

            return null;
        }
    }
}
