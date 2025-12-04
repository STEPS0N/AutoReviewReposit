using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReview.Classes
{
    public class Feedback
    {
        public int Id_Feedback { get; set; }
        public string Review_Feedback { get; set; }
        public int Rating_Feedback { get; set; }
        public DateTime Date_Feedback { get; set; }

        public int User_Id { get; set; }
        public int Car_Id { get; set; }

        public virtual User User { get; set; }
        public virtual Car Car { get; set; }

        public Feedback() { }
    }
}
