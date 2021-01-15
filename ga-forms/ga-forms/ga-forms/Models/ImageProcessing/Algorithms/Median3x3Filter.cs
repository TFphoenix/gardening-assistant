using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace ga_forms.Models.ImageProcessing.Algorithms
{
    class Median3x3Filter : IAlgorithm
    {
        public SKBitmap ProcessingImage { get; set; }
        public SKBitmap ProcessedImage { get; set; }

        public void Execute()
        {
            ProcessedImage = new SKBitmap(ProcessingImage.Width, ProcessingImage.Height);

            for (int x = 0; x < ProcessingImage.Width; x++)
            {
                for (int y = 0; y < ProcessingImage.Height; y++)
                {
                    SKColor pixel = ProcessingImage.GetPixel(x, y);

                    if (x == 0 || y == 0 || x == ProcessingImage.Width - 1 || y == ProcessingImage.Height - 1)
                    {
                        ProcessedImage.SetPixel(x, y, pixel);
                    }
                    else
                    {
                        // 3x3 mask
                        List<byte> mask = new List<byte>
                        {
                            ProcessingImage.GetPixel(x-1,y-1).Red,
                            ProcessingImage.GetPixel(x-1,y).Red,
                            ProcessingImage.GetPixel(x-1,y+1).Red,
                            ProcessingImage.GetPixel(x,y-1).Red,
                            ProcessingImage.GetPixel(x,y).Red,
                            ProcessingImage.GetPixel(x,y+1).Red,
                            ProcessingImage.GetPixel(x+1,y-1).Red,
                            ProcessingImage.GetPixel(x+1,y).Red,
                            ProcessingImage.GetPixel(x+1,y+1).Red
                        };

                        mask.Sort();
                        ProcessedImage.SetPixel(x, y, new SKColor(mask[4], mask[4], mask[4], pixel.Alpha));
                    }

                }
            }
        }
    }
}
