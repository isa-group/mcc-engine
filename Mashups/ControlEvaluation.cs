using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace isa.Mashups
{
    public class ControlEvaluation
    {
        public readonly Context Context;
        public bool Evaluation;

        public ControlEvaluation(Context ctx, bool eval)
        {
            Context = ctx;
            Evaluation = eval;
        }

        public ControlEvaluation(Context ctx)
        {
            Context = ctx;
        }
    }
}
