using System;
using System.Collections.Generic;
using de.ahzf.Styx;
using System.Collections;

namespace isa.MCC.Pipes.Generic
{
    #region AbstractPipe<S1, S2, S3, E>

    /// <summary>
    /// An AbstractMergePipe provides most of the functionality that is repeated
    /// in every instance of a IMergePipe. Any subclass of AbstractMergePipe should simply
    /// implement MoveNext().
    /// </summary>
    /// <typeparam name="S">The type of the consuming objects.</typeparam>
    /// <typeparam name="E">The type of the emitting objects.</typeparam>
    public abstract class AbstractMergePipe<S, E> : IPipe<S, E>, IMergePipe<S, E>
    {

        #region Data

        /// <summary>
        /// The internal enumerator of the collections.
        /// </summary>
        protected List<IEnumerator<S>> _InternalEnumerators;

        /// <summary>
        /// The internal current element in the collection.
        /// </summary>
        protected E _CurrentElement;

        #endregion

        #region Constructor(s)

        #region AbstractPipe()

        /// <summary>
        /// Creates a new abstract pipe.
        /// </summary>
        public AbstractMergePipe()
        {
            _InternalEnumerators = new List<IEnumerator<S>>();
        }

        #endregion

        #endregion

        # region SetInputPipes

        public void SetInputPipes(IEnumerable<IPipe> Pipes)
        {
            _InternalEnumerators.Clear();
            Pipes.ForEach(p => _InternalEnumerators.Add((IEnumerator<S>)p));
        }

        # endregion

        #region SetSource(SourceElement)

        /// <summary>
        /// Set the given element as source.
        /// </summary>
        /// <param name="SourceElement">A single source element.</param>
        public virtual void SetSource(Object SourceElement)
        {
            SetSource((S)SourceElement);
        }


        /// <summary>
        /// Set the given element as source.
        /// </summary>
        /// <param name="SourceElement">A single source element.</param>
        public virtual void SetSource(S SourceElement)
        {
            _InternalEnumerators.Clear();
            _InternalEnumerators.Add(new HistoryEnumerator<S>(new List<S>() { SourceElement }.GetEnumerator()));
        }

        #endregion


        #region SetSource(IEnumerator)

        /// <summary>
        /// Set the elements emitted by the given IEnumerator as input.
        /// </summary>
        /// <param name="IEnumerator">An IEnumerator as element source.</param>
        public virtual void SetSource(IEnumerator IEnumerator)
        {
            SetSource((IEnumerator<S>)IEnumerator);
        }


        /// <summary>
        /// Set the elements emitted by the given IEnumerator&lt;S1&gt; as input.
        /// </summary>
        /// <param name="IEnumerator">An IEnumerator&lt;S1&gt; as element source.</param>
        public virtual void SetSource(IEnumerator<S> IEnumerator)
        {

            if (IEnumerator == null)
                throw new ArgumentNullException("IEnumerator must not be null!");

            _InternalEnumerators.Clear();
            if (IEnumerator is IEndPipe<S>)
                _InternalEnumerators.Add(IEnumerator);
            else
                _InternalEnumerators.Add(new HistoryEnumerator<S>(IEnumerator));

        }

        #endregion


        #region SetSourceCollection(IEnumerable)

        /// <summary>
        /// Set the elements emitted from the given IEnumerable as input.
        /// </summary>
        /// <param name="IEnumerable">An IEnumerable as element source.</param>
        public virtual void SetSourceCollection(IEnumerable IEnumerable)
        {
            SetSourceCollection((IEnumerable<S>)IEnumerable);
        }


        /// <summary>
        /// Set the elements emitted from the given IEnumerable&lt;S1&gt; as input.
        /// </summary>
        /// <param name="IEnumerable">An IEnumerable&lt;S1&gt; as element source.</param>
        public virtual void SetSourceCollection(IEnumerable<S> IEnumerable)
        {

            if (IEnumerable == null)
                throw new ArgumentNullException("IEnumerable must not be null!");

            SetSource(IEnumerable.GetEnumerator());

        }

        #endregion


        #region GetEnumerator()

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A IEnumerator&lt;E&gt; that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<E> GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A IEnumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }

        #endregion

        #region Current

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        public E Current
        {
            get
            {
                return _CurrentElement;
            }
        }

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        Object System.Collections.IEnumerator.Current
        {
            get
            {
                return _CurrentElement;
            }
        }

        #endregion

        #region MoveNext()

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// True if the enumerator was successfully advanced to the next
        /// element; false if the enumerator has passed the end of the
        /// collection.
        /// </returns>
        public abstract Boolean MoveNext();

        #endregion

        #region Reset()

        /// <summary>
        /// Sets the enumerators to their initial positions, which
        /// is before the first element in the collections.
        /// </summary>
        public virtual void Reset()
        {
            _InternalEnumerators.ForEach(p => p.Reset());
        }

        #endregion

        #region Dispose()

        /// <summary>
        /// Disposes this pipe.
        /// </summary>
        public virtual void Dispose()
        {
            _InternalEnumerators.ForEach(p => p.Dispose());
        }

        #endregion


        #region Path

        /// <summary>
        /// Returns the transformation path to arrive at the current object
        /// of the pipe. This is a list of all of the objects traversed for
        /// the current iterator position of the pipe.
        /// </summary>
        public virtual List<Object> Path
        {

            get
            {

                var _PathElements = PathToHere;
                var _Size = _PathElements.Count;

                // do not repeat filters as they dup the object
                // todo: why is size == 0 required (Pangloss?)	        
                if (_Size == 0 || !_PathElements[_Size - 1].Equals(_CurrentElement))
                    _PathElements.Add(_CurrentElement);

                return _PathElements;

            }

        }

        #endregion

        #region PathToHere

        private List<Object> PathToHere
        {

            get
            {

                throw new NotImplementedException();

                //if (_InternalEnumerator is IPipe)
                //    return ((IPipe) _InternalEnumerator).Path;

                //else if (_InternalEnumerator is IHistoryEnumerator)
                //{

                //    var _List = new List<Object>();
                //    var _Last = ((IHistoryEnumerator) _InternalEnumerator).Last;

                //    if (_Last == null)
                //        _List.Add(_InternalEnumerator.Current);
                //    else
                //        _List.Add(_Last);

                //    return _List;

                //}

                //else if (_InternalEnumerator is ISingleEnumerator)
                //    return new List<Object>() { ((ISingleEnumerator) _InternalEnumerator).Current };

                //else
                //    return new List<Object>();

            }

        }

        #endregion


        #region ToString()

        /// <summary>
        /// A string representation of this pipe.
        /// </summary>
        public override String ToString()
        {
            return this.GetType().Name;
        }

        #endregion

    }

    #endregion

}
