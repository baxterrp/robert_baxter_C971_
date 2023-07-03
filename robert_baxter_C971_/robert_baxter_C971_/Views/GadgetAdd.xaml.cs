using robert_baxter_C971_.Services;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GadgetAdd : ContentPage
    {
        public GadgetAdd()
        {
            InitializeComponent();
        }

        private async void SaveGadget_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GadgetName.Text))
            {
                await DisplayAlert("Missing Name", "Please enter a name", "Ok");
                return;
            }

            if (string.IsNullOrWhiteSpace(GadgetColorPicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Color", "Please enter a color", "Ok");
                return;
            }

            if (!int.TryParse(GadgetsInStock.Text, out int actualStock))
            {
                await DisplayAlert("Missing Inventory Value", "Please enter a whole number", "Ok");
                return;
            }

            if (!decimal.TryParse(GadgetPrice.Text, out decimal actualPrice))
            {
                await DisplayAlert("Missing Price", "Please enter a number", "Ok");
                return;
            }

            await DatabaseService.AddGadget(
                GadgetName.Text,
                GadgetColorPicker.SelectedItem.ToString(),
                actualStock,
                actualPrice,
                CreationDatePicker.Date);

            await Navigation.PopAsync();
        }

        private async void CancelGadget_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}