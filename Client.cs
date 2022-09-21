using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOperatorTaxi
{
    class Client : Entity
    {
        private string name;
        private string patronymic;
        private string surname;
        private decimal phoneNumber;

        public string Name { get => name; set => Set(ref name, value); }
        public string Patronymic { get => patronymic; set => Set(ref patronymic, value); }
        public string Surname { get => surname; set => Set(ref surname, value); }
        public decimal PhoneNumber { get => phoneNumber; set => Set(ref phoneNumber, value); }
    }
}
