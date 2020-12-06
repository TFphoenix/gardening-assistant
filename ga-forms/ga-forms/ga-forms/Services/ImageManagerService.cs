using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;

namespace ga_forms.Services
{
    public class ImageManagerService : IImageManagerService
    {
        public SKBitmap HealthInitialImageBitmap { get; set; }
        public SKBitmap HealthSelectionImageBitmap { get; set; }
        public SKPath HealthSelectionPath { get; set; }
        public SKBitmap GetHealthSelectedBitmap() 
        {
            SKBitmap selectedBitmap = new SKBitmap(HealthInitialImageBitmap.Width, HealthInitialImageBitmap.Height);
            for(int x = 0; x < HealthInitialImageBitmap.Width; ++x)
            {
                for (int y = 0; y < HealthInitialImageBitmap.Height; ++y)
                {
                    if(HealthSelectionImageBitmap.GetPixel(x,y).Red == 0)
                    {
                        selectedBitmap.SetPixel(x,y, new SKColor(0,0,0,0));
                    }
                    else
                    {
                        selectedBitmap.SetPixel(x, y, HealthInitialImageBitmap.GetPixel(x, y));
                    }
                }
            }
            return selectedBitmap;
        }
    }
}
