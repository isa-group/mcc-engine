using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa.Mashups;
using icinetic.CatalogoProyectos;
using icinetic.EAService;
using System.Configuration;

namespace Mashups
{
    public class MashupsBuilder
    {
        public static List<IControlByProject> GetControlsByProject()
        {
            List<Proyecto> projects = GetProjectsDataSource();

            List<IControlByProject> controls = new List<IControlByProject>();

            return controls;
        }

        public static List<Proyecto> GetProjectsDataSource()
        {
            ProyectoService projectService = new ProyectoService();
            List<Proyecto> projects = projectService.FindAll();

            return projects;
        }

        public static List<IControlGlobal> GetControlsGlobal()
        {
            BPMService serviceEA = GetEADataSource();

            List<IControlGlobal> controls = new List<IControlGlobal>();

            

            return controls;
        }

        public static BPMService GetEADataSource()
        {
            // Data source
            BPMService serviceEA = new BPMService();
            return serviceEA;
        }

    }
}
