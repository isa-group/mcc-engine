using System.Collections.Generic;

namespace isa.MCC.Mashups
{
    /// <summary>
    /// Interface that each mashup repository should implement. They must be able to provide
    /// the MashupElements that correspond with a specific name. It is used to
    /// instantiate the Mashup from its call.
    /// </summary>
    public interface IMashupRepository
    {
        IEnumerable<MashupElement> FindMashup(string name);
        IEnumerable<string> MashupAssemblies();
    }
}
