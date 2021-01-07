using Acr.UserDialogs;
using ga_forms.Services;
using ga_forms.Views;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class DecorateSelectionViewModel:ViewModel
    {
        // Commands
        public Command GoBackCommand { get; }
        public Command DoneCommand { get; }
        public Command AutoBackgroundCommand { get; }

        // Services
        private readonly IImageManagerService _imageManagerService;

        // Properties
        public SKPath SelectionPath { get; set; }
        public SKBitmap SelectionBitmap { get; set; }

        // Ctor
        public DecorateSelectionViewModel(IImageManagerService imageManagerService)
        {
            Title = "Decorate Selection Page";

            GoBackCommand = new Command(OnBack);
            DoneCommand = new Command(OnDone);
            AutoBackgroundCommand = new Command(OnAutoBackground);

            _imageManagerService = imageManagerService;
        }

        public SKBitmap GetCapturedImageBitmap() => _imageManagerService.HealthInitialImageBitmap;

        private void OnAutoBackground(object obj)
        {
            //TODO
        }

        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(DecoratePage)}");
        }

        private async void OnDone(object obj)
        {
            if (SelectionPath == null || SelectionBitmap == null)
            {
                //TODO: Display dialog box that you first need to select a path
                return;
            }

            _imageManagerService.HealthSelectionPath = SelectionPath;
            _imageManagerService.HealthSelectionImageBitmap = SelectionBitmap;

            UserDialogs.Instance.ShowLoading("Processing...", MaskType.Black);
            await Task.Delay(1);
            await Shell.Current.GoToAsync($"//{nameof(HealthResultsPage)}").ContinueWith((task) => { UserDialogs.Instance.HideLoading(); });
        }
    }
}
