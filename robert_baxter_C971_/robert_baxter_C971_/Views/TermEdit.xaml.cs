using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermEdit : ContentPage
    {
        private Term _selectedTerm;

        public TermEdit()
        {
            InitializeComponent();
        }

        public TermEdit(Term term)
        {
            InitializeComponent();
            _selectedTerm = term;

            TermTitle.Text = term.Title;
            StartDatePicker.Date = term.StartDate;
            EndDatePicker.Date = term.EndDate;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var courses = await DatabaseService.GetCoursesByTerm(_selectedTerm);
            CourseCollectionView.ItemsSource = courses;

            AddCourse.IsEnabled = courses.Count() <= 6;
        }

        private async void SaveTerm_Clicked(object sender, EventArgs e)
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

            _selectedTerm.Title = TermTitle.Text;
            _selectedTerm.StartDate = StartDatePicker.Date;
            _selectedTerm.EndDate = EndDatePicker.Date;

            await DatabaseService.UpdateTerm(_selectedTerm);
            await DisplayAlert("Success", "Successfully saved term", "Ok");
            await Navigation.PopAsync();
        }

        private async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Delete Term", $"Are you sure you want to delete {_selectedTerm.Title}", "Yes", "No"))
            {
                await DatabaseService.DeleteTerm(_selectedTerm);
                await DisplayAlert("Success", "Term deleted", "Ok");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Canceled", "Term not deleted", "Ok");
            }
        }

        private async void AddCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CourseAdd(_selectedTerm));
        }

        private async void CourseCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCourse = CourseCollectionView.SelectedItem as Course;
            await Navigation.PushAsync(new CourseEdit(selectedCourse));
        }

        private async void CancelTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}