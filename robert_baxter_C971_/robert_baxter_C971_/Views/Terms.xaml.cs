using Plugin.LocalNotifications;
using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System.Linq;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Terms : ContentPage
    {
        private Term _term;

        public Terms()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await InitializeTerms();
            await InitializeCourses();
            await InitializeAssessments();
        }

        private async Task InitializeAssessments()
        {
            var assessments = await DatabaseService.GetAllAssessments();
            var notificationId = 0;

            foreach (var assessment in assessments.Where(w => DateTime.Today.Equals(w.StartDate) || DateTime.Today.Equals(w.EndDate)).ToList())
            {
                try
                {
                    if (DateTime.Today.Equals(assessment.StartDate))
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{assessment.Name} begins today!", notificationId++);
                    }
                    else
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{assessment.Name} ends today!", notificationId++);
                    }
                }
                catch (Exception exception)
                {
                    await DisplayAlert("Whoops", $"Something happened while showing notification for assessment {assessment.Name}! {exception.Message}", "Ok");
                }
            }
        }

        private async Task InitializeCourses()
        {
            var courses = await DatabaseService.GetAllCourses();
            var notificationId = 0;

            foreach (var course in courses.Where(w => DateTime.Today.Equals(w.StartDate) || DateTime.Today.Equals(w.EndDate)).ToList())
            {
                try
                {
                    if (DateTime.Today.Equals(course.StartDate))
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{course.Name} begins today!", notificationId++);
                    }
                    else
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{course.Name} ends today!", notificationId++);
                    }
                }
                catch (Exception exception)
                {
                    await DisplayAlert("Whoops", $"Something happened while showing notification for course {course.Name}! {exception.Message}", "Ok");
                }
            }
        }

        private async Task InitializeTerms()
        {
            var terms = await DatabaseService.GetAllTerms();
            TermsView.ItemsSource = terms;

            var notificationId = 0;

            foreach (var term in terms.Where(w => DateTime.Today.Equals(w.StartDate) || DateTime.Today.Equals(w.EndDate)).ToList())
            {
                try
                {
                    if (DateTime.Today.Equals(term.StartDate))
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{term.Title} begins today!", notificationId++);
                    }
                    else
                    {
                        CrossLocalNotifications.Current.Show("Notice", $"{term.Title} ends today!", notificationId++);
                    }
                }
                catch (Exception exception)
                {
                    await DisplayAlert("Whoops", $"Something happened while showing notification for term {term.Title}! {exception.Message}", "Ok");
                }
            }
        }

        private async void TermsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _term = TermsView.SelectedItem as Term;
            await Navigation.PushAsync(new TermEdit(_term));
        }

        private async void AddNewTerm_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new TermAdd());
        }
    }
}