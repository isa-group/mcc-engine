using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa.Mashups;
using icinetic.BPCMSPipes;
using icinetic.GesMaApp;
using de.ahzf.Styx;
using isa.BPCMSPipes;
using System.Configuration;
using icinetic.CatalogoProyectos;

namespace Mashups.Auditoria
{
    public interface IMultiAuditProcessor
    {
        AuditEvidence Process(EvidenciaInformation info, string projectName);
    }


}
