using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using ga_forms.Common.Enums;

namespace ga_forms.Services
{
    public class DialogBoxService : IDialogBoxService
    {
        public Dictionary<DialogBoxType, ActionSheetConfig> ActionSheetConfigs { get; set; }

        public DialogBoxService()
        {
            ActionSheetConfigs = new Dictionary<DialogBoxType, ActionSheetConfig>();
        }

        public void DisplayDialogBox(DialogBoxType dialogBox)
        {
            UserDialogs.Instance.ActionSheet(ActionSheetConfigs[dialogBox]);
        }

        public void InitHealthResultsSave(Action onNewPlant, Action onExistingPlant, Action onCancel)
        {
            var config = new ActionSheetConfig();

            List<ActionSheetOption> options = new List<ActionSheetOption>
            {
                new ActionSheetOption("New Plant", new Action(onNewPlant), "new_plant.png"),
                new ActionSheetOption("Existing Plant", new Action(onExistingPlant), "tab_plants.png"),
                new ActionSheetOption("Cancel", new Action(onCancel), "cancel.png")
            };

            config.Options = options;
            config.Title = "Save To";

            ActionSheetConfigs.Add(
                DialogBoxType.HealthResultsSave,
                config);
        }
    }
}
