using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerCare
{
    public class Propiedad
    {
        private int _Codigo;
        private string _Nombre;
        public Propiedad()
        {
        }
        public Propiedad(int cod, string nom)
        {
            this._Codigo = cod;
            this._Nombre = nom;
        }
        public int c
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }
        public string n
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
    }
}