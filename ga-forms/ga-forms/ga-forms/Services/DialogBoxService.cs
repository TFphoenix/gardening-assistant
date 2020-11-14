using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using ga_forms.Common.Enums;

namespace ga_forms.Services
{
    public class DialogBoxService : IDialogBoxService
    {
        // Dialog Boxes definitions
        public Dictionary<DialogBoxType, IDialogBox> DialogBoxes { get; set; }

        public class HealthResultsSave : IDialogBox
        {
            public ActionSheetConfig Config { get; set; }
            public DialogBoxType Type => DialogBoxType.HealthResultsSave;

            public HealthResultsSave(Action onNewPlant, Action onExistingPlant, Action onCancel)
            {
                Config = new ActionSheetConfig();

                List<ActionSheetOption> options = new List<ActionSheetOption>
                {
                    new ActionSheetOption("New Plant", new Action(onNewPlant), "new_plant.png"),
                    new ActionSheetOption("Existing Plant", new Action(onExistingPlant), "tab_plants.png"),
                    new ActionSheetOption("Cancel", new Action(onCancel), "cancel.png")
                };

                Config.Options = options;
                Config.Title = "Save To";
            }
        }

        // Ctor
        public DialogBoxService()
        {
            DialogBoxes = new Dictionary<DialogBoxType, IDialogBox>();
        }

        // Init
        public void InitDialogBox<DialogBoxT>(DialogBoxT dialogBox) where DialogBoxT : IDialogBox
        {
            DialogBoxes.Add(dialogBox.Type, dialogBox);
        }

        // Display
        public void DisplayDialogBox(DialogBoxType dialogBox)
        {
            UserDialogs.Instance.ActionSheet(DialogBoxes[dialogBox].Config);
        }
    }
}
