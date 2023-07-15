using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System;
using System.Linq;
using System.Net.Mail;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseEdit : ContentPage
    {
        private Course _selectedCourse;

        public CourseEdit()
        {
            InitializeComponent();
        }

        public CourseEdit(Course selectedCourse)
        {
            InitializeComponent();
            _selectedCourse = selectedCourse;

            CourseName.Text = _selectedCourse.Name;
            InstructorName.Text = _selectedCourse.Instructor;
            InstructorEmail.Text = _selectedCourse.InstructorEmail;
            InstructorPhone.Text = _selectedCourse.InstructorPhone;
            StartDatePicker.Date = _selectedCourse.StartDate;
            EndDatePicker.Date = _selectedCourse.EndDate;
            CourseStatusPicker.SelectedItem = _selectedCourse.Progress;
            CourseNotes.Text = _selectedCourse.Notes;
            Notification.IsToggled = _selectedCourse.Notify;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var assessments = await DatabaseService.GetAssessmentsByCourse(_selectedCourse);
            AssessmentCollectionView.ItemsSource = assessments;

            AddAssessment.IsEnabled = assessments.Count() < 2;
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

            _selectedCourse.Name = CourseName.Text;
            _selectedCourse.Instructor = InstructorName.Text;
            _selectedCourse.InstructorPhone = InstructorPhone.Text;
            _selectedCourse.InstructorEmail = InstructorEmail.Text;
            _selectedCourse.StartDate = StartDatePicker.Date;
            _selectedCourse.EndDate = EndDatePicker.Date;
            _selectedCourse.Notes = CourseNotes.Text;
            _selectedCourse.Notify = Notification.IsToggled;

            await DatabaseService.UpdateCourse(_selectedCourse);
            await DisplayAlert("Success", "Successfully saved course", "Ok");
            await Navigation.PopAsync();
        }

        private async void CancelCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void DeleteCourse_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Delete Course", "Are you sure you want to delete this course?", "Yes", "No"))
            {
                await DatabaseService.DeleteCourse(_selectedCourse);
                await DisplayAlert("Success", "Successfully deleted course", "Ok");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Canceled", "No course deleted", "Ok");
            }
        }

        private async void AddAssessment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssessmentAdd(_selectedCourse));
        }

        private async void AssessmentCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAssessment = AssessmentCollectionView.SelectedItem as Assessment;
            await Navigation.PushAsync(new AssessmentEdit(selectedAssessment));
        }

        private async void ShareNotesButton_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = _selectedCourse.Notes,
                Title = $"Course {_selectedCourse.Name} Notes",
            });
        }
    }
}