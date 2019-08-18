using System;
using System.Collections.Generic;
using de.ahzf.Styx;

namespace isa.MCC.Pipes.Generic
{
    public abstract class AbstractNamedMergePipe<S, E> : AbstractMergePipe<S, E>, INamedMergePipe<S, E>
    {
        protected IDictionary<string, IEnumerator<S>> _NamedInternalEnumerators;

        public AbstractNamedMergePipe()
        {
            _NamedInternalEnumerators = new Dictionary<string, IEnumerator<S>>();
            UpdateParentWithDict();
        }

        public void SetInputPipes(IDictionary<string, IPipe> Pipes)
        {
            _NamedInternalEnumerators.Clear();
            Pipes.ForEach(p => _NamedInternalEnumerators.Add(p.Key, (IEnumerator<S>)p.Value));
            UpdateParentWithDict();
        }

        public new void SetInputPipes(IEnumerable<IPipe> Pipes)
        {
            _NamedInternalEnumerators.Clear();
            int i = 1;
            foreach (IPipe pipe in Pipes) {
                _NamedInternalEnumerators.Add("input" + i, (IEnumerator<S>)pipe);
                i++;
            }
            UpdateParentWithDict();
        }

        /// <summary>
        /// Set the given element as source.
        /// </summary>
        /// <param name="SourceElement">A single source element.</param>
        public override void SetSource(S SourceElement)
        {
            _NamedInternalEnumerators.Clear();
            _NamedInternalEnumerators.Add("input", new HistoryEnumerator<S>(new List<S>() { SourceElement }.GetEnumerator()));
            UpdateParentWithDict();
        }

        /// <summary>
        /// Set the elements emitted by the given IEnumerator&lt;S1&gt; as input.
        /// </summary>
        /// <param name="IEnumerator">An IEnumerator&lt;S1&gt; as element source.</param>
        public override void SetSource(IEnumerator<S> IEnumerator)
        {

            if (IEnumerator == null)
                throw new ArgumentNullException("IEnumerator must not be null!");

            _NamedInternalEnumerators.Clear();
            if (IEnumerator is IEndPipe<S>)
                _NamedInternalEnumerators.Add("input", IEnumerator);
            else
                _NamedInternalEnumerators.Add("input", new HistoryEnumerator<S>(IEnumerator));

            UpdateParentWithDict();

        }

        private void UpdateParentWithDict()
        {
            _InternalEnumerators = new List<IEnumerator<S>>(_NamedInternalEnumerators.Values);
        }


    }
}
