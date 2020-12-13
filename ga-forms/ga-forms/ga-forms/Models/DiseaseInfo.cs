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
        public DiseaseResultType DiseaseResult { get; set; }

        private string _iconUrl;
        public string IconUrl
        {
            get
            {
                switch (DiseaseResult)
                {
                    case DiseaseResultType.Ok:
                        _iconUrl = "ok.png";
                        break;
                    case DiseaseResultType.Warning:
                        _iconUrl = "warning.png";
                        break;
                    case DiseaseResultType.Error:
                        _iconUrl = "error.png";
                        break;
                    default:
                        _iconUrl = "unknow.png";
                        break;
                }
                return _iconUrl;
            }
            set
            {
                _iconUrl = value;
            }
        }
        public DiseaseInfo(string name, string details, DiseaseResultType diseaseResult)
        {
            Name = name;
            Details = details;
            DiseaseResult = diseaseResult;
        }
        

    }
}
