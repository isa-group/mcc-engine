using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using icinetic.CatalogoProyectos;
using System.Diagnostics;

namespace icinetic.BPCMSPipes
{
    public class ProyectoSubproyecto
    {
        public string Estado { get { return _estado; } }
        private string _estado;

        // Stored in short name
        public string Direccion { get { return direccion; } }
        private string direccion;

        public string Area  {    get { return area; }        }
        private string area;

        public string Pais { get { return pais; }}
        private string pais;


        public string Titulo { get { return proyecto_Titulo; } }
        private string proyecto_Titulo;

        public string TituloPadre { get { return tituloPadre; } }
        private string tituloPadre;

        public Nullable<DateTime> Fecha_Inicio { get { return fecha_Inicio; } }
        private Nullable<DateTime> fecha_Inicio;

        public Nullable<DateTime> Fecha_Fin  {  get { return fecha_Fin; }  }
        private Nullable<DateTime> fecha_Fin;

        public Nullable<DateTime> Fecha_Fin_Replanificada  { get { return fecha_Fin_Replanificada; }  }
        private Nullable<DateTime> fecha_Fin_Replanificada;

        public string Tipologia { get { return tipologia; } }
        private string tipologia;

        public string Linea { get { return linea; } }
        private string linea;

        public Proyecto Proyecto { get { return proyecto; } }
        private Proyecto proyecto;

        public SubProyecto SubProyecto { get { return subproyecto; } }
        private SubProyecto subproyecto;

        public string Clasificacion { get { return _clasificacion; } }
        private string _clasificacion;

        public string Importancia { get { return _importancia; } }
        private string _importancia;

        public string BusinessPartner { get { return _businessPartner; } }
        private string _businessPartner;

        public ProyectoSubproyecto(Proyecto p)
        {
            proyecto = p;
            subproyecto = null;
            tituloPadre = "";
            linea = p.Linea;
            proyecto_Titulo = p.Proyecto_Titulo;
            _estado = p.Estado_Proyecto;
            direccion = p.Direccion;
            area = p.Empresa_Cliente;
            pais = p.Pais;
            fecha_Inicio = p.Fecha_Inicio;
            fecha_Fin = p.Fecha_Fin;
            fecha_Fin_Replanificada = p.Fecha_Fin_Replanificada;
            tipologia = p.Tipologia;
            _clasificacion = p.Clasificacion;
            _importancia = p.Importancia_Para_Negocio;
            _businessPartner = p.Business_Partner;
            
        }

        public ProyectoSubproyecto(SubProyecto sp)
        {
            subproyecto = sp;
            proyecto = null;
            _estado = sp.Estado_Subproyecto;
            tipologia = sp.Tipologia;
            proyecto_Titulo = sp.Subproyecto;
            tituloPadre = sp.Proyecto_POA;
            linea = sp.Linea_Estrategica;
            _businessPartner = sp.Business_Partner_1;

            if (sp.Proyecto != null)
            {
                direccion = DireccionConversor.ToShortName(sp.Proyecto.Direccion);
                pais = sp.Proyecto.Pais;
                fecha_Inicio = sp.Proyecto.Fecha_Inicio;
                fecha_Fin = sp.Proyecto.Fecha_Fin;
                fecha_Fin_Replanificada = sp.Proyecto.Fecha_Fin_Replanificada;
                area = sp.Proyecto.Empresa_Cliente;
                _clasificacion = sp.Proyecto.Clasificacion;
                _importancia = sp.Proyecto.Importancia_Para_Negocio;
            }
            else
            {
                Debug.WriteLine(string.Format("ERROR: El subproyecto {0} no está asociado a ningún proyecto", Titulo), "ProyectoSubproyecto");
                direccion = sp.Sd;
                pais = "Unknown";
                fecha_Inicio = null;
                fecha_Fin = null;
                fecha_Fin_Replanificada = null;
                area = sp.Area_Funcional_Siglas;
                _clasificacion = "NORMAL";
                _importancia = "MEDIA";
            }
        }

        public bool IsSubproyecto()
        {
            return proyecto == null && subproyecto != null;
        }

        public bool HasSubproyectos()
        {
            return proyecto != null && proyecto.SubProyectoList.Count > 0;
        }

        public override string ToString()
        {
            return string.Format("Proyecto {0} [{1}, {2}, {3}, {4}] est: {5}, fechaini: {6}, clasif: {7}, import: {8}, bp: {9}, subproy: {10}, hijos: {11}", 
                Titulo, 
                Region, Pais, Area, Direccion,
                Estado,
                Fecha_Inicio,
                Clasificacion,
                Importancia,
                BusinessPartner,
                IsSubproyecto(),
                HasSubproyectos());
        }

    }
}
