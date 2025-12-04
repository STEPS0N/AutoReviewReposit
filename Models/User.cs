using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReview.Classes
{
    public class User
    {
        public int Id_user { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email_User { get; set; }
        public string Type_Right { get; set; }
        public DateTime Date_Registration { get; set; }

        public virtual ICollection<Manufacturer> ManagedManufacturers { get; set; }

        public User()
        {
            Type_Right = "Пользователь";
            ManagedManufacturers = new List<Manufacturer>();
        }
    }
}
