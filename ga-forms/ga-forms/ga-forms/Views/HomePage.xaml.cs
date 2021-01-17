using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ga_forms.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            CopyrightLabel.Text = "Copyright © 2021 Gabriela Burtan & Teodor Mihăescu\nTransylvania University of Brașov";
        }
    }
}
