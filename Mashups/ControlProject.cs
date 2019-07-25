using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using isa.Mashups;
using de.ahzf.Styx;
using icinetic.BPCMSPipes;
using icinetic.CatalogoProyectos;
using icinetic.BPCMSPipes.Proyectos;

namespace Mashups
{
    class ControlProject : IControlProject
    {
        private IEnumerable<MashupConfiguration> mashups;

        public ControlInformation Information { get { return info; } }
        private ControlInformation info;

        public ControlProject(ControlInformation info, IEnumerable<MashupConfiguration> mashups)
        {
            this.mashups = mashups;
            this.info = info;
        }

        #region IControl<List<ProjectEvaluation>> Members

        public string Name
        {
            get { return info.ControlDescription.Id; }
        }
        
        public string Description
        {
            get { return info.ControlDescription.Description; }
        }
        
        public List<ControlProjectEvaluation> Evaluate()
        {
            List<List<ControlProjectEvaluation>> evaluations = new List<List<ControlProjectEvaluation>>();
            List<ControlProjectEvaluation> result;
            
            mashups.ForEach(m => evaluations.Add(InternalEvaluate(m)));

            if (evaluations.Count == 0)
            {
                result = SetAllTrue();
                // result = new List<ControlProjectEvaluation>();
            }
            else
            {
                result = MergeEvaluations(evaluations);                
            }

            return result;

        }

        private List<ControlProjectEvaluation> MergeAllEvaluations(List<List<ControlProjectEvaluation>> evaluations)
        {
            List<ControlProjectEvaluation> result = new List<ControlProjectEvaluation>();
            List<IDictionary<string, ControlProjectEvaluation>> dics = new List<IDictionary<string, ControlProjectEvaluation>>();

            foreach (List<ControlProjectEvaluation> eval in evaluations)
            {
                IDictionary<string, ControlProjectEvaluation> dic = new SortedDictionary<string, ControlProjectEvaluation>();
                foreach (ControlProjectEvaluation p in eval)
                {
                    if (dic.ContainsKey(p.Project))
                    {
                        dic[p.Project] = MergeProjectEvaluation(dic[p.Project], p);
                    }
                    else
                    {
                        dic.Add(p.Project, p);
                    }
                }

                dics.Add(dic);
            }

            // We only consider those projects that are included in all evaluations
            IEnumerable<string> projects = dics[0].Keys;
            foreach (IDictionary<string, ControlProjectEvaluation> dic in dics)
            {
                projects = projects.Intersect(dic.Keys);
            }

            foreach (string projectName in projects)
            {
                ControlProjectEvaluation p = dics[0][projectName];
                for (int i = 1; i < dics.Count; i++)
                {
                    p = MergeProjectEvaluation(p, dics[i][projectName]);
                }

                result.Add(p);
            }

            return result;
        }

        private List<ControlProjectEvaluation> MergeEvaluations(List<List<ControlProjectEvaluation>> evaluations)
        {
            List<ControlProjectEvaluation> result;
            IDictionary<string, ControlProjectEvaluation> dic = new SortedDictionary<string, ControlProjectEvaluation>();

            foreach (List<ControlProjectEvaluation> eval in evaluations)
            {
                foreach (ControlProjectEvaluation p in eval)
                {
                    if (dic.ContainsKey(p.Project))
                    {
                        dic[p.Project] = MergeProjectEvaluation(dic[p.Project], p);
                    }
                    else
                    {
                        dic.Add(p.Project, p);
                    }
                }
            }

            //List<ControlProjectEvaluation> first = evaluations[0];

            //foreach (ControlProjectEvaluation p in first)
            //{
            //    if (dic.ContainsKey(p.Project))
            //    {
            //        dic[p.Project] = MergeProjectEvaluation(dic[p.Project], p);
            //    }
            //    else
            //    {
            //        dic.Add(p.Project, p);
            //    }
            //}

            //for (int i = 1; i < evaluations.Count; i++)
            //{
            //    foreach (ControlProjectEvaluation p in evaluations[i])
            //    {
            //        if (dic.ContainsKey(p.Project))
            //        {
            //            dic[p.Project] = MergeProjectEvaluation(dic[p.Project], p);
            //        }
            //        else
            //        {
            //            dic.Add(p.Project, p);
            //        }
            //    }
            //}

            result = new List<ControlProjectEvaluation>(dic.Values);
            return result;
        }

