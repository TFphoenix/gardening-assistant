using System;
using System.Collections.Generic;
using System.Text;
using ga_forms.Common.Enums;

namespace ga_forms.Models
{
    class DiseaseInfo 
    {
        public DiseaseInfo(string name, string imageUrl, string details, DiseaseResultType diseaseResult)
        {
            Name = name;
            ImageUrl = imageUrl;
            Details = details;
            DiseaseResult = diseaseResult;
        }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Details { get; set; }
        
        private DiseaseResultType DiseaseResult { get; set; }
        public string IconUrl
        {
            get
            {
                switch (DiseaseResult)
                {
                    case DiseaseResultType.Ok:
                        return "ok.png";
                    case DiseaseResultType.Warning:
                        return "warning.png";
                    case DiseaseResultType.Error:
                        return "error.png";
                    default:
                        return "unknow.png";
                }
            }
            set
            {

            }
        }

    }
}
