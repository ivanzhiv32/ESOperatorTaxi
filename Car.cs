using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESOperatorTaxi
{
    class Car:Entity
    {
        private string brand;
        private string number;
        private string colour;
        private string className;

        public string Brand { get => brand; set => Set(ref brand, value); }
        public string Number { get => number; set => Set(ref number, value); }
        public string Colour { get => colour; set => Set(ref colour, value); }
        public string ClassName { get => className; set => Set(ref className, value); }
    }
}
