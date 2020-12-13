using System;
using System.Collections.Generic;
using System.Text;
using ga_forms.Common.Enums;
using ga_forms.Models.ImageProcessing;
using ga_forms.Services;
using Xamarin.Forms;

namespace ga_forms.Models
{
    class DiseaseInfo
    {
        public string Name { get; set; }
        public ImageSource ImgSource { get; set; }
        public string Details { get; set; }
        public double Percentage { get; set; }
        private DiseaseResultType DiseaseResult { get; set; }

        public DiseaseInfo(string name, string details, DiseaseResultType diseaseResult)
        {
            Name = name;
            Details = details;
            DiseaseResult = diseaseResult;
        }

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
        }

    }
}
