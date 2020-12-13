using ga_forms.Common;
using SkiaSharp;

namespace ga_forms.Services
{
    public class ImageManagerService : IImageManagerService
    {
        public SKBitmap HealthInitialImageBitmap { get; set; }
        public SKBitmap HealthSelectionImageBitmap { get; set; }
        public SKPath HealthSelectionPath { get; set; }

        public SKBitmap GetHealthSelectedBitmap()
        {
            SKBitmap initialImageStretched = new SKBitmap(HealthSelectionImageBitmap.Width, HealthSelectionImageBitmap.Height);
            using (SKCanvas canvas = new SKCanvas(initialImageStretched))
            {
                canvas.DrawBitmap(
                    HealthInitialImageBitmap,
                    new SKRect(0, 0, HealthSelectionImageBitmap.Width, HealthSelectionImageBitmap.Height),
                    BitmapStretch.Uniform);
            }

            SKBitmap selectedBitmap = new SKBitmap(HealthSelectionImageBitmap.Width, HealthSelectionImageBitmap.Height);

            for (int x = 0; x < HealthSelectionImageBitmap.Width; ++x)
            {
                for (int y = 0; y < HealthSelectionImageBitmap.Height; ++y)
                {
                    selectedBitmap.SetPixel(x, y,
                        HealthSelectionImageBitmap.GetPixel(x, y).Red == 0
                            ? new SKColor(0, 0, 0, 0)
                            : initialImageStretched.GetPixel(x, y));
                }
            }

            return selectedBitmap;
        }

        public double GetDiseasePercentage(SKBitmap croppedImage, SKBitmap resultImage)
        {
            int totalNumberOfPixels = 0;
            int numberOfBlackPixels = 0;

            for (int x = 0; x < resultImage.Width; ++x)
            {
                for (int y = 0; y < resultImage.Height; ++y)
                {
                    if (croppedImage.GetPixel(x,y).Alpha != 0)
                    {
                        totalNumberOfPixels++;
                        if (resultImage.GetPixel(x, y).Red == 0)
                        {
                            numberOfBlackPixels++;
                        }
                    }
                }
            }
            return (double)numberOfBlackPixels * 100 / totalNumberOfPixels;
        }
    }
}
