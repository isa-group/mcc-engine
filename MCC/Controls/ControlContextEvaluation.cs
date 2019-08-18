namespace isa.MCC.Controls
{
    public class ControlContextEvaluation
    {
        public readonly IContext Context;
        public bool Evaluation;

        public ControlContextEvaluation(IContext ctx, bool eval)
        {
            Context = ctx;
            Evaluation = eval;
        }

        public ControlContextEvaluation(IContext ctx)
        {
            Context = ctx;
        }

        public override string ToString()
        {
            return string.Format("{0} in context {1}", Evaluation, Context);            
        }
    }
}
