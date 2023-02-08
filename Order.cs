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
        private int statusId;
        private int orderClassId;
        private int driverId;
        private int clientId;

        public string Comment { get => comment; set => Set(ref comment, value); }
        public int Price { get => price; set => Set(ref price, value); }
        public int OrderClassId { get => orderClassId; set => Set(ref orderClassId, value); }
        public int StatusId { get => statusId; set => Set(ref statusId, value); }
        public int DriverId { get => driverId; set => Set(ref driverId, value); }
        public int ClientId { get => clientId; set => Set(ref clientId, value); }
    }
}
