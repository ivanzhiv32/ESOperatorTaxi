using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text.RegularExpressions;

namespace ESOperatorTaxi
{
    [Table("drivers")]
    class Driver :Entity
    {
        private string surname;
        private string name;
        private string patronymic;
        private int carId;
        private Car car;
        private bool isFree;
        private Int64 phoneNumber;

        [Column("Name")]
        public string Name { get => name; set => Set(ref name, value); }
        [Column("Surname")]
        public string Surname { get => surname; set => Set(ref surname, value); }
        [Column("Patronymic")]
        public string Patronymic { get => patronymic; set => Set(ref patronymic, value); }
        [Column("idCar")]
        public int CarId { get => carId; set => Set(ref carId, value); }
        public Car Car { get => car; set => Set(ref car, value); }
        [Column("isFree")]
        public bool IsFree { get => isFree; set => Set(ref isFree, value); }
        public string Status { get
            {
                if (isFree) return "Свободен";
                else return "Занят";
            }}
        [Column("PhoneNumber")]
        public Int64 PhoneNumber { get => phoneNumber; set => Set(ref phoneNumber, value); }

        public Geopoint GetLocation()
        {
            List<string> streets = new List<string>();

            StreamReader sr = new StreamReader("streets.json", Encoding.UTF8);

            var text = sr.ReadToEnd();
            string pattern = @"""(\w*)""";
            foreach (Match match in Regex.Matches(text, pattern, RegexOptions.IgnoreCase))
            {
                streets.Add(match.Groups[1].Value);
            }
            Random rnd = new Random();
            Geopoint geopoint = new Geopoint("Брянск", streets[rnd.Next(0,  streets.Count)], rnd.Next(1, 100));
            return geopoint;
        }
        
    }
}
