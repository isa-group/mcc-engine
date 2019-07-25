using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa.Mashups;
using System.Configuration;
using de.ahzf.Styx;
using icinetic.BPCMSPipes;
using icinetic.GesMaApp;

namespace Mashups.Auditoria
{
    public interface ISimpleAuditProcessor
    {
        AuditEvidence Process(EvidenciaInformation info);
    }

    public class SimpleAuditProcessorFactory
    {

        public static ISimpleAuditProcessor CreateProcessor(EvidenciaInformation info)
        {
            ISimpleAuditProcessor processor;
            if (info.EvidenciaDescription.Tipo.Equals("Herramienta", StringComparison.CurrentCultureIgnoreCase))
                processor = new ToolAuditProcessor();
            else
                processor = new MetodologiaAuditProcessor();

            return processor;
        }

        
    }


    public class ToolAuditProcessor : ISimpleAuditProcessor, IMultiAuditProcessor
    {
        public AuditEvidence Process(EvidenciaInformation info)
        {
            string desc = "La evidencia se encuentra en la herramienta " + info.EvidenciaDescription.Nombre +". ";
            desc += "Para acceder a la información hay que consultar lo siguiente: " + info.EvidenciaDescription.Descripcion;

            AuditEvidence evidence = new AuditEvidence(desc);

            return evidence;
        }

        public AuditEvidence Process(EvidenciaInformation info, string projectName)
        {
            return Process(info);
        }
    }

    public class MetodologiaAuditProcessor : ISimpleAuditProcessor, IMultiAuditProcessor
    {
        public AuditEvidence Process(EvidenciaInformation info)
        {
            return new AuditEvidence("La metodología garantiza el cumplimiento del control");
        }

        public AuditEvidence Process(EvidenciaInformation info, string projectName)
        {
            return Process(info);
        }

    }
}
