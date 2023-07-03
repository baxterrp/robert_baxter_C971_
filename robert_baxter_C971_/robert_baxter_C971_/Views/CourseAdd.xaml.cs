using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseAdd : ContentPage
    {
        private Term _selectedTerm;

        public CourseAdd()
        {
            InitializeComponent();
        }

        public CourseAdd(Term selectedTerm)
        {
            InitializeComponent();
            _selectedTerm = selectedTerm;
        }

        private async void SaveCourse_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CourseName.Text))
            {
                await DisplayAlert("Error", "Please enter course name", "Ok");
                return;
            }

            if (string.IsNullOrWhiteSpace(InstructorName.Text))
            {
                await DisplayAlert("Error", "Please enter Instructor name", "Ok");
                return;
            }

            if (EndDatePicker.Date < StartDatePicker.Date)
            {
                await DisplayAlert("Error", "Course end date cannot precede start date", "Ok");
                return;
            }

            await DatabaseService.SaveNewCourse(new Course
            {
                TermId = _selectedTerm.Id,
                Name = CourseName.Text,
                Instructor = InstructorName.Text,
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date,
                Notes = CourseNotes.Text,
            });

            await DisplayAlert("Success", "New Course Added", "Ok");
            await Navigation.PopAsync();
        }

        private async void CancelCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}