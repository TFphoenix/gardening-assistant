using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using ga_forms.Common.Enums;

namespace ga_forms.Services
{
    public interface IDialogBox
    {
        ActionSheetConfig Config { get; set; }
        DialogBoxType Type { get; }
    }

    public interface IDialogBoxService
    {
        Dictionary<DialogBoxType, IDialogBox> DialogBoxes { get; set; }

        void InitDialogBox<DialogBoxT>(DialogBoxT dialogBox) where DialogBoxT : IDialogBox;
        void DisplayDialogBox(DialogBoxType dialogBox);
    }
}
