using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using de.ahzf.Styx;
using Mashups;
using Mashups.ControlsExcel;


namespace icinetic.BPCMSPipes.Proyectos
{
    public class ProyectoSubproyectoEval
    {
        public ProyectoSubproyecto Proyecto
        {
            get { return _proyecto; }
        }
        private ProyectoSubproyecto _proyecto;

        private bool _eval;
        public bool Evaluation
        {
            get { return _eval; }
            set { _eval = value; }
        }

        public ProyectoSubproyectoEval(ProyectoSubproyecto ps, bool eval)
        {
            _proyecto = ps;
            _eval = eval;
        }
    }

    public class ForEachProject : AbstractPipe<ProyectoSubproyecto, ProyectoSubproyectoEval>, IMashupCaller
    {
        private string _mashup;
        private IDictionary<string, string> _parameters;
        private MashupConfiguration mashupConfiguration;

        public ForEachProject(string mashup)
        {
            _mashup = mashup;
        }

        public void SetParameters(IDictionary<string, string> parameters)
        {
            _parameters = parameters;
            MashupDescription md = MashupDescription.CreateMashupDescription(_mashup, _parameters);
            ProcessParameters(md);
            mashupConfiguration = md.GetMashupConfiguration();
        }

        private void ProcessParameters(MashupDescription md)
        {
            Dictionary<string,string> paramsToModify = new Dictionary<string,string>();

            foreach (string key in md.Parameters.Keys)
            {
                string value = md.Parameters[key];

                if (value.StartsWith("@Param"))
                {
                    string[] splitted = value.Split('(', ')');
                    string paramName = splitted[1].Trim();
                    if (_parameters.ContainsKey(paramName))
                    {
                        paramsToModify.Add(key, paramName);
                        //md.Parameters[key] = _parameters[paramName];
                    }
                    else
                    {
                        throw new ArgumentException("Not parameter provided for " + key);
                    }

                }

            }

            foreach (string key in paramsToModify.Keys)
            {
                md.Parameters[key] = _parameters[paramsToModify[key]];
            }
        }

        private void SetProjectListInDataSource(ProyectoSubproyecto ps, MashupContainer container)
        {
            foreach (IDataSource dataSource in container.DataSources)
            {
                EachProjectDataSource ds = dataSource as EachProjectDataSource;
                if (ds != null)
                    ds.SetSourceList(new ProyectoSubproyecto[] { ps });                
            }
        }


        public override Boolean MoveNext()
        {

            if (_InternalEnumerator == null)
            {
                return false;
            }

            if (_InternalEnumerator.MoveNext())
            {
                ProyectoSubproyecto p = _InternalEnumerator.Current;
                _CurrentElement = new ProyectoSubproyectoEval(p, false);

                MashupContainer container = mashupConfiguration.Build();
                SetProjectListInDataSource(p, container);
                IPipe pipe = container.Pipe;

                if (pipe != null && pipe.MoveNext())
                {
                    if (pipe.Current is bool)
                    {
                        _CurrentElement.Evaluation = (bool)pipe.Current;
                        while (pipe.MoveNext())
                        {
                            _CurrentElement.Evaluation = _CurrentElement.Evaluation && (bool)pipe.Current;
                        }
                    }
                }

                return true;
            }

            return false;

        } 

    }
}
