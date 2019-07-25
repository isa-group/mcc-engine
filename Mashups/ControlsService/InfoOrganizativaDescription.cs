using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace isa.Mashups
{
    public class InfoOrganizativaDescription
    {
        string direccion;

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        string departamento;

        public string Departamento
        {
            get { return departamento; }
            set { departamento = value; }
        }

        public InfoOrganizativaDescription(string p, string p_2)
        {
            // TODO: Complete member initialization
            this.direccion = p;
            this.departamento = p_2;
        }
    }
}
