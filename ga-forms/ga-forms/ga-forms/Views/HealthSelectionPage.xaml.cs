using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ga_forms.Common;
using ga_forms.TouchTracking;
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
        // Bitmaps
        private SKBitmap _backgroundBitmap;
        private SKBitmap _selectionBitmap;

        // Paths
        private Dictionary<long, SKPath> _inProgressPaths = new Dictionary<long, SKPath>();
        private List<SKPath> _completedPaths = new List<SKPath>();

        // Paints
        private SKPaint _selectionPaint;
        private SKPaint _selectionPaintFill;

        // Ctor
        public HealthSelectionPage()
        {
            InitializePaints();
            InitializeComponent();
            BindingContext = DependencyInjectionManager.ServiceProvider.GetService<HealthSelectionViewModel>();
            DoneButton.Margin = new Thickness(0, 0, 0, 30);
        }

        // On page appearing life hook
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ImportHealthImage();
        }

        // Initialize paints
        private void InitializePaints()
        {
            _selectionPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Red,
                StrokeWidth = Constants.SelectionStrokeWidth,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
                PathEffect = SKPathEffect.CreateDash(new float[]
                {
                    Constants.SelectionPathDashLength, Constants.SelectionPathGapLength
                }, Constants.SelectionPathPhase)
            };

            _selectionPaintFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(80, 80, 80, 127)
            };
        }

        // Import health image
        private void ImportHealthImage()
        {
            try
            {
                _backgroundBitmap = (BindingContext as HealthSelectionViewModel)?.GetCapturedImageBitmap();
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
            // Properties
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            // Init
            SKRect backgroundDestination = new SKRect(0, 0, info.Width, info.Height);
            // Create bitmap the size of the display surface
            if (_selectionBitmap == null)
            {
                _selectionBitmap = new SKBitmap(info.Width, info.Height);
            }
            // Or create new bitmap for a new size of display surface
            else if (_selectionBitmap.Width < info.Width || _selectionBitmap.Height < info.Height)
            {
                SKBitmap newBitmap = new SKBitmap(Math.Max(_selectionBitmap.Width, info.Width),
                    Math.Max(_selectionBitmap.Height, info.Height));

                using (SKCanvas newCanvas = new SKCanvas(newBitmap))
                {
                    newCanvas.Clear();
                    newCanvas.DrawBitmap(_selectionBitmap, 0, 0);
                }

                _selectionBitmap = newBitmap;
            }

            // Draw
            canvas.Clear();
            canvas.DrawBitmap(_backgroundBitmap, backgroundDestination, BitmapStretch.Uniform);
            canvas.DrawBitmap(_selectionBitmap, 0, 0);
        }

        // Canvas touch
        private void CanvasView_OnTouch(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!_inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath();
                        path.MoveTo(ConvertToPixel(args.Location));
                        _inProgressPaths.Add(args.Id, path);
                        UpdateBitmap();
                    }
                    break;

                case TouchActionType.Moved:
                    if (_inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = _inProgressPaths[args.Id];
                        path.LineTo(ConvertToPixel(args.Location));
                        UpdateBitmap();
                    }
                    break;

                case TouchActionType.Released:
                    if (_inProgressPaths.ContainsKey(args.Id))
                    {
                        _completedPaths.Add(_inProgressPaths[args.Id]);
                        _inProgressPaths.Remove(args.Id);
                        UpdateBitmap();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (_inProgressPaths.ContainsKey(args.Id))
                    {
                        _inProgressPaths.Remove(args.Id);
                        UpdateBitmap();
                    }
                    break;
            }
        }

        // Convert touch to pixel location
        SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(CanvasView.CanvasSize.Width * pt.X / CanvasView.Width),
                (float)(CanvasView.CanvasSize.Height * pt.Y / CanvasView.Height));
        }

        // Draw updated bitmap
        void UpdateBitmap()
        {
            using (SKCanvas saveBitmapCanvas = new SKCanvas(_selectionBitmap))
            {
                saveBitmapCanvas.Clear();

                foreach (SKPath path in _completedPaths)
                {
                    saveBitmapCanvas.DrawPath(path, _selectionPaintFill);
                    saveBitmapCanvas.DrawPath(path, _selectionPaint);
                }

                foreach (SKPath path in _inProgressPaths.Values)
                {
                    saveBitmapCanvas.DrawPath(path, _selectionPaintFill);
                    saveBitmapCanvas.DrawPath(path, _selectionPaint);
                }
            }

            CanvasView.InvalidateSurface();
        }

        // Undo selection
        private void Undo_OnClicked(object sender, EventArgs e)
        {
            _completedPaths.Clear();
            _inProgressPaths.Clear();
            UpdateBitmap();
            CanvasView.InvalidateSurface();
        }

        //TODO: Undo functionality
        //void OnClearButtonClicked(object sender, EventArgs args)
        //{
        //    completedPaths.Clear();
        //    inProgressPaths.Clear();
        //    UpdateBitmap();
        //    canvasView.InvalidateSurface();
        //}

        //TODO: Save functionality
        //async void OnSaveButtonClicked(object sender, EventArgs args)
        //{
        //    using (SKImage image = SKImage.FromBitmap(saveBitmap))
        //    {
        //        SKData data = image.Encode();
        //        DateTime dt = DateTime.Now;
        //        string filename = String.Format("FingerPaint-{0:D4}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}{6:D3}.png",
        //            dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

        //        IPhotoLibrary photoLibrary = DependencyService.Get<IPhotoLibrary>();
        //        bool result = await photoLibrary.SavePhotoAsync(data.ToArray(), "FingerPaint", filename);

        //        if (!result)
        //        {
        //            await DisplayAlert("FingerPaint", "Artwork could not be saved. Sorry!", "OK");
        //        }
        //    }
        //}
    }
}