using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa.Mashups;

namespace Mashups.Auditoria
{
    public abstract class AuditProcessProcessor
    {

        public static AuditProcessProcessor CreateProcessor(EvidenciaInformation info)
        {
            if (info.EvidenciaDescription.Tipo.Equals("Documento", StringComparison.CurrentCultureIgnoreCase))
            {

            }
            return null;
        }

        public abstract AuditEvidence Process(EvidenciaInformation info);
    }
}
