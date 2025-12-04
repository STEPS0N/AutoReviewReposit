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
        public DateTime Date_Registration { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }

        public User()
        {
            Feedbacks = new List<Feedback>();
        }
    }
}
