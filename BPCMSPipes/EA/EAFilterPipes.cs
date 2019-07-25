using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using de.ahzf.Styx;
using icinetic.BPMMetamodel;


namespace icinetic.BPCMSPipes.EA
{
    public class FiltrarActividadPipe : FuncFilterPipe<Actividad>
    {
        public FiltrarActividadPipe(string name) 
            : base(delegate(Actividad a) { return !a.Nombre.Equals(name); })
        {
        }
    }

    public class FiltrarActivoPipe : FuncFilterPipe<Activo>
    {
        public FiltrarActivoPipe(string name) 
            : base(delegate(Activo a) { return !a.Nombre.Equals(name); })
        {
        }
    }

    public class FiltrarActorPipe : FuncFilterPipe<Actor>
    {
        public FiltrarActorPipe(string name) 
            : base(delegate(Actor a) { return !a.Nombre.Equals(name); })
        {
        }
    }

    public class ModelElementFilterPipe : FuncFilterPipe<ModelElement>
    {
        public ModelElementFilterPipe(string modelElementName)
            : base(delegate(ModelElement a) { return (a.Nombre != null) && (!a.Nombre.Equals(modelElementName)); })
        {
        }
    }


}
