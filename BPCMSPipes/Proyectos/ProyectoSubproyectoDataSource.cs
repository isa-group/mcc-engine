using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;
using icinetic.CatalogoProyectos;
using System.Diagnostics;

namespace icinetic.BPCMSPipes.Proyectos
{
    public class ProyectoSubproyectoDataSource : AbstractPipe<Proyecto, ProyectoSubproyecto>, IDataSource<ProyectoSubproyecto>
    {
        IEnumerator<SubProyecto> tmpEnumerator = null;

        public ProyectoSubproyectoDataSource() : base()
        {
            ProyectoService projectService = new ProyectoService();
            List<Proyecto> projects = projectService.FindAll();

            this.SetSourceCollection(projects);
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
                _CurrentElement = new ProyectoSubproyecto(tmpEnumerator.Current);
                Debug.WriteLine(string.Format("Current project is {0}", _CurrentElement.Titulo), "ProyectoSubproyectoDataSource");
                return true;
            }
            else if (_InternalEnumerator.MoveNext())
            {
                Proyecto p = _InternalEnumerator.Current;
                if (p.SubProyectoList.Count > 0)
                {
                    LoadIterator();
                    if (tmpEnumerator.MoveNext())
                    {
                        _CurrentElement = new ProyectoSubproyecto(tmpEnumerator.Current);
                        Debug.WriteLine(string.Format("Current project is {0}", _CurrentElement.Titulo), "ProyectoSubproyectoDataSource");
                        return true;
                    }
                }
                else
                {
                    tmpEnumerator = null;
                    _CurrentElement = new ProyectoSubproyecto(p);
                    Debug.WriteLine(string.Format("Current project is {0}", _CurrentElement.Titulo), "ProyectoSubproyectoDataSource");
                    return true;
                }

            }

            return false;
        }

        private void LoadIterator()
        {
            List<SubProyecto> subproyectos = _InternalEnumerator.Current.SubProyectoList;
            tmpEnumerator = subproyectos.GetEnumerator();
        }

        #endregion
    }
}
