using System;
using System.Collections.Generic;
using de.ahzf.Styx;

namespace isa.MCC.Pipes.Generic
{
    public interface INamedMergePipe : IMergePipe
    {
        void SetInputPipes(IDictionary<string, IPipe> Pipes);
    }

    public interface INamedMergePipe<S, E> : IMergePipe<S,E>
    {
    }

}
