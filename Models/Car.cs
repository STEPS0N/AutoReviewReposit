using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReview.Classes
{
    public class Car
    {
        public int Id_Car { get; set; }
        public string Model_Car { get; set; }
        public int Year_Release { get; set; }
        public string Body_Type { get; set; }
        public decimal Price_Car { get; set; }

        public int Manufacturer_Id { get; set; }
        public int Engine_Id { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Engine Engine { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }

        public Car()
        {
            Equipments = new List<Equipment>();
        }
    }
}
