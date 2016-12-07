using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI;
using System.Security;
using System.Configuration;
using System.Web.Configuration;

namespace CustomerCare
{
    public class Perfil
    {
        public int codigo { get; private set; }
        public string perm { get; private set; }
        public Permiso[] permisos { get; set; }
        public bool tienePermiso(Permiso permiso)
        {
            Utilidades.log("permiso: "+permiso.ToString());
            Utilidades.log("perm: " + perm);
            return permisos.Contains(permiso);
        }
        public Perfil(int codigo, string perm)
        {
            this.codigo = codigo;
            this.perm = perm;
        }
    }

    public partial class DAO
    {
        private Permiso[] permisos(int perfil, string  perm)
        {
            string[] pers;
            string permis = "";
            if (perfil > 0)
            {
                SqlCommand qry = new SqlCommand("SELECT permisos FROM Perfiles WHERE Codigo=" + perfil.ToString(), conex);
                try
                {
                    qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    qry.CommandTimeout = 300;
                }
                permis = qry.ExecuteScalar().ToString();
            }
            permis += ",";
            permis += perm;
            permis += ",";
            pers = permis.Split(',');
            Permiso[] result = new Permiso[pers.Length];
            for (int i = 0; i < result.Length; i++)
            {
                try
                {
                    result[i] = (Permiso)Convert.ToInt32(pers[i]);
                }
                catch
                {

                }
            }
            return result;
        }

        public Perfil getPerfil(int codigo, string perm)
        {
            Perfil result = new Perfil(codigo,perm);
            result.permisos = permisos(codigo,perm);
            return result;
        }
    }

    public enum Permiso
    {
        CancelarTickets = 1,
        EscalarTicketsAjenos = 2,
        LevantarTicketsDerivados = 3,
        VerTicketsInactivos = 4,
        VerMenuDeAdministracion = 5,
        VerDatosExtra = 6,
        VerDetalleDeTickets = 7,
        VerEncabezadoPrincipal = 8,
        VerNotasPrivadas = 9,
        VerFiltros = 10,
        AccederPorURL = 11,
        VerTicketsCancelados = 12,
        EditarTickets = 13,
        VerLimiteDeTickets = 14,
        VerResponsableDeTickets = 15,
        TipificarTickets = 16,
        AgregarNotasATickets = 17,
        VerColumnaDeEstado = 18,
        VerColumnaDeIconos = 19,
        VerEventosDeEdicion = 20,
        LevantarTickets = 21,
        VerPaginaDetalleTickets = 22,
        ModificarReportes = 23,
        VerEstadisticas = 24,
        RepactarTickets = 25,
        OrdenarTicketsPorCodigo = 26,
        ModificarTicketsProgramados = 27,
        ModificarFormatos = 28,
        ModificarUsuarios = 29,
        ModificarSolicitantes = 30,
        ModificarTipificaciones = 31,
        ModificarDatosExtra = 32,
        Priorizar = 33,
        PriorizarCritico = 34,
        LevantarAjenas = 35,
        LevantarCerradas = 36,
        Inactivar = 37,
        VerColumnasUsuariosDesarrollo = 38,
        Reprogramar = 39,
        VerProyectoIncidencia = 40
    }
}