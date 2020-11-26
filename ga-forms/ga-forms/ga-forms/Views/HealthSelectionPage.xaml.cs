using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ga_forms.Common;
using ga_forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace ga_forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthSelectionPage : ContentPage
    {
        // Visual components
        private SKBitmap _backgroundBitmap;

        // Ctor
        public HealthSelectionPage()
        {
            InitializeComponent();
            BindingContext = DependencyInjectionManager.ServiceProvider.GetService<HealthSelectionViewModel>();
            DoneButton.Margin = new Thickness(0, 0, 0, 30);

            InitializePaints();
            ImportHealthImage();
        }

        // Initialize paints
        private void InitializePaints()
        {
            //...
        }

        // Import health image
        private void ImportHealthImage()
        {
            try
            {
                _backgroundBitmap =
                    BitmapExtensions.LoadBitmapResource(typeof(HealthSelectionPage),
                        "ga-forms.Resources.leaf_test.jpg");
            }
            catch (Exception e)
            {
                //TODO: Handle exceptions by displaying a dialog box and redirecting the user to HealthCameraPage
                Console.WriteLine(e);
            }
        }

        // Canvas refresh
        private void CanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKRect destination = new SKRect(0, 0, info.Width, info.Height);

            canvas.DrawBitmap(_backgroundBitmap, destination, BitmapStretch.Uniform);
        }

        // Canvas touch
        private void CanvasView_OnTouch(object sender, SKTouchEventArgs args)
        {
            //TODO
        }
    }
}