using ga_forms.Common;
using ga_forms.Models.ImageProcessing.Algorithms;
using ga_forms.Services;
using ga_forms.Views;
using System;
using System.Collections.Generic;
using System.Text;
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
        public Command GoBackCommand { get; }
        public Command GoHomeCommand { get; }

        public DecorateResultsViewModel(IImageManagerService imageManagerService)
        {
            _imageManagerService = imageManagerService;
            Title = "Health Results Page";
            GoBackCommand = new Command(OnBack);
            GoHomeCommand = new Command(OnHome);
        }

        public void DisplayImages()
        {
            _hsvConvertor.ProcessingImage = _imageManagerService.DecorateInitialImageBitmap;
            _hsvConvertor.Execute();
            FirstImageSource = BitmapExtensions.GetImageFromBitmap(_imageManagerService.DecorateInitialImageBitmap).Source;
            SecondImageSource = BitmapExtensions.GetImageFromBitmap(_imageManagerService.GetTriadImages(_imageManagerService.DecorateInitialImageBitmap, _imageManagerService.GetDecorateSelectedBitmap(), _hsvConvertor.GetPredominantColor()).Item1).Source;
            ThirdImageSource = BitmapExtensions.GetImageFromBitmap(_imageManagerService.GetTriadImages(_imageManagerService.DecorateInitialImageBitmap, _imageManagerService.GetDecorateSelectedBitmap(), _hsvConvertor.GetPredominantColor()).Item2).Source;
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
