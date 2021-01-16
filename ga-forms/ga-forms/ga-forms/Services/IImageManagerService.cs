using SkiaSharp;
using System;
using System.Collections.Generic;

namespace ga_forms.Services
{
    public interface IImageManagerService
    {
        // Images
        SKBitmap HealthInitialImageBitmap { get; set; }
        SKBitmap HealthSelectionImageBitmap { get; set; }
        SKBitmap DecorateInitialImageBitmap { get; set; }
        SKBitmap DecorateSelectionImageBitmap { get; set; }

        // Paths
        SKPath HealthSelectionPath { get; set; }
        SKPath DecorateSelectionPath { get; set; }

        SKBitmap GetHealthSelectedBitmap();
        SKBitmap GetDecorateSelectedBitmap();
        double GetDiseasePercentage(SKBitmap croppedImage, SKBitmap resultImage);
        Tuple<SKBitmap, SKBitmap, SKBitmap> GetDecorateImages(SKBitmap croppedImage, List<SKColor> dominantColors);
    }
}
