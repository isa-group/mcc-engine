using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa;


namespace isa.Mashups
{
    public interface IControl<S>
    {
        string Name { get; }
        string Description { get; }
        ControlInformation Information { get; }

        S Evaluate();
    }
}
