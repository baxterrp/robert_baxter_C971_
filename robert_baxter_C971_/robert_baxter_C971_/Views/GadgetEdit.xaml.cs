using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GadgetEdit : ContentPage
    {
        private int _selectedGadgetId;

        public GadgetEdit(Gadget gadget)
        {
            InitializeComponent();

            _selectedGadgetId = gadget.Id;

            GadgetId.Text = gadget.Id.ToString();
            GadgetName.Text = gadget.Name;
            GadgetColorPicker.SelectedItem = gadget.Color;
            GadgetsInStock.Text = gadget.InStock.ToString();
            GadgetPrice.Text = gadget.Price.ToString();
            CreationDatePicker.Date = gadget.CreationDate;
        }

        public GadgetEdit()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            WidgetCollectionView.ItemsSource = await DatabaseService.GetWidgets(_selectedGadgetId);
        }

        private async void SaveGadget_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GadgetName.Text))
            {
                await DisplayAlert("Missing Name", "Please enter a name.", "Ok");
                return;
            }

            if (string.IsNullOrWhiteSpace(GadgetColorPicker.SelectedItem?.ToString()))
            {
                await DisplayAlert("Missing Color", "Please select a color.", "Ok");
                return;
            }

            if (!int.TryParse(GadgetsInStock.Text, out int actualStock))
            {
                await DisplayAlert("Incorrect Inventory Value", "Please enter a whole number.", "Ok");
                return;
            }

            if (!decimal.TryParse(GadgetPrice.Text, out decimal actualPrice))
            {
                await DisplayAlert("Incorrect Price Value", "Please enter a number.", "Ok");
                return;
            }

            await DatabaseService.UpdateGadget(
                int.Parse(GadgetId.Text),
                GadgetName.Text,
                GadgetColorPicker.SelectedItem.ToString(),
                actualStock,
                actualPrice,
                CreationDatePicker.Date);

            await Navigation.PopAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Delete Gadget and Related Widgets?", "Delete this Gadget?", "Yes", "No"))
            {
                await DatabaseService.RemoveGadget(int.Parse(GadgetId.Text));
                await DisplayAlert("Gadget Deleted", "Gadget Deleted", "Ok");
            }
            else
            {
                await DisplayAlert("Deleted Canceled", "Nothing Deleted", "Ok");
            }

            await Navigation.PopAsync();
        }

        private async void AddWidget_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WidgetAdd(int.Parse(GadgetId.Text)));
        }

        private async void WidgetCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var widget = (Widget)e.CurrentSelection?.FirstOrDefault();

            if(widget != null)
            {
                await Navigation.PushAsync(new WidgetEdit(widget));
            }
        }
    }
}