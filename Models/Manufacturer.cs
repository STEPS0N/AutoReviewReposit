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
        public string Director_Email { get; set; }

        public virtual User Director { get; set; }

        public virtual ICollection<Car> Cars { get; set; }

        public Manufacturer()
        {
            Cars = new List<Car>();
        }
    }
}
