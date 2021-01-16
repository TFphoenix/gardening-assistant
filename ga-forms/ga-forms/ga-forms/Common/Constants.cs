using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace ga_forms.Common
{
    public static class Constants
    {
        // HealthSelection
        public const float SelectionStrokeWidth = 20;
        public const float SelectionStrokeOutlineWidth = 30;
        public const float SelectionPathDashLength = 20;
        public const float SelectionPathGapLength = 40;
        public const float SelectionPathPhase = 0;
        public static SKColor SelectionStrokeColor = new SKColor(64, 145, 108);
        public static SKColor SelectionStrokeOutlineColor = new SKColor(248, 248, 248);
        public static SKColor SelectionFillColor = new SKColor(8, 28, 21, 127);

        // General
        public const float RESCALE_FACTOR = 0.25f;
        public const string NO_PROCESSING_IMAGE = "This algorithm does not define a processing image";
        public const string NO_PROCESSED_IMAGE = "This algorithm does not define a processed image";
    }
}
