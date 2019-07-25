using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa.Mashups;
using icinetic.BPCMSPipes;
using de.ahzf.Styx;
using icinetic.CatalogoProyectos;

namespace Mashups
{
    class BooleanControlProject : IControlProject
    {
        private IEnumerable<MashupConfiguration> mashups;

        public ControlInformation Information { get { return info; } }
        private ControlInformation info;

        public BooleanControlProject(ControlInformation info, IEnumerable<MashupConfiguration> mashups)
        {
            this.mashups = mashups;
            this.info = info;
        }

        #region IControl<List<ProjectEvaluation>> Members

        public string Name
        {
            get { return info.ControlDescription.Id; }
        }
        
        public string Description
        {
            get { return info.ControlDescription.Description; }
        }
        
        public List<ControlProjectEvaluation> Evaluate()
        {
            List<List<ControlProjectEvaluation>> evaluations = new List<List<ControlProjectEvaluation>>();
            List<ControlProjectEvaluation> result;
            
            result = SetAllTrue();
            return result;

        }

        private List<ControlProjectEvaluation> SetAllTrue()
        {
            List<ControlProjectEvaluation> result = new List<ControlProjectEvaluation>();

            // Data source
            IPipe<Proyecto, Proyecto> projectPipe = new ProyectoPipe();
            projectPipe.SetSourceCollection(MashupsBuilder.GetProjectsDataSource());

            // Filtro por subproyecto (si tiene subproyectos fuera)
            IPipe<Proyecto, Proyecto> filterSubprojectsPipe = new FuncFilterPipe<Proyecto>(delegate(Proyecto p) { return (p.SubProyectoList.Count > 0); });
            filterSubprojectsPipe.SetSourceCollection(projectPipe);

            foreach (Proyecto p in filterSubprojectsPipe)
            {
                ProyectoSubproyecto ps = new ProyectoSubproyecto(p);
                ControlProjectEvaluation pEval = new ControlProjectEvaluation(
                    new Context(ps.Region, ps.Pais, ps.Area, ps.Direccion, ps.Linea),
                    ps, true);

                result.Add(pEval);
            }

            // Data source
            projectPipe = new ProyectoPipe();
            projectPipe.SetSourceCollection(MashupsBuilder.GetProjectsDataSource());

            // Obtenemos subproyectos
            IPipe<Proyecto, SubProyecto> subProyectPipe = new SubProyectoPipe();
            subProyectPipe.SetSourceCollection(projectPipe);

            foreach (SubProyecto p in subProyectPipe)
            {
                ProyectoSubproyecto ps = new ProyectoSubproyecto(p);
                ControlProjectEvaluation pEval = new ControlProjectEvaluation(
                    new Context(ps.Region, ps.Pais, ps.Area, ps.Direccion, ps.Linea),
                    ps, true);

                result.Add(pEval);
            }

            return result;

        }

        #endregion
    }
}
