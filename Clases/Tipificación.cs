using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerCare
{
    public class Tipificación
    {
        public Tipificación(int código)
        {
            this.código = código;
        }
        public int código { get; set; }
        public string descripción { get; set; }
    }
}