using SkiaSharp;

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

        SKBitmap GetHealthSelectedBitmap();
        SKBitmap GetDecorateSelectedBitmap();
        double GetDiseasePercentage(SKBitmap croppedImage, SKBitmap resultImage);
    }
}
