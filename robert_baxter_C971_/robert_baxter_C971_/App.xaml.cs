using robert_baxter_C971_.Views;
using Xamarin.Forms;

namespace robert_baxter_C971_
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var dashboard = new Dashboard();
            var nav = new NavigationPage(dashboard);

            MainPage = nav;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
