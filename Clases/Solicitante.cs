using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerCare
{
    public class Solicitante
    {
        public Solicitante(int código, int tipoSolicitante)
        {
            this.código = código;
            this.tipoSolicitante = tipoSolicitante;
        }
        public int código { get; set; }
        public string nombre { get; set; }
        public int tipoSolicitante { get; set; }
    }
}