using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESOperatorTaxi
{
    [Table("cities")]
    class City :Entity
    {
        private string name;

        [Column("Name")]
        public string Name { get => name; set => Set(ref name, value); }
    }
}
