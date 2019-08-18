using System;
using System.Collections.Generic;

namespace isa.MCC.Controls
{
    public class GenericContext : IContext
    {
        protected IDictionary<string, object> _Context;

        public GenericContext(IDictionary<string, object> context)
        {
            _Context = new Dictionary<string, object>(context);
        }
        
        public IDictionary<string, object> ContextInfo { get => _Context; }

        public IContext Merge(IContext c)
        {
            GenericContext mergedContext = new GenericContext(_Context);

            foreach(KeyValuePair<string, object> pair in c.ContextInfo)
            {
                if (mergedContext._Context.ContainsKey(pair.Key))
                {
                    mergedContext._Context[pair.Key] = pair.Value;
                }
                else
                {
                    mergedContext._Context.Add(pair);
                }
            }

            return mergedContext;
        }

        public bool ParentOf(IContext c)
        {
            throw new NotImplementedException();
        }
    }
}
