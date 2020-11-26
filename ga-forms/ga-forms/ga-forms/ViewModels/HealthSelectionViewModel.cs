﻿using System;
using System.Collections.Generic;
using System.Text;
using ga_forms.Services;
using ga_forms.Views;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class HealthSelectionViewModel : ViewModel
    {
        // Commands
        public Command GoBackCommand { get; }
        public Command DoneCommand { get; }
        public Command UndoCommand { get; }
        public Command AutoBackgroundCommand { get; }

        // Services
        private readonly IImageManagerService _imageManagerService;

        // Ctor
        public HealthSelectionViewModel(IImageManagerService imageManagerService)
        {
            Title = "Health Selection Page";
            GoBackCommand = new Command(OnBack);
            DoneCommand = new Command(OnDone);
            UndoCommand = new Command(OnUndo);
            AutoBackgroundCommand = new Command(OnAutoBackground);

            _imageManagerService = imageManagerService;
        }

        private void OnAutoBackground(object obj)
        {
            //TODO
        }

        private void OnUndo(object obj)
        {
            //TODO
        }

        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HealthCameraPage)}");
        }

        private async void OnDone(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HealthResultsPage)}");
        }
    }
}
