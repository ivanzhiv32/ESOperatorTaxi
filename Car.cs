using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOperatorTaxi
{
    /// <summary>
    /// Перечисление, характеризующее класс автомобиля
    /// </summary>
    enum CarClass 
    {
        Econom = 1,
        Comfort,
        Business
    }

    [Table("cars")]
    class Car:Entity
    {
        private string brand;
        private string model;
        private string number;
        private string colour;
        private CarClass carClass;

        [Column("Brand")]
        public string Brand { get => brand; set => Set(ref brand, value); }

        [Column("Model")]
        public string Model { get => model; set => Set(ref model, value); }

        [Column("Number")]
        public string Number { get => number; set => Set(ref number, value); }

        [Column("Colour")]
        public string Colour { get => colour; set => Set(ref colour, value); }

        [Column("IdClass")]
        public CarClass CarClass { get => carClass; set => Set(ref carClass, value); }
    }
}
