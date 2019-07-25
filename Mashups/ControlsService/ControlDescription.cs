using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace isa.Mashups
{
    public class ControlDescription
    {
        string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public ControlDescription(string id, string descr) {
            this.id = id;
            this.description = descr;
        }
    }
}
