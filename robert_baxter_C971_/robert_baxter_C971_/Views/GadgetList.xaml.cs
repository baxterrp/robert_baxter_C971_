using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GadgetList : ContentPage
    {
        public GadgetList()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            GadgetCollectionView.ItemsSource = await DatabaseService.GetGadgets();
        }

        private async void GadgetCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                var gadget = (Gadget)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new GadgetEdit(gadget));
            }
        }
    }
}