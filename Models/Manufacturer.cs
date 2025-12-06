using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReview.Classes
{
    public class Manufacturer
    {
        public int Id_Manufacturer { get; set; }
        public string Title_Brand { get; set; }
        public string Country_Brand { get; set; }
        public int Id_owner { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual ICollection<Car> Cars { get; set; }

        public Manufacturer()
        {
            Cars = new List<Car>();
        }
    }
}
