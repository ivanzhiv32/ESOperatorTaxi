using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESOperatorTaxi
{
    [Table("driver_ratings")]
    class DriverRating:Entity
    {
        private double rating;
        private DateTime date;
        private int idDriver;
        private Driver driver;

        [Column("Rating")]
        public double Rating { get => rating; set => Set(ref rating, value); }
        [Column("DateTime")]
        public DateTime Date { get => date; set => Set(ref date, value); }
        [Column("IdDriver")]
        public int IdDriver { get => idDriver; set => Set(ref idDriver, value); }
        public Driver Driver { get => driver; set => Set(ref driver, value); }

    }
}
