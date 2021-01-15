using Acr.UserDialogs;
using ga_forms.Common;
using ga_forms.Common.Enums;
using ga_forms.Models;
using ga_forms.Models.ImageProcessing;
using ga_forms.Models.ImageProcessing.Algorithms;
using ga_forms.Services;
using ga_forms.Views;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class HealthResultsViewModel : ViewModel
    {
        // Diseases
        private ObservableCollection<DiseaseInfo> _diseases;

        // Algorithms Pipelines
        private AlgorithmsPipeline _blackSpotsPipeline;
        private AlgorithmsPipeline _greyscalePipeline;

        // Services
        private readonly IDialogBoxService _dialogBoxService;
        private readonly IImageManagerService _imageManagerService;

        //ctor
        public HealthResultsViewModel(IDialogBoxService dialogBoxService, IImageManagerService imageManagerService)
        {
            _imageManagerService = imageManagerService;
            _dialogBoxService = dialogBoxService;
            _dialogBoxService.InitDialogBox(new DialogBoxService.HealthResultsSave(OnNewPlant, OnExistingPlant, OnCancel));
            InitializePipelines();

            DiseasesCollection = new ObservableCollection<DiseaseInfo> { new DiseaseInfo("Black Spots", "Details about disease 1", DiseaseResultType.Ok),
                                                                         new DiseaseInfo("Disease2", "Details about disease 2", DiseaseResultType.Warning),
                                                                         new DiseaseInfo("Disease3", "Details about disease 3", DiseaseResultType.Error),
                                                                         new DiseaseInfo("Disease3", "Details about disease 3", DiseaseResultType.Error),
                                                                         new DiseaseInfo("Disease3", "Details about disease 3", DiseaseResultType.Error),
                                                                         new DiseaseInfo("Disease3", "Details about disease 3", DiseaseResultType.Error),
                                                                         new DiseaseInfo("Disease3", "Details about disease 3", DiseaseResultType.Error),
                                                                         new DiseaseInfo("Disease3", "Details about disease 3", DiseaseResultType.Error)};
            Title = "Health Results Page";
            GoBackCommand = new Command(OnBack);
            GoHomeCommand = new Command(OnHome);
            SaveCommand = new Command(OnSave);
        }

        public void PopulateResults()
        {
            // Black Spots
            double percentage = System.Math.Round(_imageManagerService.GetDiseasePercentage(_imageManagerService.GetHealthSelectedBitmap(), _blackSpotsPipeline.ResultImage), 2);
            DiseasesCollection[0].ImgSource = BitmapExtensions.GetImageFromBitmap(_blackSpotsPipeline.ResultImage).Source;
            if (percentage < 10.0)
            {
                DiseasesCollection[0].DiseaseResult = DiseaseResultType.Ok;
                DiseasesCollection[0].Details = "Your plant is in a good state.\nPrevention for black spots disease:\n1. Baking soda spray\n2. Neem oil\n3. Sulfur";
            }
            else if (percentage > 10.0 && percentage < 20.0)
            {
                DiseasesCollection[0].DiseaseResult = DiseaseResultType.Warning;
                DiseasesCollection[0].Details = "Your plant seems to become affected by black spots disease.\n1. Provide good air circulation around and through your plant\n2. Remove any infected leaves.\n";
            }
            else if (percentage > 20.0)
            {
                DiseasesCollection[0].DiseaseResult = DiseaseResultType.Error;
                DiseasesCollection[0].Details = "Your plant is seriously affected. You need to be very careful!\n1. Provide good air circulation around and through your plant\n2. Avoid getting the leaves wet while watering.\n3. Remove any infected leaves.";
            }
            DiseasesCollection[0].Percentage = "Severity:\n" + percentage + "%";

            // Grayscale
            DiseasesCollection[1].ImgSource =
                BitmapExtensions.GetImageFromBitmap(_greyscalePipeline.ResultImage).Source;

            // Refresh Diseases GUI
            DiseasesCollection = new ObservableCollection<DiseaseInfo>(DiseasesCollection);
        }

        private void InitializePipelines()
        {
            // Grayscale pipeline
            _greyscalePipeline = new AlgorithmsPipeline
            (
                new List<IAlgorithm>
                {
                    new GrayscaleConvertor()
                }
            );

            // Black spots pipeline
            _blackSpotsPipeline = new AlgorithmsPipeline
            (
                new List<IAlgorithm>
                {
                    new GrayscaleConvertor(),
                    //new GaussFilter()
                    new Otsu()
                }
            );
        }

        public void StartProcessing()
        {
            // set selected bitmap
            SKBitmap healthSelectedBitmap = _imageManagerService.GetHealthSelectedBitmap();

            // set pipelines initial images
            _blackSpotsPipeline.InitialImage = healthSelectedBitmap;
            _greyscalePipeline.InitialImage = healthSelectedBitmap;

            // execute pipelines
            _blackSpotsPipeline.ExecutePipeline();
            _greyscalePipeline.ExecutePipeline();

            // display header images
            ProcessingImageSource = BitmapExtensions.GetImageFromBitmap(_imageManagerService.HealthInitialImageBitmap).Source;
            ProcessedImageSource = BitmapExtensions.GetImageFromBitmap(healthSelectedBitmap).Source;
        }


        public ObservableCollection<DiseaseInfo> DiseasesCollection
        {
            get { return _diseases; }
            set { SetProperty(ref _diseases, value); }
        }

        public Command GoBackCommand { get; }
        public Command GoHomeCommand { get; }
        public Command SaveCommand { get; }

        ImageSource processingImageSource = "";
        public ImageSource ProcessingImageSource
        {
            get { return processingImageSource; }
            set { SetProperty(ref processingImageSource, value); }
        }

        ImageSource processedImageSource = "";
        public ImageSource ProcessedImageSource
        {
            get { return processedImageSource; }
            set { SetProperty(ref processedImageSource, value); }
        }

        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HealthSelectionPage)}");
        }
        private async void OnHome(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        private void OnNewPlant()
        {
            //TODO
        }

        private void OnExistingPlant()
        {
            //TODO
        }

        private void OnCancel()
        {
            //TODO
        }

        private async void OnSave(object obj)
        {
            using (var progress = UserDialogs.Instance.Progress("Saving..."))
            {
                for (var i = 0; i < 100; i++)
                {
                    progress.PercentComplete = i;

                    await Task.Delay(20);
                }
            }

            _dialogBoxService.DisplayDialogBox(DialogBoxType.HealthResultsSave);

        }
    }
}
