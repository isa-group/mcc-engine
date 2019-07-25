using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using icinetic.GesMaApp;
using de.ahzf.Styx;
using System.Diagnostics;
using System.Globalization;

namespace icinetic.BPCMSPipes.GesMa
{
    public class FilterByClasificacion : FuncFilterPipe<ProyectoSubproyecto>
    {
        public FilterByClasificacion(string clasificacion)
            : base(delegate(ProyectoSubproyecto p) 
                {
                    //bool removeIf = !p.Clasificacion.Equals(clasificacion, StringComparison.CurrentCultureIgnoreCase);
                    bool removeIf = string.Compare(p.Clasificacion, clasificacion, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) != 0;

                    Debug.WriteLineIf(removeIf,
                        string.Format("Project {0} with clasificacion {1} removed [{2}]", p.Titulo, p.Clasificacion, clasificacion),
                        "FilterByClasificacion");

                    return (removeIf); 
                })
        {
        }
    }

    public class FilterByImportancia : FuncFilterPipe<ProyectoSubproyecto>
    {
        public FilterByImportancia(string importancia)
            : base(delegate(ProyectoSubproyecto p) 
                {
                    //bool removeIf = (!p.Importancia.Equals(importancia, StringComparison.CurrentCultureIgnoreCase));

                    bool removeIf = (String.Compare(p.Importancia, importancia, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) != 0);

                    Debug.WriteLineIf(removeIf,
                        string.Format("Project {0} with clasificacion {1} removed [{2}]", p.Titulo, p.Importancia, importancia),
                        "FilterByImportancia");

                    return removeIf; 
                })
        {
        }
    }
}
