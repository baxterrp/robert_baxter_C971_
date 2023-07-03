using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WidgetEdit : ContentPage
    {
        public WidgetEdit()
        {
            InitializeComponent();
        }

        public WidgetEdit(Widget widget)
        {
            InitializeComponent();

            WidgetId.Text = widget.Id.ToString();
            WidgetName.Text = widget.Name.ToString();
            WidgetColorPicker.SelectedItem = widget.Color;
            WidgetsInStock.Text = widget.InStock.ToString();
            WidgetPrice.Text = widget.Price.ToString();
            NotesEditer.Text = widget.Notes;
            CreationDatePicker.Date = widget.CreationDate;
            Notification.IsEnabled = widget.StartNotification;
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

            await DatabaseService.UpdateWidget(
                int.Parse(WidgetId.Text),
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

        private async void DeleteWidget_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Delete this widget?", "Delete this widget?", "Yes", "No"))
            {
                await DatabaseService.RemoveWidget(int.Parse(WidgetId.Text));
                await DisplayAlert("Widget Deleted", "Widget Deleted", "Ok");
            }
            else
            {
                await DisplayAlert("Delete Canceled", "Nothing Deleted", "Ok");
            }

            await Navigation.PopAsync();
        }

        private async void ShareButton_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = WidgetName.Text,
                Title = "Share Text",
            });
        }

        private async void ShareUri_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = "https://docs.microsoft.com/en-us/xamarin/essentials/share?tabs=android",
                Title = "Share Web Link",
            });
        }
    }
}