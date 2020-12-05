using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace ga_forms.Common
{
    public static class Constants
    {
        // HealthSelection
        public static float SelectionStrokeWidth = 20;
        public static float SelectionStrokeOutlineWidth = 30;
        public static float SelectionPathDashLength = 20;
        public static float SelectionPathGapLength = 40;
        public static float SelectionPathPhase = 0;
        public static SKColor SelectionStrokeColor = new SKColor(64, 145, 108);
        public static SKColor SelectionStrokeOutlineColor = new SKColor(248, 248, 248);
        public static SKColor SelectionFillColor = new SKColor(8, 28, 21, 127);
    }
}
