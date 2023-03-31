using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESOperatorTaxi
{
    [Table("reviews")]
    class Review:Entity
    {
        private int orderId;
        private Order order;
        private int score;
        private string comment;

        [Column("IdOrder")]
        public int OrderId { get => orderId; set => Set(ref orderId, value); }
        internal Order Order { get => order; set => Set(ref order, value); }
        [Column("Score")]
        public int Score { get => score; set => Set(ref score, value); }
        [Column("Comment")]
        public string Comment { get => comment; set => Set(ref comment, value); }
    }
}
