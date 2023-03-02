using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOperatorTaxi
{
    [Table("clients")]
    class Client : Entity
    {
        private string name;
        private string patronymic;
        private string surname;
        private decimal phoneNumber;

        [Column("Name")]
        public string Name { get => name; set => Set(ref name, value); }

        [Column("Patronymic")]
        public string Patronymic { get => patronymic; set => Set(ref patronymic, value); }

        [Column("Surname")]
        public string Surname { get => surname; set => Set(ref surname, value); }

        [Column("PhoneNumber")]
        public decimal PhoneNumber { get => phoneNumber; set => Set(ref phoneNumber, value); }

    }
}
