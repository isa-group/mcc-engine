using System;
using System.Collections.Generic;

namespace isa.MCC.Controls
{
    public class ProcessInstanceContext : GenericContext
    {

        private readonly ICollection<string> _instances;

        public ProcessInstanceContext(ICollection<string> instances, IDictionary<string, object> context) : base(context)
        {
            _instances = new List<string>(instances);
            _Context.Add("instances", _instances);
        }

        public ICollection<string> Instances => _instances;
    }
}
