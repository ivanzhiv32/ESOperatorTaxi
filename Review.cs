using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOperatorTaxi
{
    class Review:Entity
    {
        private int orderId;
        private int score;
        private string comment;

        public int OrderId { get => orderId; set => Set(ref orderId, value); }
        public int Score { get => score; set => Set(ref score, value); }
        public string Comment { get => comment; set => Set(ref comment, value); }
    }
}
