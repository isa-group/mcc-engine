using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace de.ahzf.Styx
{
    public interface IMergePipe : IPipe
    {
        void SetInputPipes(IEnumerable<IPipe> Pipes);
    }

    public interface IMergePipe<S, E> : IPipe<S, E>, IMergePipe
    {
    }
}
