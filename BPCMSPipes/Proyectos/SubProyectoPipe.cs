using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;
using icinetic.BPMMetamodel;
using icinetic.EAService;
using icinetic.CatalogoProyectos;

namespace icinetic.BPCMSPipes
{
    public class SubProyectoPipe : AbstractPipe<Proyecto, SubProyecto>
    {
        IEnumerator<SubProyecto> tmpEnumerator = null;
        SubProyectoSpecification specification = null;

        public SubProyectoPipe(SubProyectoSpecification specification)
        {
            this.specification = specification;
        }

        public SubProyectoPipe()
            : this(new SubProyectoSpecification())
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
            else 
            {
                while (_InternalEnumerator.MoveNext())
                {
                    LoadIterator();
                    if (tmpEnumerator.MoveNext())
                    {
                        _CurrentElement = tmpEnumerator.Current;
                        return true;
                    }
                }

            }

            return false;
        }

        private void LoadIterator()
        {
            Proyecto proyecto = _InternalEnumerator.Current;
            tmpEnumerator = proyecto.SubProyectoList.Where(specification.SatisfiedBy().Compile()).GetEnumerator();
        }

        #endregion
    }
}
