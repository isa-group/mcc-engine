using System;
using System.Collections.Generic;
using System.Linq;
using de.ahzf.Styx;
using isa.MCC.Controls;

namespace isa.MCC.Pipes.Generic
{
    public class MergeResultWithContext : AbstractNamedMergePipe<object, ResultWithContext>
    {
        public MergeResultWithContext() : base()
        {
        }

        public override bool MoveNext()
        {
            if (_NamedInternalEnumerators.Values.Any(p => p == null))
            {
                return false;
            }

            IEnumerable<bool> resultMoving = _NamedInternalEnumerators.Values.MapEach(p => p.MoveNext());

            if (resultMoving.All(result => result))
            {
                IDictionary<string, object> currentElements = new Dictionary<string, object>();
                _NamedInternalEnumerators.ForEach(p => currentElements.Add(p.Key, p.Value.Current));

                object result = null;
                if (currentElements.ContainsKey("result"))
                {
                    result = currentElements["result"];
                    currentElements.Remove("result");
                }

                IContext context = new GenericContext(currentElements);

                _CurrentElement = new ResultWithContext(result, context);

                return true;
            }

            return false;
        }
    }

    public class ResultWithContext
    {
        public object Result { get; }
        public IContext Context { get; }

        public ResultWithContext(object result, IContext context)
        {
            Result = result;
            Context = context;
        }
    }
}
