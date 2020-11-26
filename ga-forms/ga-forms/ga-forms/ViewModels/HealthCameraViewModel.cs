using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ga_forms.Services;
using ga_forms.Views;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class HealthCameraViewModel : ViewModel
    {
        private readonly IImageManagerService _imageManagerService;

        public Command GoBackCommand { get; }
        public Command UploadCommand { get; }

        public HealthCameraViewModel(IImageManagerService imageManagerService)
        {
            _imageManagerService = imageManagerService;

            Title = "Health Camera Page";
            GoBackCommand = new Command(OnBack);
            UploadCommand = new Command(UploadImage);
        }

        public async void OnSnapshot(ImageSource image, string path, byte[] imageData)
        {
            _imageManagerService.HealthInitialImage = image;

            await Shell.Current.GoToAsync($"//{nameof(HealthSelectionPage)}");
        }

        private void UploadImage(object obj)
        {
            //TODO
        }

        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}
