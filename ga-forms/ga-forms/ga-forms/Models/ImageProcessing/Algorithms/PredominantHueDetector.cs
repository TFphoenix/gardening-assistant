using ga_forms.Common.Enums;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ga_forms.Common;

namespace ga_forms.Models.ImageProcessing.Algorithms
{
    class PredominantHueDetector : IAlgorithm
    {
        public SKBitmap ProcessingImage { get; set; }
        public SKBitmap ProcessedImage { get; set; }
        public int PredominantHue { get; set; }

        private readonly int[] _histogram = new int[361];

        public void Execute()
        {
            ProcessedImage = new SKBitmap(ProcessingImage.Width, ProcessingImage.Height);

            for (int x = 0; x < ProcessingImage.Width; x++)
            {
                for (int y = 0; y < ProcessingImage.Height; y++)
                {
                    ++_histogram[BitmapExtensions.RgbToHsv(ProcessingImage.GetPixel(x, y)).H];
                }
            }

            PredominantHue = _histogram.ToList().IndexOf(_histogram.Max());
        }
    }
}
