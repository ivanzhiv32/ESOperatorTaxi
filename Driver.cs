using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOperatorTaxi
{
    class Driver:Entity
    {
        //Problem with 'bool' type
        private string surname;
        private string name;
        private string patronymic;
        private string numberCar;
        //private bool isFree;

        public string Name { get => name; set => Set(ref name, value); }
        public string Patronymic { get => patronymic; set => Set(ref patronymic, value); }
        public string Surname { get => surname; set => Set(ref surname, value); }
        public string NumberCar { get => numberCar; set => Set(ref numberCar, value); }
        //public bool IsFree { get => isFree; set => Set(ref isFree, value); }
    }
}
