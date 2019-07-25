using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mashups.ControlsExcel;

namespace isa.Mashups
{
    // clase para cargar la informacion de controles de un archivo Excel
    public class ControlInformation
    {
        ControlDescription controlDescription;
        public ControlDescription ControlDescription
        {
            get { return controlDescription; }
            set { controlDescription = value; }
        }

        Context context;
        public Context Context
        {
            get { return context; }
            set { context = value; }
        }


        //EvidenciaDescription evidenciaDescription;
        //public EvidenciaDescription EvidenciaDescription
        //{
        //    get { return evidenciaDescription; }
        //    set { evidenciaDescription = value; }
        //}

        //ProcedeDescription procedeDescription;
        //public ProcedeDescription ProcedeDescription
        //{
        //    get { return procedeDescription; }
        //    set { procedeDescription = value; }
        //}


        //InfoGeograficaDescription infoGeograficaDescription;

        //public InfoGeograficaDescription InfoGeograficaDescription
        //{
        //    get { return infoGeograficaDescription; }
        //    set { infoGeograficaDescription = value; }
        //}

        //InfoOrganizativaDescription infoOrganizativaDescription;

        //public InfoOrganizativaDescription InfoOrganizativaDescription
        //{
        //    get { return infoOrganizativaDescription; }
        //    set { infoOrganizativaDescription = value; }
        //}

        //public List<MashupDescription> DesignMashupsDescription
        //{
        //    get { return designMashups; }
        //}
        //private List<MashupDescription> designMashups;

        //public List<MashupDescription> RuntimeMashupsDescription
        //{
        //    get { return runtimeMashups; }
        //}
        //private List<MashupDescription> runtimeMashups;
        public List<EvidenciaInformation> Evidencias { get { return _evidencias; } }
        private List<EvidenciaInformation> _evidencias;

        public ControlInformation() 
        {
            //designMashups = new List<MashupDescription>();
            //runtimeMashups = new List<MashupDescription>();
            _evidencias = new List<EvidenciaInformation>();
        }
    }

    public class EvidenciaInformation
    {
        EvidenciaDescription evidenciaDescription;
        public EvidenciaDescription EvidenciaDescription
        {
            get { return evidenciaDescription; }
        }

        ProcedeDescription procedeDescription;
        public ProcedeDescription ProcedeDescription
        {
            get { return procedeDescription; }
        }

        public MashupDescription RuntimeMashupDescription { get { return _runtimeMashup; } }
        private MashupDescription _runtimeMashup;

        public MashupDescription DesignMashupDescription { get { return _designMashup; } }
        private MashupDescription _designMashup;

        public EvidenciaInformation(EvidenciaDescription evidencia, ProcedeDescription procede, MashupDescription design, MashupDescription runtime)
        {
            this.evidenciaDescription = evidencia;
            this.procedeDescription = procede;
            this._designMashup = design;
            this._runtimeMashup = runtime;
        }

            
    }
}
