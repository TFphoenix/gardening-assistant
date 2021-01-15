using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ga_forms.Models.ImageProcessing.Algorithms
{
    public class GrayscaleConvertor : IAlgorithm
    {
        public SKBitmap ProcessingImage { get; set; }
        public SKBitmap ProcessedImage { get; set; }

        public void Execute()
        {
            ProcessedImage = new SKBitmap(ProcessingImage.Width, ProcessingImage.Height);
            for (int x = 0; x < ProcessingImage.Width; ++x)
            {
                for (int y = 0; y < ProcessingImage.Height; ++y)
                {
                    SKColor color = ProcessingImage.GetPixel(x, y);
                    double grayscale = color.Red * 0.3 + color.Green * 0.59 + color.Blue * 0.11;
                    byte value = (byte)grayscale;
                    ProcessedImage.SetPixel(x, y, new SKColor(value, value, value, color.Alpha));
                }
            }
        }
    }
}
