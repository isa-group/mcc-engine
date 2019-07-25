using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace isa.Mashups
{
    public class EvidenciaDescription
    {
        string tipo;

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public bool PorProyecto
        {
            get { return porProyecto; }
            set { PorProyecto = value; }
        }
        private bool porProyecto;

        public EvidenciaDescription(string t, string n, string d, bool porProyecto)
        {
            tipo = t;
            nombre = n;
            descripcion = d;
            this.porProyecto = porProyecto;
        }
    }
}
