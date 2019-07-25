﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;
using icinetic.BPMMetamodel;
using icinetic.EAService;
using System.Configuration;

namespace icinetic.BPCMSPipes
{
    public class SubprocesoEnEADataSource : AbstractPipe<Root, Subproceso>, IDataSource<Subproceso>
    {
        IEnumerator<Subproceso> tmpEnumerator = null;
        SubprocesoSpecification specification = null;

        public static BPMService GetEADataSource()
        {
            // Data source            
            //string configPath = ConfigurationManager.AppSettings["TmpPath"].ToString();
            //string configPath = icinetic.BPCMSPipes.Properties.Settings.Default.TmpPath;
            //string pathEA = System.IO.Path.Combine(configPath, "procede.eap");
            BPMService serviceEA = new BPMService();
            icinetic.BPMMetamodel.Root r = serviceEA.Root;
            return serviceEA;
        }


        public SubprocesoEnEADataSource(string macroproceso, string proceso, string subproceso) : base(new List<Root>() { GetEADataSource().Root }, null)
        {
            this.specification = new SubprocesoSpecification();
            specification.Nombre = ProcedeConversor.ToLongName(subproceso);            

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
            Root root = _InternalEnumerator.Current;
            tmpEnumerator = root.SubprocesoList.Where(specification.SatisfiedBy().Compile()).GetEnumerator();
        }

        #endregion
    }
}