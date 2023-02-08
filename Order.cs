using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOperatorTaxi
{
    class Order:Entity
    {
        private string comment;
        private int price;
        private string orderClass;
        private string status;
        private int driverId;
        private int clientId;

        public string Comment { get => comment; set => Set(ref comment, value); }
        public int Price { get => price; set => Set(ref price, value); }
        public string OrderClass { get => orderClass; set => Set(ref orderClass, value); }
        public string Status { get => status; set => Set(ref status, value); }
        public int DriverId { get => driverId; set => Set(ref driverId, value); }
        public int ClientId { get => clientId; set => Set(ref clientId, value); }
    }
}
