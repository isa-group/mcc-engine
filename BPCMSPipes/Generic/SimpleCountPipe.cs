using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;

namespace isa.BPCMSPipes
{
    public class SimpleCountPipe<S> : AbstractPipe<S, UInt64>
    {

        public SimpleCountPipe()
        {
            _CurrentElement = 0;
        }

        #region MoveNext()

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// True if the enumerator was successfully advanced to the next
        /// element; false if the enumerator has passed the end of the
        /// collection.
        /// </returns>
        public override Boolean MoveNext()
        {

            if (_InternalEnumerator == null)
                return false;

            if (_InternalEnumerator.MoveNext())
            {
                _CurrentElement++;

                while (_InternalEnumerator.MoveNext())
                {
                    _CurrentElement++;
                }

                return true;
            }

            return false;

        }

        #endregion

    }
}
