using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESOperatorTaxi
{
    public enum OrderClass
    {
        Econom = 1,
        Comfort,
        Business
    }

    public enum OrderStatus
    {
        Creating = 1,
        Running,
        Completed,
        Cancelled
    }

    [Table("orders")]
    class Order:Entity
    {
        private OrderStatus statusId;
        private string? comment;
        private int price;
        private OrderClass orderClassId;
        private int driverId;
        private Driver driver;
        private int clientId;
        private Client client;
        private DateTime date;
        private int startStreetId;
        private Street startStreet;
        private int startHouseNumber;
        private int startEntranseNumber;
        private int finishStreetId;
        private Street finishStreet;
        private int finishHouseNumber;
        private int finishEntranseNumber;

        [Column("IdStatus")]
        public OrderStatus StatusId { get => statusId; set => Set(ref statusId, value); }
        [Column("Comment")]
        public string Comment { get => comment; set => Set(ref comment, value); }
        [Column("Price")]
        public int Price { get => price; set => Set(ref price, value); }
        [Column("IdOrderClass")]
        public OrderClass OrderClassId { get => orderClassId; set => Set(ref orderClassId, value); }
        [Column("IdDriver")]
        public int DriverId { get => driverId; set => Set(ref driverId, value); }
        public Driver Driver { get => driver; set => Set(ref driver, value); }
        [Column("IdClient")]
        public int ClientId { get => clientId; set => Set(ref clientId, value); }
        public Client Client { get => client; set => Set(ref client, value); }
        [Column("Date")]
        public DateTime Date { get => date; set => Set(ref date, value); }
        [Column("IdStartStreet")]
        public int StartStreetId { get => startStreetId; set => Set(ref startStreetId, value); }
        public Street StartStreet { get => startStreet; set => Set(ref startStreet, value); }
        [Column("StartHouseNumber")]
        public int StartHouseNumber { get => startHouseNumber; set => Set(ref startHouseNumber, value); }
        [Column("StartEntranseNumber")]
        public int StartEntranseNumber { get => startEntranseNumber; set => Set(ref startEntranseNumber, value); }
        public string StartAdress { get => startStreet.Name + " " + Convert.ToString(startHouseNumber); }
        [Column("IdFinishStreet")]
        public int FinishStreetId { get => finishStreetId; set => Set(ref finishStreetId, value); }
        public Street FinishStreet { get => finishStreet; set => Set(ref finishStreet, value); }
        [Column("FinishHouseNumber")]
        public int FinishHouseNumber { get => finishHouseNumber; set => Set(ref finishHouseNumber, value); }
        [Column("FinishEntranseNumber")]
        public int FinishEntranseNumber { get => finishEntranseNumber; set => Set(ref finishEntranseNumber, value); }
        public string FinishAdress { get => finishStreet.Name + " " + Convert.ToString(finishHouseNumber); }

    }
}
