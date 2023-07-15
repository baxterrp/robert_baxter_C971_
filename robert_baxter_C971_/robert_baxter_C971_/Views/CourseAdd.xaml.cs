using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
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

            if (string.IsNullOrWhiteSpace(InstructorEmail.Text))
            {
                await DisplayAlert("Error", "Please enter an instructor email", "Ok");
                return;
            }
            else
            {
                try
                {
                    // if the MailAddress class cannot parse the email text it is an invalid input
                    // from Microsoft:         
                    //   T:System.FormatException:
                    //   address is not in a recognized format. -or- address contains non-ASCII characters.
                    var _ = new MailAddress(InstructorEmail.Text);
                }
                catch (FormatException)
                {
                    await DisplayAlert("Error", $"{InstructorEmail.Text} is not a valid email", "Ok");
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(InstructorPhone.Text))
            {
                await DisplayAlert("Error", $"Please enter an instructor phone number", "Ok");
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
                InstructorEmail = InstructorEmail.Text,
                InstructorPhone = InstructorPhone.Text,
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date,
                Progress = CourseStatusPicker.SelectedItem.ToString(),
                Notes = CourseNotes.Text ?? string.Empty,
                Notify = Notification.IsToggled,
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