using System.Collections.Generic;

namespace isa.MCC.Controls
{
    /// <summary>
    /// Class that contains the information of a control
    /// </summary>
    public class ControlInformation
    {
        private IContext context;
        public IContext Context
        {
            get { return context; }
            set { context = value; }
        }
               
        public List<RuleInformation> Rules { get { return _rules; } }
        private List<RuleInformation> _rules;

        public ControlInformation() 
        {
            _rules = new List<RuleInformation>();
        }
    }
}
