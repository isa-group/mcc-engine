using System.Linq;
using de.ahzf.Styx;

namespace isa.MCC.Mashups
{
    /// <summary>
    /// This class represents an instantiated mashup ready to be used
    /// </summary>
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
