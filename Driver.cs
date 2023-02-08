using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int carId;
        private int tripTicketId;
        private bool isFree;
        //private ObservableCollection<Order> orders;

        public string Name { get => name; set => Set(ref name, value); }
        public string Patronymic { get => patronymic; set => Set(ref patronymic, value); }
        public string Surname { get => surname; set => Set(ref surname, value); }
        public int CarId { get => carId; set => Set(ref carId, value); }
        public int TripTicketId { get => tripTicketId; set => Set(ref tripTicketId, value); }
        public bool IsFree { get => isFree; set => Set(ref isFree, value); }
        //public ObservableCollection<Order> Orders { get => orders; set => Set(ref orders, value); }
    }
}
