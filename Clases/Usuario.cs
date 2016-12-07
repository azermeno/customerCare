using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace CustomerCare
{

    public class Usuario : Clase
    {
        private DAO dao;
        public Usuario(int código) : base(código)
        {
            this.dao = new DAO();
        }
        public new Usuario responsable { get{return this;}}
        public string login { get; set; }
        public bool activo { get; set; }
        public Perfil perfil { get; set; }
        public bool interno { get; set; }
        public string filtroDefault { get; set; }
        public int tipoUsuario { get; set; }
        public string permisos { get; set; }
        public bool tienePermiso(Permiso permiso)
        {
            Utilidades.log("Usuario: " + nombre);
            Utilidades.log("Perfil permisos: " + perfil.permisos.Count().ToString());
            Utilidades.log("Perfil perm: " + perfil.perm);
            return perfil.tienePermiso(permiso);
        }
        public List<Clase> getReceptores()
        {
            dao.open();
            List<Clase> receptores;
            receptores = dao.getReceptores(this);
            dao.close();
            return receptores;
        }
    }

    public partial class DAO
    {
        
        public Usuario getUsuario(int código)
        {
            DAO daoAux=new DAO();
            Usuario result;
            SqlCommand qry = new SqlCommand("Select Nombre, login, activo, isnull(usuario,0), perfil, filtrodefault, tipoUsuario, permisos from Usuarios u left join Internos i on u.codigo=i.usuario where u.codigo=" + código, conex);
            try
            {
                qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                qry.CommandTimeout = 300;
            }
            Utilidades.log("Se obtinie datos del usuario");
            Utilidades.log(qry.CommandText);
            //conex.Open();
            //try{
            SqlDataReader reader = qry.ExecuteReader();
            if (reader.Read())
            {
                result = new Usuario(código);
                result.nombre = reader.GetString(0);
                result.login = reader.GetString(1);
                result.activo = reader.GetBoolean(2);
                result.interno = reader.GetInt32(3) > 0;
                daoAux.open();
                result.perfil = daoAux.getPerfil(reader.GetInt32(4), reader.GetString(7));
                daoAux.close();
                result.filtroDefault = reader.GetString(5);
                result.tipoUsuario = reader.GetInt32(6);
                result.permisos = reader.GetString(7);
                reader.Close();
            }
            else
            {
                reader.Close();
                return null;
            }
            //}
            //finally { conex.Close(); }
            return result;
        }

        //public bool tienePermiso (int codigo, Permiso permiso)
        //{
        //    return permisos(codigo).Contains(permiso);
        //}

        public List<Clase> getPadresClase(Clase clase)
        {
            DAO daoAux=new DAO();
            List<Clase> result = new List<Clase>();
            SqlCommand query = new SqlCommand("", conex);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            if (clase is Usuario)
                query.CommandText="SELECT Clase from Subscripciones where Usuario=" + clase.código;
            else
                query.CommandText="Select Padre from RelacionesClases where Hijo=" + clase.código;
            SqlDataReader reader = query.ExecuteReader();
            while(reader.Read())
            {
                daoAux.open();
                result.Add(daoAux.getClase(reader.GetInt32(0)));
                daoAux.close();
            }
            return result;
        }

        public List<Clase> getHijosClase(Clase clase)
        {
            DAO daoAux=new DAO();
            List<Clase> result = new List<Clase>();
            SqlCommand query = new SqlCommand("", conex);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            if (clase is Usuario)
                return new List<Clase>();
            else
                query.CommandText="Select Hijo from RelacionesClases where Padre=" + clase.código;
            SqlDataReader reader = query.ExecuteReader();
            while(reader.Read())
            {
                daoAux.open();
                result.Add(daoAux.getClase(reader.GetInt32(0)));
                daoAux.close();
            }
            return result;
        }

        public List<Clase> getReceptores(Usuario usuario)
        {
            SqlCommand query = new SqlCommand("", conex);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            List<Clase> result = new List<Clase>();
            if (!usuario.interno)
            {
                try
                {
                    query.CommandText = "SELECT TOP 1 Clase from Subscripciones WHERE Usuario=" + usuario.código;
                    result.Add(this.getClase((int)query.ExecuteScalar()).responsable);
                }
                catch
                {
                }
            }
            else
            {
                Clase temp = usuario;
                List<Clase> padres = getPadresClase(usuario);
                while (padres.Count > 0)
                {
                    List<Clase> padrestemp = new List<Clase>();
                    foreach (Clase padre in padres)
                    {
                        result.Add(padre);
                        result.AddRange(getHijosClase(padre));
                        padrestemp.AddRange(getPadresClase(padre));
                    }
                    padres = padrestemp;
                }
                for (int i = 0; i < result.Count; i++)
                    for (int j = i + 1; j < result.Count; j++)
                        if (result[i].código == result[j].código && result[i].nombre == result[j].nombre)
                        {
                            result.RemoveAt(i);
                            i--;
                        }
            }
            return result;
        }
    }
}