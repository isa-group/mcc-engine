using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using icinetic.EAService;
using System.Diagnostics;

namespace Mashups.ControlsExcel
{
    public class MashupDescription
    {
        public string Name { get{ return _name;}}
        private string _name;

        public IDictionary<string, string> Parameters { get { return _parameters; } }
        private IDictionary<string, string> _parameters;

        public static MashupDescription CreateMashupDescription(string mashupCall, IDictionary<string, string> standardParameters)
        {
            string name;
            IDictionary<string, string> parameters = new Dictionary<string, string>();

            string[] tokens = mashupCall.Split('[', ']');
            name = tokens[0];

            CopyStandardParameters(parameters, standardParameters);

            if (tokens.Length > 1)
            {
                AddParameters(parameters, tokens);
            }

            MashupDescription mashupDescription = new MashupDescription(name, parameters);
            return mashupDescription;
        }

        private static void CopyStandardParameters(IDictionary<string, string> parameters, IDictionary<string, string> standardParameters)
        {
            foreach (string key in standardParameters.Keys)
            {
                if (!parameters.ContainsKey(key))
                    parameters.Add(key, standardParameters[key]);
                else
                    parameters[key] = standardParameters[key];
            }
        }

        private static void AddParameters(IDictionary<string, string> parameters, string[] tokens)
        {
            string[] paramTokens = tokens[1].Split(',');
            foreach (string p in paramTokens)
            {
                string[] param = p.Split('=');
                if (param.Length == 2)
                {
                    string key = param[0].Trim();
                    string value = param[1].Trim();
                    if (!parameters.ContainsKey(key))
                    {
                        parameters.Add(key, value);
                    }
                    else
                    {
                        parameters[key] = value;
                    }
                }
            }
        }


        public MashupDescription(string name, IDictionary<string,string> parameters)
        {
            _name = name;
            _parameters = parameters;
        }

        public MashupConfiguration GetMashupConfiguration()
        {
            BPMService service = new BPMService();
            List<icinetic.BPMMetamodel.Mashup> candidates = service.Root.MashupList.Where(m => m.Name.Equals(_name, StringComparison.CurrentCultureIgnoreCase)).ToList();

            if (candidates.Count == 0)
                return null;

            MashupConfiguration mc = new MashupConfiguration(candidates[0].Roots, _parameters);

            Debug.WriteLine("Created MashupConfiguration: " + mc.ToString(), "MashupDescription");

            return mc;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("MashupDescription { name: ");
            sb.Append(Name);
            sb.Append(", parameters: [ ");
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
