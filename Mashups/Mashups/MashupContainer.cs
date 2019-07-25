using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;

namespace Mashups
{
    public class MashupContainer
    {
        public IPipe Pipe { get { return _pipe; } }
        private IPipe _pipe;

        public IDataSource[] DataSources { get { return _sources; } }
        private IDataSource[] _sources;

        public MashupContainer(IPipe pipe, params IDataSource[] sources)
        {
            _pipe = pipe;
            _sources = sources.ToArray();
        }
    }
}