        private List<ControlProjectEvaluation> SetAllTrue()
        {
            List<ControlProjectEvaluation> result = new List<ControlProjectEvaluation>();

            // Data source
            IPipe<Proyecto, Proyecto> projectPipe = new ProyectoPipe();
            projectPipe.SetSourceCollection(MashupsBuilder.GetProjectsDataSource());

            // Filtro por subproyecto (si tiene subproyectos fuera)
            IPipe<Proyecto, Proyecto> filterSubprojectsPipe = new FuncFilterPipe<Proyecto>(delegate(Proyecto p) { return (p.SubProyectoList.Count > 0); });
            filterSubprojectsPipe.SetSourceCollection(projectPipe);

            foreach (Proyecto p in filterSubprojectsPipe)
            {
                ProyectoSubproyecto ps = new ProyectoSubproyecto(p);
                ControlProjectEvaluation  pEval = new ControlProjectEvaluation(
                    new Context(ps.Region, ps.Pais, ps.Area, ps.Direccion, ps.Linea),
                    ps,true);

                result.Add(pEval);
            }

            // Data source
            projectPipe = new ProyectoPipe();
            projectPipe.SetSourceCollection(MashupsBuilder.GetProjectsDataSource());

            // Obtenemos subproyectos
            IPipe<Proyecto, SubProyecto> subProyectPipe = new SubProyectoPipe();
            subProyectPipe.SetSourceCollection(projectPipe);

            foreach (SubProyecto p in subProyectPipe)
            {
                ProyectoSubproyecto ps = new ProyectoSubproyecto(p);
                ControlProjectEvaluation pEval = new ControlProjectEvaluation(
                    new Context(ps.Region, ps.Pais, ps.Area, ps.Direccion, ps.Linea),
                    ps, true);

                result.Add(pEval);
            }

            return result;
            
        }

        private ControlProjectEvaluation MergeProjectEvaluation(ControlProjectEvaluation projectEvaluation, ControlProjectEvaluation p)
        {
            projectEvaluation.Evaluation = projectEvaluation.Evaluation && p.Evaluation;
            return projectEvaluation;
        }

        private List<ControlProjectEvaluation> InternalEvaluate(MashupConfiguration c)
        {
            List<ControlProjectEvaluation> result = EvaluateProjects(c);
            result.AddRange(EvaluateSubprojects(c));

            return result;
        }

        List<ControlProjectEvaluation> EvaluateProjects(MashupConfiguration c)
        {
            List<ControlProjectEvaluation> result = new List<ControlProjectEvaluation>();

            // Data source
            IPipe<Proyecto, Proyecto> projectPipe = new ProyectoPipe();
            projectPipe.SetSourceCollection(MashupsBuilder.GetProjectsDataSource());

            // Filtro por subproyecto (si tiene subproyectos fuera)
            IPipe<Proyecto, Proyecto> filterSubprojectsPipe = new FuncFilterPipe<Proyecto>(delegate(Proyecto p) { return (p.SubProyectoList.Count > 0); });
            filterSubprojectsPipe.SetSourceCollection(projectPipe);

            foreach (Proyecto p in filterSubprojectsPipe)
            {
                ProyectoSubproyecto ps = new ProyectoSubproyecto(p);
                ControlProjectEvaluation pEval; 
                bool wasEvaluated = TryGenericEvaluation(c, ps, out pEval);

                if (wasEvaluated)
                    result.Add(pEval);
            }

            return result;

        }

        List<ControlProjectEvaluation> EvaluateSubprojects(MashupConfiguration c)
        {
            List<ControlProjectEvaluation> result = new List<ControlProjectEvaluation>();
            // Data source
            IPipe<Proyecto, Proyecto> projectPipe = new ProyectoPipe();
            projectPipe.SetSourceCollection(MashupsBuilder.GetProjectsDataSource());

            // Obtenemos subproyectos
            IPipe<Proyecto, SubProyecto> subProyectPipe = new SubProyectoPipe();
            subProyectPipe.SetSourceCollection(projectPipe);

            foreach (SubProyecto s in subProyectPipe)
            {
                ProyectoSubproyecto ps = new ProyectoSubproyecto(s);
                ControlProjectEvaluation pEval;
                bool wasEvaluated = TryGenericEvaluation(c, ps, out pEval);

                if (wasEvaluated)
                    result.Add(pEval);
            }

            return result;

        }

        private bool TryGenericEvaluation(MashupConfiguration c, ProyectoSubproyecto ps, out ControlProjectEvaluation evaluation)
        {
            MashupContainer container = c.Build();
            if (c.IsProjectMashup())
            {
                SetProjectListInDataSource(ps, container);
            }

            bool proyectoEvaluation;
            bool wasEvaluated = TryEvaluateProyecto(container, out proyectoEvaluation);

            evaluation = new ControlProjectEvaluation(
                new Context(ps.Region, ps.Pais, ps.Area, ps.Direccion, ps.Linea),
                ps,
                proyectoEvaluation);

            return wasEvaluated;
        }

        private bool TryEvaluateProyecto(MashupContainer container, out bool proyectoEvaluation)
        {
            bool wasEvaluated = false;
            proyectoEvaluation = false;

            IPipe pipe = container.Pipe;

            if (pipe.MoveNext())
            {
                wasEvaluated = true;
                if (pipe.Current is bool)
                {
                    proyectoEvaluation = (bool)pipe.Current;
                    while (pipe.MoveNext())
                    {
                        proyectoEvaluation = proyectoEvaluation && (bool)pipe.Current;
                    }
                }
            }

            return wasEvaluated;
        }

        private void SetProjectListInDataSource(ProyectoSubproyecto ps, MashupContainer container)
        {
            foreach (IDataSource dataSource in container.DataSources)
            {
                if (dataSource is ProyectoSubproyectoDataSource)
                {
                    ProyectoSubproyectoDataSource psds = (ProyectoSubproyectoDataSource)dataSource;
                    psds.SetSourceList(new ProyectoSubproyecto[] { ps });
                }
            }
        }


        #endregion
    }
}
