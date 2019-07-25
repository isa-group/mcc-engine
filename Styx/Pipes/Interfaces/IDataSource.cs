using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;

namespace de.ahzf.Styx
{
    public interface IDataSource : IEndPipe
    {
    }

    public interface IDataSource<S> : IEndPipe<S>, IDataSource
    {
    }
}
