using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using de.ahzf.Styx;

namespace isa.BPCMSPipes
{
    // devuelve true si todas las entradas son true; false en caso contrario
    public class AndParameterPipe : AbstractPipe<Boolean, Boolean>
    {
        private int numElements;
        
        public AndParameterPipe(int numElem)
        {
            this.numElements = numElem;
            _CurrentElement = true;
        }
   
        public override Boolean MoveNext()
        {

            if (_InternalEnumerator == null)
            {
                _CurrentElement = false;
                return false;
            }

            while (_InternalEnumerator.MoveNext())
            {
                if (!_InternalEnumerator.Current)
                {
                    _CurrentElement = false;
                    return false;
                }
            }

            return true;

        } 
    }
}
