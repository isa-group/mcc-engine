using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace isa.Mashups
{
    public class ProjectsState
    {
        public string Proyecto;
        public Boolean Estado;
        private string p;
        private string p_2;

        public ProjectsState(string proy, Boolean est)
        {
            Proyecto = proy;
            Estado = est;
        }

        public ProjectsState(string p, string p_2)
        {
            // TODO: Complete member initialization
            this.p = p;
            this.p_2 = p_2;
        }
    }
}
