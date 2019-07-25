using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace de.ahzf.Styx
{
    public interface ISplitPipe: IPipe
    {
        void SetOutputPipes(IEnumerable<IPipe> Pipes);

    }

    public interface ISplitPipe<S, E> : IPipe<S, E>, ISplitPipe
    {
    }
}
