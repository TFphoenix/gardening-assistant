using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;

namespace ga_forms.Services
{
    public interface IImageManagerService
    {
        // Images
        SKBitmap HealthInitialImageBitmap { get; set; }
        SKBitmap HealthSelectionImageBitmap { get; set; }

        // Paths
        SKPath HealthSelectionPath { get; set; }

        SKBitmap GetHealthSelectedBitmap();
        double GetDiseasePercentage(SKBitmap resultImage);
    }
}
