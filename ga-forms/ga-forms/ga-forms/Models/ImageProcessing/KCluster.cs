using SkiaSharp;
using System;
using System.Collections.Generic;
using ga_forms.Common;

namespace ga_forms.Models.ImageProcessing
{
    class KCluster
    {
        // Parameters
        public SKColor Center { get; set; }
        public int PriorCount { get; set; }
        public List<SKColor> Colors { get; set; }

        // ctor
        public KCluster(SKColor center)
        {
            Center = center;
            Colors = new List<SKColor>();
        }

        public bool RecalculateCenter(double threshold = 0.0d)
        {
            SKColor updatedCenter;

            if (Colors.Count > 0)
            {
                float r = 0, g = 0, b = 0;
                foreach (SKColor color in Colors)
                {
                    r += color.Red;
                    g += color.Green;
                    b += color.Blue;
                }

                updatedCenter = new SKColor((byte)Math.Round(r / Colors.Count), (byte)Math.Round(g / Colors.Count), (byte)Math.Round(b / Colors.Count));
            }
            else
            {
                updatedCenter = new SKColor(0, 0, 0);
            }

            double distance = BitmapExtensions.EuclideanDistance(Center, updatedCenter);
            Center = updatedCenter;

            PriorCount = Colors.Count;
            Colors.Clear();

            return distance > threshold;
        }

        public double DistanceFromCenter(SKColor color)
        {
            return BitmapExtensions.EuclideanDistance(color, Center);
        }
    }
}
