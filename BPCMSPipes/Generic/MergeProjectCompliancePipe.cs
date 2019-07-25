using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;
using isa.Mashups;

namespace isa.BPCMSPipes
{
    public class MergeProjectCompliancePipe<S> : AbstractPipe<S, bool, ProjectsState> 
    {
        private Func<S, string> Conversor;
        public MergeProjectCompliancePipe(Func<S,string> Conversor)
        {
            this.Conversor = Conversor;
        }

        public override Boolean MoveNext()
        {

            if (_InternalEnumerator1 == null || _InternalEnumerator2 == null)
            {
                return false;
            }

            if (_InternalEnumerator1.MoveNext())
            {
                S p = _InternalEnumerator1.Current;

                if (_InternalEnumerator2.MoveNext())
                {
                    _CurrentElement = new ProjectsState(Conversor(p), _InternalEnumerator2.Current);
                }
                else
                {
                    _CurrentElement = new ProjectsState(Conversor(p), false);
                }

                return true;
            }

            return false;

        } 


    }
}
