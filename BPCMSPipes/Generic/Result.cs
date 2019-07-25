using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;

namespace icinetic.BPCMSPipes.Generic
{
    class Result<S> : FuncFilterPipe<S>
    {
        public Result()
            : base(delegate(S p) { return false; })
        {
        }
    }
}
