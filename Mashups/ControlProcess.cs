using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa.Mashups;
using de.ahzf.Styx;

namespace Mashups
{
    class ControlProcess : IControlProcess
    {

        private IEnumerable<MashupConfiguration> mashups;
        public ControlInformation Information { get { return info; } }
        private ControlInformation info;

        public ControlProcess(ControlInformation info, IEnumerable<MashupConfiguration> mashups)
        {
            this.mashups = mashups;
            this.info = info;
        }

        #region IControl<ControlEvaluation> Members

        public string Name
        {
            get { return info.ControlDescription.Id; }
        }

        public string Description
        {
            get { return info.ControlDescription.Description; }
        }

        public ControlEvaluation Evaluate()
        {
            bool eval = false;

            eval = mashups.All(m => InternalEvaluate(m));

            return new ControlEvaluation(info.Context, eval);
        }

        private bool InternalEvaluate(MashupConfiguration m)
        {
            bool evaluation = false;
            MashupContainer container = m.Build();
            IPipe pipe = container.Pipe;

            if (pipe.MoveNext())
            {
                if (pipe.Current is bool)
                {
                    evaluation = (bool)pipe.Current;
                    while (pipe.MoveNext())
                    {
                        evaluation = evaluation && (bool)pipe.Current;
                    }
                }
            }

            return evaluation;
        }

        #endregion
    }
}
