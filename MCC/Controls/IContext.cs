using System.Collections.Generic;

namespace isa.MCC.Controls
{
    public interface IContext
    {
        IDictionary<string, object> ContextInfo { get; }
        bool ParentOf(IContext c);
        IContext Merge(IContext c);
    }
}