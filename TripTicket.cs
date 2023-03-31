using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESOperatorTaxi
{
    [Table("trip_tickets")]
    class TripTicket:Entity
    {
        private int driverId;
        private Driver driver;
        private int milleage;
        private DateTime dateStart;
        private DateTime dateEnd;

        [Column("IdDriver")]
        public int DriverId { get => driverId; set => Set(ref driverId, value); }
        public Driver Driver { get => driver; set => Set(ref driver, value); }
        [Column("Mileage")]
        public int Milleage { get => milleage; set => Set(ref milleage, value); }
        [Column("DateStart")]
        public DateTime DateStart { get => dateStart; set => Set(ref dateStart, value); }
        [Column("DateEnd")]
        public DateTime DateEnd { get => dateEnd; set => Set(ref dateEnd, value); }
    }
}
