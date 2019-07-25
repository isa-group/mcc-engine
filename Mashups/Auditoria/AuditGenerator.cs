using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa.Mashups;

namespace Mashups.Auditoria
{
    public class AuditGenerator
    {
        public AuditGenerator()
        {
        }

        public List<AuditControlProcess> GetAuditControlProcess(List<string> controlIds)
        {
            List<AuditControlProcess> result = new List<AuditControlProcess>();

            IEnumerable<IControlProcess> controlsProcess = ControlsRegistry.Default.GetExecutionControlsProcess().Where(c => controlIds.Contains(c.Name));
            foreach (IControlProcess p in controlsProcess)
            {
                AuditControlProcess audit = new AuditControlProcess(p.Information);

                foreach (EvidenciaInformation evInfo in p.Information.Evidencias)
                {
                    ISimpleAuditProcessor processor = SimpleAuditProcessorFactory.CreateProcessor(evInfo);
                    audit.Evidences.Add(processor.Process(evInfo));
                }

                result.Add(audit);
            }

            return result;
        }

        public List<AuditControlProject> GetAuditControlProject(List<string> controlIds, List<string> projectNames)
        {
            List<AuditControlProject> result = new List<AuditControlProject>();
            IEnumerable<IControlProject> controlsProject = ControlsRegistry.Default.GetExecutionControlsProject().Where(c => controlIds.Contains(c.Name));
            foreach (IControlProject p in controlsProject)
            {                
                AuditControlProject audit = new AuditControlProject(p.Information);

                foreach (string projectName in projectNames)
                {
                    if (!audit.EvidencesByProject.ContainsKey(projectName))
                        audit.EvidencesByProject.Add(projectName, new List<AuditEvidence>());

                    foreach (EvidenciaInformation evInfo in p.Information.Evidencias)
                    {
                        IMultiAuditProcessor processor = MultiAuditProcessorFactory.CreateProcessor(evInfo);
                        audit.EvidencesByProject[projectName].Add(processor.Process(evInfo, projectName));
                    }
                }

                result.Add(audit);
            }

            return result;

        }
    }

    public class AuditControlProcess
    {
        private ControlInformation control;
        public ControlInformation Control { get { return control; } }

        private List<AuditEvidence> _evidences;
        public List<AuditEvidence> Evidences { get { return _evidences; } }

        public AuditControlProcess(ControlInformation control)
        {
            this.control = control;
            this._evidences = new List<AuditEvidence>();
        }
    }

    public class AuditControlProject
    {
        private ControlInformation control;
        public ControlInformation Control { get { return control; } }

        private Dictionary<string, List<AuditEvidence>> _evidences;
        public Dictionary<string, List<AuditEvidence>> EvidencesByProject { get { return _evidences; } }

        public AuditControlProject(ControlInformation control)
        {
            this.control = control;
            this._evidences = new Dictionary<string, List<AuditEvidence>>();
        }
    }

    public class AuditEvidence
    {
        private string _url;
        public string Url { get { return _url; } }

        private string _description;
        public String Description { get { return _description; } }

        public AuditEvidence(string desc)
            : this(desc, "")
        {
        }
        public AuditEvidence(string desc, string url)
        {
            _url = url;
            _description = desc;
        }
    }

}
