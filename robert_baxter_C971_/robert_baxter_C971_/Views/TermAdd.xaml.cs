using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermAdd : ContentPage
    {
        public TermAdd()
        {
            InitializeComponent();
        }

        private async void SaveNewTerm_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TermTitle.Text))
            {
                await DisplayAlert("Error", "Please enter term title", "Ok");
                return;
            }

            if (EndDatePicker.Date < StartDatePicker.Date)
            {
                await DisplayAlert("Error", "End date can not precede start date", "Ok");
                return;
            }

            await DatabaseService.SaveNewTerm(new Term
            {
                Title = TermTitle.Text,
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date,
            });

            await DisplayAlert("Success", "New Term Added", "Ok");

            await Navigation.PopAsync();
        }

        private async void CancelCreateNewTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}