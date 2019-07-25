using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using icinetic.EAService;

namespace Mashups.ControlsExcel
{
    public class MashupDescription
    {
        public string Name { get{ return _name;}}
        private string _name;

        public IDictionary<string, string> Parameters { get { return _parameters; } }
        private IDictionary<string, string> _parameters;

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
                        
            return new MashupConfiguration(candidates[0].Roots, _parameters);
        }

    }
}
