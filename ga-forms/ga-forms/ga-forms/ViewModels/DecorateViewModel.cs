using ga_forms.Common;
using ga_forms.Services;
using ga_forms.Views;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class DecorateViewModel : ViewModel
    {
        private readonly IImageManagerService _imageManagerService;

        public Command GoBackCommand { get; }
        public Command UploadCommand { get; }
        
        public DecorateViewModel(IImageManagerService imageManagerService)
        {
            _imageManagerService = imageManagerService;
            Title = "Health Camera Page";
            GoBackCommand = new Command(OnBack);
            UploadCommand = new Command(UploadImage);
        }

        public async void OnSnapshot(string path)
        {
            // Set Bitmap
            //SKBitmap rotatedBitmap;
            //using (var bitmap = SKBitmap.Decode(path))
            //{
            //    rotatedBitmap = new SKBitmap(bitmap.Height, bitmap.Width);

            //    using (var surface = new SKCanvas(rotatedBitmap))
            //    {
            //        surface.Translate(rotatedBitmap.Width, 0);
            //        surface.RotateDegrees(90);
            //        surface.DrawBitmap(bitmap, 0, 0);
            //    }
            //}

            //_imageManagerService.HealthInitialImageBitmap = rotatedBitmap;
            _imageManagerService.HealthInitialImageBitmap = BitmapExtensions.RotateBitmap(path, 90);

            await Shell.Current.GoToAsync($"//{nameof(HealthSelectionPage)}");
        }

        private async void UploadImage(object obj)
        {
            IPhotoLibrary photoLibrary = DependencyService.Get<IPhotoLibrary>();
            using (Stream stream = await photoLibrary.PickPhotoAsync())
            {
                if (stream != null)
                {
                    // Set Bitmap
                    _imageManagerService.HealthInitialImageBitmap = SKBitmap.Decode(stream);

                    await Shell.Current.GoToAsync($"//{nameof(HealthSelectionPage)}");
                }
            }
        }

        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}
