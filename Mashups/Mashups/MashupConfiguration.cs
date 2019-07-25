using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using icinetic.BPCMSPipes;
using icinetic.BPMMetamodel;
using icinetic.BPCMSPipes.Proyectos;

namespace Mashups
{
    public class MashupConfiguration
    {
        public IEnumerable<MashupElement> Roots { get { return Roots; }}
        private IEnumerable<MashupElement> roots;
        public IDictionary<string, string> Parameters { get { return parameters; } }
        private IDictionary<string, string> parameters;

        public MashupConfiguration(MashupElement root)
            : this(new MashupElement[] { root })
        {
        }
        public MashupConfiguration(IEnumerable<MashupElement> roots)
            : this(roots, new Dictionary<string, string>())
        {
        }

        public MashupConfiguration(IEnumerable<MashupElement> roots, IDictionary<string, string> parameters)
        {
            this.roots = roots;
            this.parameters = parameters;
        }

        public bool IsProjectMashup()
        {
            return roots.Any(e => e.Stereotype.ToUpper().Equals(typeof(ProyectoSubproyectoDataSource).Name.ToUpper()));
        }

        public MashupContainer Build()
        {
            MashupBuilder builder = new MashupBuilder(roots);
            return builder.Build(parameters);
        }
    }
}
