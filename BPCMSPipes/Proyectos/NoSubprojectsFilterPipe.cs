using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using icinetic.CatalogoProyectos;
using de.ahzf.Styx;
using isa.BPCMSPipes;

namespace icinetic.BPCMSPipes.Proyectos
{
    // Filtro por subproyecto: Sólo deja pasar si no tiene subproyectos
    public class NoSubprojectsFilterPipe : FuncFilterPipe<Proyecto>
    {
        public NoSubprojectsFilterPipe(string documentName)
            : base(delegate(Proyecto p) { return (p.SubProyectoList.Count > 0); })
        {
        }
    }

}
