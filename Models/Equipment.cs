using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoReview.Classes
{
    //Equipment представляет из себя комплектации для любого автомобиля, его название и уровень
    public class Equipment
    {
        public int Id_Equipment { get; set; }
        public string Title_Equipment { get; set; }
        public string Equipment_Level { get; set; }

        public int Car_Id { get; set; }

        public virtual Car Car { get; set; }

        public Equipment() { }

    }
}
