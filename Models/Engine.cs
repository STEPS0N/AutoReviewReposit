using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReview.Classes
{
    public class Engine
    {
        public int Id_Engine { get; set; }
        public string Type_Engine { get; set; }
        public decimal Capacity_Engine { get; set; }
        public int Power_Engine { get; set; }

        public virtual ICollection<Car> Cars { get; set; }

        public Engine()
        {
            Cars = new List<Car>();
        }
    }
}
