using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;

namespace icinetic.BPCMSPipes.Generic
{
    class AndPipe : AbstractMergePipe<bool, bool>
    {
        public AndPipe()
            : base()
        {
        }

        public override Boolean MoveNext()
        {
            if (_InternalEnumerators.Any(p => p == null))
            {
                return false;
            }

            IEnumerable<bool> resultMoving = _InternalEnumerators.MapEach(p => p.MoveNext());

            if (resultMoving.All(result => result))
            {
                _CurrentElement = _InternalEnumerators.All(p => p.Current);
                return true;
            }
            else if (resultMoving.Any(result => result))
            {
                _CurrentElement = false;
                return true;
            }

            return false;

        } 


    }
}
