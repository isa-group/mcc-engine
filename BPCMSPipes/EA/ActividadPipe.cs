﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;
using icinetic.BPMMetamodel;
using icinetic.EAService;


namespace icinetic.BPCMSPipes
{
    public class ActividadPipe : AbstractPipe<Subproceso, Actividad>
    {
        IEnumerator<Actividad> tmpEnumerator = null;
        ActividadSpecification specification = null;

        public ActividadPipe(ActividadSpecification specification)
        {
            this.specification = specification;
        }

        public ActividadPipe()
            : this(new ActividadSpecification())
        {
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

            if (tmpEnumerator != null && tmpEnumerator.MoveNext())
            {
                _CurrentElement = tmpEnumerator.Current;
                return true;
            }
            else if (_InternalEnumerator.MoveNext())
            {
                LoadIterator();

                if (tmpEnumerator.MoveNext())
                {
                    _CurrentElement = tmpEnumerator.Current;
                    return true;
                }
            }

            return false;
        }

        private void LoadIterator()
        {
            Subproceso subproceso = _InternalEnumerator.Current;
            tmpEnumerator = subproceso.ActividadesList.Where(specification.SatisfiedBy().Compile()).GetEnumerator();
        }

        #endregion
    }
}
