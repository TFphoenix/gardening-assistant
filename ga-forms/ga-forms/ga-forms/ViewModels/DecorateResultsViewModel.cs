using ga_forms.Common;
using ga_forms.Models.ImageProcessing.Algorithms;
using ga_forms.Services;
using ga_forms.Views;
using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class DecorateResultsViewModel : ViewModel
    {
        private readonly IImageManagerService _imageManagerService;
        private HsvConvertor _hsvConvertor = new HsvConvertor();

        ImageSource firstImageSource = "";
        public ImageSource FirstImageSource
        {
            get { return firstImageSource; }
            set { SetProperty(ref firstImageSource, value); }
        }

        ImageSource secondImageSource = "";
        public ImageSource SecondImageSource
        {
            get { return secondImageSource; }
            set { SetProperty(ref secondImageSource, value); }
        }

        ImageSource thirdImageSource = "";
        public ImageSource ThirdImageSource
        {
            get { return thirdImageSource; }
            set { SetProperty(ref thirdImageSource, value); }
        }

        ImageSource fourthImageSource = "";
        public ImageSource FourthImageSource
        {
            get { return fourthImageSource; }
            set { SetProperty(ref fourthImageSource, value); }
        }
        public Command GoBackCommand { get; }
        public Command GoHomeCommand { get; }

        public DecorateResultsViewModel(IImageManagerService imageManagerService)
        {
            _imageManagerService = imageManagerService;
            Title = "Decorate Results Page";

            GoBackCommand = new Command(OnBack);
            GoHomeCommand = new Command(OnHome);
        }

        public void DisplayImages()
        {
            // Determine predominant color
            _hsvConvertor.ProcessingImage = _imageManagerService.DecorateInitialImageBitmap;
            _hsvConvertor.Execute();

            // Generate decorate images
            var decorateImages = _imageManagerService.GetDecorateImages(
                _imageManagerService.GetDecorateSelectedBitmap(),
                _hsvConvertor.PredominantHue);

            // Bind decorate images
            FirstImageSource = BitmapExtensions.GetImageFromBitmap(_imageManagerService.DecorateInitialImageBitmap).Source;
            SecondImageSource = BitmapExtensions.GetImageFromBitmap(decorateImages.Item1).Source;
            ThirdImageSource = BitmapExtensions.GetImageFromBitmap(decorateImages.Item2).Source;
            FourthImageSource = BitmapExtensions.GetImageFromBitmap(decorateImages.Item3).Source;
        }
        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(DecorateSelectionPage)}");
        }
        private async void OnHome(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}
