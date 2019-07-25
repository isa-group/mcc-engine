using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;

namespace icinetic.BPCMSPipes.Generic
{
    class ResultPipe<S> : FuncFilterPipe<S>
    {
        public ResultPipe()
            : base(delegate(S p) { return false; })
        {
        }
    }
}
