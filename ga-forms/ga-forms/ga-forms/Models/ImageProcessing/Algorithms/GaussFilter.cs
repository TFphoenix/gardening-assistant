using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ga_forms.Models.ImageProcessing.Algorithms
{
    public class GaussFilter:IAlgorithm
    {
        public SKBitmap ProcessingImage { get ; set; }
        public SKBitmap ProcessedImage { get ; set; }

        public void Execute()
        {
            ProcessedImage = new SKBitmap(ProcessingImage.Width, ProcessingImage.Height);
            int sigma = 3;
            double constant = 1d / (2 * Math.PI * sigma * sigma);
            for (int y = sigma; y < ProcessingImage.Height - sigma; y++)
            {
                for (int x = sigma; x < ProcessingImage.Width - sigma; x++)
                {
                    double suma = 0, distanta = 0, sumBeforeDivide = 0;
                    for (int i = -sigma; i <= sigma; i++)
                    {
                        for (int j = -sigma; j <= sigma; j++)
                        {
                            distanta = ((i * i) + (j * j)) / (2 * sigma * sigma);
                            sumBeforeDivide = sumBeforeDivide + ProcessingImage.GetPixel(x+i,y+i).Red * (constant * Math.Exp(-distanta));
                            suma = suma + constant * Math.Exp(-distanta);
                        }
                    }
                    for (int i = -sigma; i <= sigma; i++)
                        for (int j = -sigma; j <= sigma; j++)
                        {
                            byte value = (byte)(sumBeforeDivide / suma);
                            ProcessedImage.SetPixel(x, y, new SKColor(value, value, value));
                        }

                }
            }
            for (int y = 0; y < sigma; y++)
                for (int x = 0; x < ProcessingImage.Width; x++)
                {
                    ProcessedImage.SetPixel(x, y, ProcessingImage.GetPixel(x, y));
                    ProcessedImage.SetPixel(ProcessingImage.Width - x - 1, y, ProcessingImage.GetPixel(ProcessingImage.Width - x - 1, y));
                }
            for (int y = ProcessingImage.Height - sigma; y < ProcessingImage.Height; y++)
                for (int x = 0; x < ProcessingImage.Width; x++)
                {
                    ProcessedImage.SetPixel(x, y, ProcessingImage.GetPixel(x, y));
                    ProcessedImage.SetPixel(ProcessingImage.Width - x - 1, y, ProcessingImage.GetPixel(ProcessingImage.Width-x-1, y));
                }
            for (int y = 0; y < ProcessingImage.Height; y++)
                for (int x = 0; x < sigma; x++)
                {
                    ProcessedImage.SetPixel(x, y, ProcessingImage.GetPixel(x, y));
                    ProcessedImage.SetPixel(ProcessingImage.Width - x - 1, y, ProcessingImage.GetPixel(ProcessingImage.Width - x - 1, y));
                }
            for (int y = 0; y < ProcessingImage.Height; y++)
                for (int x = ProcessingImage.Width - sigma; x < ProcessingImage.Width; x++)
                {
                    ProcessedImage.SetPixel(x, y, ProcessingImage.GetPixel(x, y));
                    ProcessedImage.SetPixel(ProcessingImage.Width - x - 1, y, ProcessingImage.GetPixel(ProcessingImage.Width - x - 1, y));
                }
        }
    }
}
