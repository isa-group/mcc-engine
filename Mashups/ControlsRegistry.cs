using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa.Mashups;
using Mashups.ControlsExcel;

namespace Mashups
{
    public class ControlsRegistry
    {

        private static ControlsRegistry defaultInstance = null;

        public static ControlsRegistry Default
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = new ControlsRegistry();

                return defaultInstance;
            }
        }

        private List<ControlInformation> multiControls;
        private List<ControlInformation> simpleControls;

        public ControlsRegistry()
        {
            ControlsService cs = new ControlsService();
            IEnumerable<ControlInformation> controls = cs.FindAll();
            multiControls = new List<ControlInformation>();
            simpleControls = new List<ControlInformation>();

            foreach (ControlInformation c in controls)
            {
                if (c.Evidencias.Any(ev => ev.EvidenciaDescription.PorProyecto))
                {
                    multiControls.Add(c);
                }
                else
                {
                    simpleControls.Add(c);
                }
            }

        }

        public List<IControlProject> GetExecutionControlsProject()
        {
            List<IControlProject> result = new List<IControlProject>();

            foreach (ControlInformation c in multiControls)
            {
                IControlProject cp;

                if (c.Evidencias.Any(info => info.EvidenciaDescription.Tipo.Equals("Documento", StringComparison.CurrentCultureIgnoreCase)))
                    cp = new ControlProject(c, CreateRuntimeMashupConfiguration(c.Evidencias));
                else if (c.Evidencias.Any(info => info.EvidenciaDescription.Tipo.Equals("Herramienta", StringComparison.CurrentCultureIgnoreCase)))
                    cp = new ToolControlProject(c, CreateRuntimeMashupConfiguration(c.Evidencias));
                else
                    cp = new BooleanControlProject(c, CreateRuntimeMashupConfiguration(c.Evidencias));

                result.Add(cp);
            }

            return result;
        }

        public List<IControlProject> GetDesignControlsProject()
        {
            return new List<IControlProject>();
        }

        public List<IControlProcess> GetExecutionControlsProcess()
        {
            List<IControlProcess> result = new List<IControlProcess>();

            foreach (ControlInformation c in simpleControls)
            {
                result.Add(new ControlProcess(c, CreateRuntimeMashupConfiguration(c.Evidencias)));
            }

            return result;
        }

        public List<IControlProcess> GetDesignControlsProcess()
        {
            List<IControlProcess> result = new List<IControlProcess>();
            Context designContext = new Context("Global", "Global", "Global", "Oficina del CIO", "Procesos y Procedimientos");

            foreach (ControlInformation c in simpleControls)
            {
                ControlInformation ci = new ControlInformation();
                ci.ControlDescription = c.ControlDescription;
                ci.Context = designContext;
                ci.Evidencias.AddRange(c.Evidencias);

                result.Add(new ControlProcess(ci, CreateDesignMashupConfiguration(c.Evidencias)));
            }
            foreach (ControlInformation c in multiControls)
            {
                ControlInformation ci = new ControlInformation();
                ci.ControlDescription = c.ControlDescription;
                ci.Context = designContext;
                ci.Evidencias.AddRange(c.Evidencias);
                result.Add(new ControlProcess(ci, CreateDesignMashupConfiguration(c.Evidencias)));
            }

            return result;
        }

        private List<MashupConfiguration> CreateDesignMashupConfiguration(List<EvidenciaInformation> evidencias)
        {
            return CreateMashupConfiguration(evidencias, ev => ev.DesignMashupDescription);
        }

        private List<MashupConfiguration> CreateRuntimeMashupConfiguration(List<EvidenciaInformation> evidencias)
        {
            return CreateMashupConfiguration(evidencias, ev => ev.RuntimeMashupDescription);
        }

        private List<MashupConfiguration> CreateMashupConfiguration(List<EvidenciaInformation> evidencias, Func<EvidenciaInformation, MashupDescription> getter)
        {
            List<MashupConfiguration> configs = new List<MashupConfiguration>();
            foreach (EvidenciaInformation ev in evidencias)
            {
                MashupDescription md = getter(ev);
                MashupConfiguration mc = md.GetMashupConfiguration();
                if (mc != null)
                    configs.Add(mc);
            }
            return configs;
        }         

    }
}
