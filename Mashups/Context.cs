using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace isa.Mashups
{
    public class Context
    {
        public static readonly string UNKNOWN = "Desconocido";

        public string Region { get { return _region; } }
        private string _region;

        public string Country { get { return _country; } }
        private string _country;

        public string Area 
        { 
            get 
            { 
                if (! string.IsNullOrEmpty(_area))
                    return _area; 
                else
                    return UNKNOWN;
            } 
        }
        private string _area;

        public string Section { get { return _section; } }
        private string _section;

        public string Department 
        { 
            get 
            {
                if (!string.IsNullOrEmpty(_department))
                    return _department;
                else
                    return UNKNOWN;
            } 
        }
        private string _department;

        public Context(string region, string country, string area, string section, string department)
        {
            _region = region;
            _country = country;
            _area = area;
            _section = section;
            _department = department;
        }

        public bool ParentOf(Context c)
        {
            return MatchProperty(c, p => p.Region) &&
                MatchProperty(c, p => p.Country) &&
                MatchProperty(c, p => p.Area) &&
                MatchProperty(c, p => p.Section) &&
                MatchProperty(c, p => p.Department);
        }

        private bool MatchProperty(Context c, Func<Context, string> getter)
        {
            return getter(c).Length == 0 || getter(c).Equals(getter(this), StringComparison.CurrentCultureIgnoreCase);
        }

        public override string ToString()
        {            
            return "[ " + _region + ", " + _country + ", " + _area + ", " + _section + ", " +_department +" ]";
        }
    }
}
