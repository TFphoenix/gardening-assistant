using ga_forms.Common;
using SkiaSharp;
using System;

namespace ga_forms.Services
{
    public class ImageManagerService : IImageManagerService
    {
        public SKBitmap HealthInitialImageBitmap { get; set; }
        public SKBitmap HealthSelectionImageBitmap { get; set; }
        public SKPath HealthSelectionPath { get; set; }
        public SKBitmap DecorateInitialImageBitmap { get; set; }
        public SKBitmap DecorateSelectionImageBitmap { get; set; }
        public SKPath DecorateSelectionPath { get; set; }

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

        public SKBitmap GetDecorateSelectedBitmap()
        {
            SKBitmap initialImageStretched = new SKBitmap(DecorateSelectionImageBitmap.Width, DecorateSelectionImageBitmap.Height);
            using (SKCanvas canvas = new SKCanvas(initialImageStretched))
            {
                canvas.DrawBitmap(
                    DecorateInitialImageBitmap,
                    new SKRect(0, 0, DecorateSelectionImageBitmap.Width, DecorateSelectionImageBitmap.Height),
                    BitmapStretch.Uniform);
            }

            SKBitmap selectedBitmap = new SKBitmap(DecorateSelectionImageBitmap.Width, DecorateSelectionImageBitmap.Height);

            for (int x = 0; x < DecorateSelectionImageBitmap.Width; ++x)
            {
                for (int y = 0; y < DecorateSelectionImageBitmap.Height; ++y)
                {
                    selectedBitmap.SetPixel(x, y,
                        DecorateSelectionImageBitmap.GetPixel(x, y).Red == 0
                            ? new SKColor(0, 0, 0, 0)
                            : initialImageStretched.GetPixel(x, y));
                }
            }

            return selectedBitmap;
        }

        public Tuple<SKBitmap,SKBitmap> GetTriadImages(SKBitmap originalImage, SKBitmap croppedImage, int predominantColor)
        {
            SKBitmap firstImage = new SKBitmap(originalImage.Width, originalImage.Height);
            SKBitmap secondImage = new SKBitmap(originalImage.Width, originalImage.Height);
            int firstColorHue = 0, secondColorHue = 0;
            if((predominantColor + 120) < 360 && (predominantColor - 120) > 0)
            {
                firstColorHue = predominantColor + 120;
                secondColorHue = predominantColor - 120;
            }
            else if((predominantColor + 120 > 360) && (predominantColor - 120) > 0)
            {
                firstColorHue = 120 - 360 - predominantColor;
                secondColorHue = predominantColor - 120;
            }
            else if((predominantColor + 120) < 360 && (predominantColor - 120) < 0)
            {
                firstColorHue = predominantColor + 120;
                secondColorHue = 360 - 120 + predominantColor;
            }
            SKColor firstColor = SKColor.FromHsv(firstColorHue, 100, 60);
            SKColor secondColor = SKColor.FromHsv(secondColorHue, 100, 60);
            for(int x = 0; x < originalImage.Width; x++)
            {
                for(int y = 0; y < originalImage.Height;y++)
                {
                    if(croppedImage.GetPixel(x,y).Alpha != 0)
                    {
                        firstImage.SetPixel(x, y, firstColor);
                        secondImage.SetPixel(x, y, secondColor);
                    }
                    else
                    {
                        firstImage.SetPixel(x, y, originalImage.GetPixel(x, y));
                        secondImage.SetPixel(x, y, originalImage.GetPixel(x, y));
                    }
                }
            }
            return new Tuple<SKBitmap, SKBitmap>(firstImage, secondImage);
        }
    }
}
