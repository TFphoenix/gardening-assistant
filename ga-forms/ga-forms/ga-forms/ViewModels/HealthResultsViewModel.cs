﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ga_forms.Common.Enums;
using ga_forms.Models;
using ga_forms.Services;
using ga_forms.Views;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class HealthResultsViewModel : ViewModel
    {
        private ObservableCollection<DiseaseInfo> _diseases;
        private readonly IDialogBoxService _dialogBoxService;

        public ObservableCollection<DiseaseInfo> DiseasesCollection
        {
            get { return _diseases; }
            set { SetProperty(ref _diseases, value); }
        }

        public Command GoBackCommand { get; }
        public Command GoHomeCommand { get; }
        public Command SaveCommand { get; }

        public HealthResultsViewModel(IDialogBoxService dialogBoxService)
        {
            DiseasesCollection = new ObservableCollection<DiseaseInfo> { new DiseaseInfo("Disease1", "user64.png", "Details about disease 1", DiseaseResultType.Ok),
                                                                         new DiseaseInfo("Disease2", "add64.png", "Details about disease 2", DiseaseResultType.Warning),
                                                                         new DiseaseInfo("Disease3", "tab_home.png", "Details about disease 3", DiseaseResultType.Error),
                                                                         new DiseaseInfo("Disease3", "tab_home.png", "Details about disease 3", DiseaseResultType.Error),
                                                                         new DiseaseInfo("Disease3", "tab_home.png", "Details about disease 3", DiseaseResultType.Error),
                                                                         new DiseaseInfo("Disease3", "tab_home.png", "Details about disease 3", DiseaseResultType.Error),
                                                                         new DiseaseInfo("Disease3", "tab_home.png", "Details about disease 3", DiseaseResultType.Error),
                                                                         new DiseaseInfo("Disease3", "tab_home.png", "Details about disease 3", DiseaseResultType.Error)};
            Title = "Health Results Page";
            GoBackCommand = new Command(OnBack);
            GoHomeCommand = new Command(OnHome);
            SaveCommand = new Command(OnSave);

            _dialogBoxService = dialogBoxService;
            _dialogBoxService.InitDialogBox(new DialogBoxService.HealthResultsSave(OnNewPlant, OnExistingPlant, OnCancel));
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
