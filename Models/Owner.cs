using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReview.Classes
{
    public class Owner
    {
        public int Id_owner { get; set; }
        public string Fio { get; set; }
        public string Owner_Email { get; set; }
        public string Phone_number { get; set; }

        public virtual ICollection<Manufacturer> Manufacturers { get; set; }

        public Owner()
        {
            Manufacturers = new List<Manufacturer>();
        }
    }
}
