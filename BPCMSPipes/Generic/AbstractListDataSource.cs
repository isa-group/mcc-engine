using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;

namespace icinetic.BPCMSPipes
{
    public abstract class AbstractListDataSource<S> : IDataSource<S>
    {
        public AbstractListDataSource()
        {
            sourceList = new List<S>();
            SetSourceList(new List<S>());
        }

        public AbstractListDataSource(IEnumerable<S> p)
        {
            sourceList = new List<S>();
            SetSourceList(new List<S>(p));
        }

        public AbstractListDataSource(S elem): this(new S[] { elem })
        {
        }

        private List<S> sourceList;
        private IEnumerator<S> enumerator;

        public void SetSourceList(IEnumerable<S> p)
        {
            sourceList.Clear();
            sourceList.AddRange(p);
            enumerator = sourceList.GetEnumerator();
        }
        

        #region IEndPipe Members

        List<object> IEndPipe.Path
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { return enumerator.Current; }
        }

        bool System.Collections.IEnumerator.MoveNext()
        {
            return enumerator.MoveNext();
        }

        void System.Collections.IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            enumerator.Dispose();
        }

        #endregion

        #region IEnumerator<S> Members

        S IEnumerator<S>.Current
        {
            get { return enumerator.Current; }
        }

        #endregion

        #region IEnumerable<S> Members

        IEnumerator<S> IEnumerable<S>.GetEnumerator()
        {
            return this;
        }

        #endregion
    }
}
