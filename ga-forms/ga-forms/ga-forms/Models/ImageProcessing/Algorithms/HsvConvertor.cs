using ga_forms.Common.Enums;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ga_forms.Models.ImageProcessing.Algorithms
{
    class HsvConvertor:IAlgorithm
    {
        private Dictionary<HueColor, int> Histogram { get; set; }
        public SKBitmap ProcessingImage { get; set; }
        public SKBitmap ProcessedImage { get; set; }
        public void Execute()
        {
            ProcessedImage = new SKBitmap(ProcessingImage.Width, ProcessingImage.Height);
            for (int x = 0; x < ProcessingImage.Width; x++)
            {
                for (int y = 0; y < ProcessingImage.Height; y++)
                {
                    double delta, min, h = 0, s, v;
                    SKColor color = ProcessingImage.GetPixel(x, y);
                    min = Math.Min(Math.Min(color.Red, color.Green), color.Blue);
                    v = Math.Max(Math.Max(color.Red, color.Green), color.Blue);
                    delta = v - min;
                    if(v == 0.0)
                    {
                        s = 0;
                    }
                    else
                    {
                        s = delta / v;
                    }
                    if(s==0)
                    {
                        h = 0.0;
                    }
                    else
                    {
                        if(color.Red == v)
                        {
                            h = (color.Green - color.Blue) / delta;
                        }
                        else if(color.Green == v)
                        {
                            h = 2 + (color.Blue - color.Red) / delta;
                        }
                        else if(color.Blue == v)
                        {
                            h = 4 + (color.Red - color.Green) / delta;
                        }
                        h *= 60;
                        if(h < 0.0)
                        {
                            h = h + 360;
                        }
                    }
                    if(h >= 0 && h <= 60)
                    {
                        Histogram[HueColor.Red]++;
                    }
                    if(h>=61&& h<=120)
                    {
                        Histogram[HueColor.Yellow]++;
                    }
                    if(h>=121 && h<=180)
                    {
                        Histogram[HueColor.Green]++;
                    }
                    if(h>=181 && h<=240)
                    {
                        Histogram[HueColor.Cyan]++;
                    }
                    if(h>=241 && h<=300)
                    {
                        Histogram[HueColor.Blue]++;
                    }
                    if (h >= 301 && h <= 360)
                    {
                        Histogram[HueColor.Magenta]++;
                    }
                }
            }
        }
        public HueColor GetPredominantColor()
        {
            return Histogram.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }
    }
}
