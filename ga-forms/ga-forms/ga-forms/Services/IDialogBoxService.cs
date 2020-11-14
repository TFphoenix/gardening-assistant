using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using ga_forms.Common.Enums;

namespace ga_forms.Services
{
    public interface IDialogBoxService
    {
        Dictionary<DialogBoxType, ActionSheetConfig> ActionSheetConfigs { get; set; }

        void DisplayDialogBox(DialogBoxType dialogBox);

        void InitHealthResultsSave(Action onNewPlant, Action onExistingPlant, Action onCancel);
    }
}
