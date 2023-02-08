using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOperatorTaxi
{
    class TripTicket:Entity
    {
        private int milleage;
        private DateTime dateStart;
        private DateTime dateEnd;

        public int Milleage { get => milleage; set => Set(ref milleage, value); }
        public DateTime DateStart { get => dateStart; set => Set(ref dateStart, value); }
        public DateTime DateEnd { get => dateEnd; set => Set(ref dateEnd, value); }
    }
}
