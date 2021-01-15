using ga_forms.Common;
using ga_forms.TouchTracking;
using ga_forms.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
        private SKPaint _selectionPaintOutline;
        private SKPaint _selectionPaintFill;
        private SKPaint _completedSelectionFill;

        // Members
        private bool _canDraw = true;
        private readonly HealthSelectionViewModel _viewModel;
        private static readonly float RESCALE_FACTOR = 0.25f;

        // Ctor
        public HealthSelectionPage()
        {
            InitializePaints();
            InitializeComponent();
            BindingContext = DependencyInjectionManager.ServiceProvider.GetService<HealthSelectionViewModel>();
            _viewModel = (HealthSelectionViewModel)BindingContext;
            DoneButton.Margin = new Thickness(0, 0, 0, 30);
        }

        // On page appearing life hook
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ImportHealthImage();
        }

        // On page disappearing life hook
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UndoSelection();
        }

        // Initialize paints
        private void InitializePaints()
        {
            _selectionPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Constants.SelectionStrokeColor,
                StrokeWidth = Constants.SelectionStrokeWidth,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
                PathEffect = SKPathEffect.CreateDash(new float[]
                {
                    Constants.SelectionPathDashLength, Constants.SelectionPathGapLength
                }, Constants.SelectionPathPhase)
            };

            _selectionPaintOutline = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Constants.SelectionStrokeOutlineColor,
                StrokeWidth = Constants.SelectionStrokeOutlineWidth,
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
                Color = Constants.SelectionFillColor
            };

            _completedSelectionFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White
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
            if (!_canDraw) return;

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
                        // auto-close path
                        SKPath path = _inProgressPaths[args.Id];
                        path.LineTo(_inProgressPaths[args.Id].GetPoint(0));

                        // save path
                        _viewModel.SelectionPath = _inProgressPaths[args.Id];
                        _viewModel.SelectionBitmap = GetCompletedSelectionBitmap(_inProgressPaths[args.Id]);
                        _completedPaths.Add(_inProgressPaths[args.Id]);
                        _inProgressPaths.Remove(args.Id);

                        UpdateBitmap();
                    }

                    _canDraw = false;
                    break;

                case TouchActionType.Cancelled:
                    if (_inProgressPaths.ContainsKey(args.Id))
                    {
                        _inProgressPaths.Remove(args.Id);

                        UpdateBitmap();
                    }

                    _canDraw = false;
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
                    saveBitmapCanvas.DrawPath(path, _selectionPaintOutline);
                    saveBitmapCanvas.DrawPath(path, _selectionPaint);
                }

                foreach (SKPath path in _inProgressPaths.Values)
                {
                    saveBitmapCanvas.DrawPath(path, _selectionPaintFill);
                    saveBitmapCanvas.DrawPath(path, _selectionPaintOutline);
                    saveBitmapCanvas.DrawPath(path, _selectionPaint);
                }
            }

            CanvasView.InvalidateSurface();
        }

        // Get selection bitmap
        SKBitmap GetCompletedSelectionBitmap(SKPath selectionPath)
        {
            SKBitmap completedSelectionBitmap = new SKBitmap(_selectionBitmap.Width, _selectionBitmap.Height);

            // Resulting image (1440 x 1881)
            using (SKCanvas completedSelectionCanvas = new SKCanvas(completedSelectionBitmap))
            {
                completedSelectionCanvas.Clear(SKColors.Black);
                completedSelectionCanvas.DrawPath(selectionPath, _completedSelectionFill);
            }

            // Rescaled image (360 x 470)
            completedSelectionBitmap = completedSelectionBitmap
                .Resize(new SKSizeI(
                    (int)(completedSelectionBitmap.Width * RESCALE_FACTOR),
                    (int)(completedSelectionBitmap.Height * RESCALE_FACTOR)),
                    SKFilterQuality.Medium);
            return completedSelectionBitmap;
        }

        // Undo selection event
        private void Undo_OnClicked(object sender, EventArgs e)
        {
            UndoSelection();
        }

        // Undo selection
        private void UndoSelection()
        {
            _viewModel.SelectionPath = null;
            _viewModel.SelectionBitmap = null;
            _completedPaths.Clear();
            _inProgressPaths.Clear();
            UpdateBitmap();
            CanvasView.InvalidateSurface();

            _canDraw = true;
        }

        // Save functionality (currently unnecessary)
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