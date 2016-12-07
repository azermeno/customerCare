using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerCare
{
    public class Evento
    {
        public TipoEvento tipo { get; set; }
        public string texto { get; set; }
        public DateTime fecha { get; set; }
        public int? usuario { get; set; }
        public Evento(TipoEvento tipo, string texto, DateTime fecha)
        {
            this.tipo = tipo;
            this.texto = texto;
            this.fecha = fecha;
            this.usuario = null;
        }
        public Evento(TipoEvento tipo, int usuario, string texto, DateTime fecha)
        {
            this.tipo = tipo;
            this.texto = texto;
            this.fecha = fecha;
            this.usuario = usuario;
        }
    }
}