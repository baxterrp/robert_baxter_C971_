using Plugin.LocalNotifications;
using robert_baxter_C971_.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var widgets = await DatabaseService.GetWidgets();
            var id = 0;

            foreach(var widget in widgets.Where(w => w.StartNotification && DateTime.Today.Equals(w.CreationDate)).ToList())
            {
                try
                {
                    CrossLocalNotifications.Current.Show("Notice", $"{widget.Name} begins today!", id++);
                }
                catch (Exception exception) 
                {
                    await DisplayAlert("Whoops", $"Something happened while showing notifications! {exception.Message}", "Ok");
                }

            }
        }

        private async void AddGadget_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new GadgetAdd());
        }

        private async void ViewGadgets_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new GadgetList());
        }

        private async void Settings_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AppSettings());
        }
    }
}