using System;
using System.Collections.Generic;
using System.Text;

namespace AutoReview.Classes
{
    //Отвечает за хранение данных только в процессе сессии, тем самм обеспечивая безопасность
    public class AuthData
    {
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static bool Rights { get; set; } = true;
    }
}
