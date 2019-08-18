using System.Collections.Generic;

namespace isa.MCC.Mashups
{
    public interface IMashupCaller
    {
        void SetParameters(IDictionary<string, string> parameters);
    }
}
