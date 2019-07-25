using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using icinetic.BPCMSPipes;

namespace isa.Mashups
{
    public class ControlProjectEvaluation : ControlEvaluation
    {
        public String Project;
        private ProyectoSubproyecto p;

        public ControlProjectEvaluation(Context ctx, ProyectoSubproyecto projectName, bool eval)
            : base(ctx, eval)
        {
            Project = projectName.Titulo;
            p = projectName;
        }

        public ControlProjectEvaluation(Context ctx, ProyectoSubproyecto projectName)
            : base(ctx)
        {
            Project = projectName.Titulo;
            p = projectName;
        }
    
    }
}
