using System;

namespace isa.Mashups
{
    public class ProjectsState
    {
        public string Project;
        public Boolean State;
        private string p;
        private string p_2;

        public ProjectsState(string proj, Boolean st)
        {
            Project = proj;
            State = st;
        }

        public ProjectsState(string p, string p_2)
        {
            // TODO: Complete member initialization
            this.p = p;
            this.p_2 = p_2;
        }
    }
}
