using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;

namespace ga_forms.Services
{
    public interface IImageManagerService
    {
        SKBitmap HealthInitialImageBitmap { get; set; }
    }
}
