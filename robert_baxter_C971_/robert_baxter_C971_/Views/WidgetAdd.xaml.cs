using robert_baxter_C971_.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WidgetAdd : ContentPage
    {
        private int _gadgetId;

        public WidgetAdd()
        {
            InitializeComponent();
        }

        public WidgetAdd(int gadgetId)
        {
            InitializeComponent();

            _gadgetId = gadgetId;
        }

        private async void SaveWidget_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(WidgetName.Text))
            {
                await DisplayAlert("Missing Name", "Please enter a name", "Ok");
                return;
            }

            if (string.IsNullOrWhiteSpace(WidgetColorPicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Coor", "Please select a color", "Ok");
                return;
            }

            if (!int.TryParse(WidgetsInStock.Text, out int actualStock))
            {
                await DisplayAlert("Missing Stock", "Please enter a whole number", "Ok");
                return;
            }

            if (!decimal.TryParse(WidgetPrice.Text, out decimal actualPrice))
            {
                await DisplayAlert("Missing Price", "Please enter a number", "Ok");
                return;
            }

            await DatabaseService.AddWidget(
                _gadgetId,
                WidgetName.Text,
                WidgetColorPicker.SelectedItem.ToString(),
                actualStock,
                actualPrice,
                CreationDatePicker.Date,
                Notification.IsToggled,
                NotesEditer.Text);

            await Navigation.PopAsync();
        }

        private async void CancelWidget_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Home_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}