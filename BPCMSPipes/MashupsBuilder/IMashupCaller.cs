using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mashups
{
    public interface IMashupCaller
    {
        void SetParameters(IDictionary<string, string> parameters);
    }
}
