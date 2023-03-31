using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESOperatorTaxi
{
    [Table("streets")]
    class Street :Entity
    {
        private int cityId;
        private City city;
        private string name;

        [Column("IdCity")]
        public int CityId { get => cityId; set => Set(ref cityId, value); }
        internal City City { get => city; set => Set(ref city, value); }
        [Column("Name")]
        public string Name { get => name; set => Set(ref name, value); }
    }
}
