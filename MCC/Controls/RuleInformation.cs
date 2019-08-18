using System;
using System.Collections.Generic;
using isa.MCC.Mashups;

namespace isa.MCC.Controls
{
    public class RuleInformation
    {
        public IDictionary<string, object> Metadata { get; }

        public MashupDescription RuntimeMashupDescription { get; }

        public MashupDescription DesignMashupDescription { get; }

        public RuleInformation(IDictionary<string, object> metadata, MashupDescription design, MashupDescription runtime)
        {
            this.Metadata = new Dictionary<string, object>(metadata);
            this.DesignMashupDescription = design;
            this.RuntimeMashupDescription = runtime;
        }
    }
}
