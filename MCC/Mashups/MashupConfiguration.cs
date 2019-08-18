using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace isa.MCC.Mashups
{
    /// <summary>
    /// This class represents a specific configuration of a mashup composed of
    /// its root MashupElement and its parameters. It allows one to obtain
    /// an instantiated mashup (MashupContainer) using method Build.
    /// </summary>
    public class MashupConfiguration
    {
        public IEnumerable<MashupElement> Roots { get { return roots; }}
        private IEnumerable<MashupElement> roots;
        public IDictionary<string, string> Parameters { get { return parameters; } }
        private IDictionary<string, string> parameters;

        private IEnumerable<string> _mashupAssemblies;

        public MashupConfiguration(MashupElement root, IEnumerable<string> mashupAssemblies)
            : this(new MashupElement[] { root }, mashupAssemblies)
        {
        }
        public MashupConfiguration(IEnumerable<MashupElement> roots, IEnumerable<string> mashupAssemblies)
            : this(roots, new Dictionary<string, string>(), mashupAssemblies)
        {
        }

        public MashupConfiguration(IEnumerable<MashupElement> roots, IDictionary<string, string> parameters, IEnumerable<string> mashupAssemblies)
        {
            this.roots = roots;
            this.parameters = parameters;
            this._mashupAssemblies = mashupAssemblies;
        }

        public MashupContainer Build()
        {
            Debug.WriteLine("Building mashup for configuration: " + this.ToString(), "MashupConfiguration");
            MashupBuilder builder = new MashupBuilder(roots, _mashupAssemblies);
            return builder.Build(parameters);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("MashupConfiguration { ");
            sb.Append(" roots: [");
            foreach (MashupElement root in Roots)
            {
                sb.Append(root.Stereotype);
                sb.Append(", ");
            }
            sb.Append(" ], ");

            sb.Append("parameters: [ ");
            foreach (string key in Parameters.Keys)
            {
                sb.Append("{");
                sb.Append(key);
                sb.Append(" : ");
                sb.Append(Parameters[key]);
                sb.Append("}, ");
            }
            sb.Append(" ] ");
            sb.Append(" } ");

            return sb.ToString();
            
        }
    }
}
