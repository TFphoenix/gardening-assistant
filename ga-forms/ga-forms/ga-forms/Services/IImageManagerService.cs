using SkiaSharp;

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
        double GetDiseasePercentage(SKBitmap croppedImage, SKBitmap resultImage);
    }
}
