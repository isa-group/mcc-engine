using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa.Mashups;

namespace Mashups
{
    class ToolControlProject : IControlProject
    {
        private IEnumerable<MashupConfiguration> mashups;

        public ControlInformation Information { get { return info; } }
        private ControlInformation info;

        public ToolControlProject(ControlInformation info, IEnumerable<MashupConfiguration> mashups)
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
            List<ControlProjectEvaluation> result = new List<ControlProjectEvaluation>();
            return result;
        }


        #endregion
    }
}
