using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace isa.Mashups
{
    public class InfoGeograficaDescription
    {
        string region;

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        string pais;

        public string Pais
        {
            get { return pais; }
            set { pais = value; }
        }

        string areaDeNegocio;
 
        public string AreaDeNegocio
        {
            get { return areaDeNegocio; }
            set { areaDeNegocio = value; }
        }


        public InfoGeograficaDescription(string p, string p_2, string p_3)
        {
            // TODO: Complete member initialization
            this.region = p;
            this.pais = p_2;
            this.areaDeNegocio = p_3;
        }
    }
}
