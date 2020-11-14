using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ga_forms.Services
{
    public interface IImageManagerService
    {
        ImageSource HealthInitialImage { get; set; }
    }
}
