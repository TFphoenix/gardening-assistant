using Acr.UserDialogs;
using ga_forms.Services;
using ga_forms.Views;
using SkiaSharp;
using System.Threading.Tasks;
using ga_forms.Common;
using ga_forms.Models.ImageProcessing.Algorithms;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class HealthSelectionViewModel : ViewModel
    {
        // Commands
        public Command GoBackCommand { get; }
        public Command DoneCommand { get; }

        // Services
        private readonly IImageManagerService _imageManagerService;

        // Properties
        public SKPath SelectionPath { get; set; }
        public SKBitmap SelectionBitmap { get; set; }

        // Ctor
        public HealthSelectionViewModel(IImageManagerService imageManagerService)
        {
            Title = "Health Selection Page";

            GoBackCommand = new Command(OnBack);
            DoneCommand = new Command(OnDone);

            _imageManagerService = imageManagerService;
        }

        public SKBitmap GetCapturedImageBitmap() => _imageManagerService.HealthInitialImageBitmap;

        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HealthCameraPage)}");
        }

        private async void OnDone(object obj)
        {
            if (SelectionBitmap == null)
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

        public SKBitmap AutoBackgroundSelection(SKBitmap bitmap)
        {
            ColorSegmentation colorSegmentation = new ColorSegmentation
            {
                ProcessingImage = bitmap,
                Color = bitmap.GetPixel(0, 0),
                EstimatedDistance = Constants.SelectionAutoBackgroundEstimatedDistance
            };

            colorSegmentation.Execute();

            return colorSegmentation.ProcessedImage;
        }

        public void ShowAutoBackgroundAlert()
        {
            UserDialogs.Instance.Alert("Your background was automatically removed", "Auto Background", "Ok");
        }
    }
}
