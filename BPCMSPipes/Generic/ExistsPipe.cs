using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using de.ahzf.Styx;

namespace isa.BPCMSPipes
{
    // comprueba si el elemento que se le pasa está vacio o contiene N elementos
    public class ExistsPipe<S> : AbstractPipe<S, bool>
    {
        ulong CompareNumber;
        bool next;

        public ExistsPipe(ulong count)
        {
            this.CompareNumber = count;
            _CurrentElement = false;
            next = true;
        }

        public ExistsPipe(string count) : this(ulong.Parse(count))
        {            
        }

        public override Boolean MoveNext()
        {
            _CurrentElement = false;
            ulong count = 0;

            if (!next)
                return false;
            else
                next = false;

            if (_InternalEnumerator != null)
            {
                while (_InternalEnumerator.MoveNext() && count <= this.CompareNumber)
                {
                    count++;
                }

                _CurrentElement = (count >= this.CompareNumber);

            }

            return true;

            // if SimpleCountPipe != compareNumber then false; else true
            //IPipe<S, UInt64> countPipe = new SimpleCountPipe<S>();
            //countPipe.SetSource(_InternalEnumerator);

            //if (countPipe.MoveNext())
            //{
            //    if (countPipe.Current == this.CompareNumber)
            //    {
            //        _CurrentElement = true;
            //    }
            //    else
            //    {
            //        _CurrentElement = false;
            //    }

            //    return true;
            //}
            //else 
            //    return false;

        } 
    }
}
