using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace ga_forms.Models.ImageProcessing.Algorithms
{
    class ColorSegmentation : IAlgorithm
    {
        public SKBitmap ProcessingImage { get; set; }
        public SKBitmap ProcessedImage { get; set; }
        public SKColor Color { get; set; }
        public float EstimatedDistance { get; set; }

        public ColorSegmentation()
        {
            Color = new SKColor(127, 127, 127);
            EstimatedDistance = 10f;
        }

        public void Execute()
        {
            for (int x = 0; x < ProcessingImage.Width; x++)
            {
                for (int y = 0; y < ProcessingImage.Height; y++)
                {
                    SKColor pixel = ProcessingImage.GetPixel(x, y);
                    float distance = (float)Math.Sqrt(
                        ((int)pixel.Red - (int)Color.Red) * ((int)pixel.Red - (int)Color.Red) +
                        ((int)pixel.Green - (int)Color.Green) * ((int)pixel.Green - (int)Color.Green) +
                        ((int)pixel.Blue - (int)Color.Blue) * ((int)pixel.Blue - (int)Color.Blue)
                    );

                    ProcessedImage.SetPixel(x, y,
                        distance <= EstimatedDistance
                            ? new SKColor(255, 255, 255, pixel.Alpha)
                            : new SKColor(0, 0, 0, pixel.Alpha));
                }
            }
        }
    }
}
