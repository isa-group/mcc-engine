using de.ahzf.Styx;

namespace isa.MCC.Pipes.Generic
{
    class Result<S> : FuncFilterPipe<S>
    {
        public Result()
            : base(delegate(S p) { return false; })
        {
        }
    }
}
