using ga_forms.Common.Enums;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ga_forms.Models.ImageProcessing.Algorithms
{
    interface IAlgorithm
    {
        SKBitmap ProcessingImage { get; set; }
        SKBitmap ProcessedImage { get; set; }

        //TODO
        //Dictionary<TransientDataType, Object> TransientData { get; set; }

        void Execute();
    }
}
