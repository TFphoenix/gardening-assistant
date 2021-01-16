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
        private readonly DominantColorsDetector _dominantColorsDetector = new DominantColorsDetector();

        ImageSource _firstImageSource;
        public ImageSource FirstImageSource
        {
            get => _firstImageSource;
            set => SetProperty(ref _firstImageSource, value);
        }

        ImageSource _secondImageSource;
        public ImageSource SecondImageSource
        {
            get => _secondImageSource;
            set => SetProperty(ref _secondImageSource, value);
        }

        ImageSource _thirdImageSource;
        public ImageSource ThirdImageSource
        {
            get => _thirdImageSource;
            set => SetProperty(ref _thirdImageSource, value);
        }

        ImageSource _fourthImageSource;
        public ImageSource FourthImageSource
        {
            get => _fourthImageSource;
            set => SetProperty(ref _fourthImageSource, value);
        }

        Tuple<ImageSource, ImageSource, ImageSource> _dominantColors;
        public Tuple<ImageSource, ImageSource, ImageSource> DominantColors
        {
            get => _dominantColors;
            set => SetProperty(ref _dominantColors, value);
        }

        public Command GoBackCommand { get; }
        public Command GoHomeCommand { get; }

        public DecorateResultsViewModel(IImageManagerService imageManagerService)
        {
            // General
            _imageManagerService = imageManagerService;
            Title = "Decorate Results Page";

            // Commands
            GoBackCommand = new Command(OnBack);
            GoHomeCommand = new Command(OnHome);

            // Image Sources
            _firstImageSource = "";
            _secondImageSource = "";
            _thirdImageSource = "";
            _fourthImageSource = "";
            _dominantColors = new Tuple<ImageSource, ImageSource, ImageSource>("", "", "");
        }

        public void DisplayImages()
        {
            // Determine dominant colors
            _dominantColorsDetector.ProcessingImage = _imageManagerService.DecorateInitialImageBitmap;
            _dominantColorsDetector.Execute();

            // Generate decorate images
            var decorateImages = _imageManagerService.GetDecorateImages(
                _imageManagerService.GetDecorateSelectedBitmap(),
                _dominantColorsDetector.DominantColors);

            // Bind decorate images
            FirstImageSource = BitmapExtensions.GetImageFromBitmap(_imageManagerService.DecorateInitialImageBitmap).Source;
            SecondImageSource = BitmapExtensions.GetImageFromBitmap(decorateImages.Item1).Source;
            ThirdImageSource = BitmapExtensions.GetImageFromBitmap(decorateImages.Item2).Source;
            FourthImageSource = BitmapExtensions.GetImageFromBitmap(decorateImages.Item3).Source;

            // Bind dominant colors
            SKBitmap dominantBitmap1 = new SKBitmap(240, 120);
            SKBitmap dominantBitmap2 = new SKBitmap(240, 120);
            SKBitmap dominantBitmap3 = new SKBitmap(240, 120);
            dominantBitmap1.Erase(_dominantColorsDetector.DominantColors[0]);
            dominantBitmap2.Erase(_dominantColorsDetector.DominantColors[1]);
            dominantBitmap3.Erase(_dominantColorsDetector.DominantColors[2]);
            DominantColors = new Tuple<ImageSource, ImageSource, ImageSource>
            (
                BitmapExtensions.GetImageFromBitmap(dominantBitmap1).Source,
                BitmapExtensions.GetImageFromBitmap(dominantBitmap2).Source,
                BitmapExtensions.GetImageFromBitmap(dominantBitmap3).Source
            );
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
