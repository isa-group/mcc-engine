using System.Collections.Generic;
using System.Linq;

namespace isa.MCC.Mashups
{
    public class MashupElement
    {
        public string Stereotype { get; set; }
        public IDictionary<string, string> TagNames { get; set; }
        public IList<MashupElement> Next { get; set; }
        public IList<string> NextNames { get; set; }

        public MashupElement(string stereotype)
        {
            Stereotype = stereotype;
            Next = new List<MashupElement>();
            NextNames = new List<string>();
            TagNames = new Dictionary<string, string>();
        }

        public MashupElement(string stereotype, MashupElement nextElement) : this(stereotype, nextElement, "default")
        {
        }

        public MashupElement(string stereotype, MashupElement nextElement, string nextName) : this(stereotype)
        {
            Next.Add(nextElement);
            NextNames.Add(nextName);
        }

        public MashupElement(string stereotype, IList<MashupElement> nextElements) : this(stereotype, nextElements, Enumerable.Repeat("default", nextElements.Count).ToList())
        {

        }

        public MashupElement(string stereotype, IList<MashupElement> nextElements, IList<string> nextNames) : this(stereotype)
        {
            foreach (MashupElement e in nextElements)
            {
                Next.Add(e);
            }

            foreach (string e in nextNames)
            {
                NextNames.Add(e);
            }
        }
    }

}
