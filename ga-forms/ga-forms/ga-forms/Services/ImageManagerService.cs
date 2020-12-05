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
        public SKPath HealthSelectionPath { get; set; }
    }
}
