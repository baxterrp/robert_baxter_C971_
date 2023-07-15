using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System.Linq;
using System.Security;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentAdd : ContentPage
    {
        private Course _selectedCourse;

        public AssessmentAdd()
        {
            InitializeComponent();
        }

        public AssessmentAdd(Course selectedCourse)
        {
            InitializeComponent();
            _selectedCourse = selectedCourse;
        }

        private async void SaveAssessment_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AssessmentName.Text))
            {
                await DisplayAlert("Error", "Please enter assessment name", "Ok");
                return;
            }

            var selectedAssessmentType = AssessmentTypePicker.SelectedItem?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(selectedAssessmentType))
            {
                await DisplayAlert("Error", "Please select assessment type", "Ok");
                return;
            }

            if (EndDatePicker.Date < StartDatePicker.Date)
            {
                await DisplayAlert("Error", "End date cannot precede start date", "Ok");
                return;
            }

            var assessments = (await DatabaseService.GetAssessmentsByCourse(_selectedCourse)).ToList();
            
            if (assessments.Any(assessment => selectedAssessmentType.Equals(assessment.Type)))
            {
                await DisplayAlert("Error", $"An assessment of type {selectedAssessmentType} has already been added to this course", "Ok");
                return;
            }

            await DatabaseService.SaveNewAssessment(new Assessment
            {
                CourseId = _selectedCourse.Id,
                Name = AssessmentName.Text,
                Type = AssessmentTypePicker.SelectedItem.ToString(),
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date,
                Notify = Notification.IsToggled,
            });

            await DisplayAlert("Success", "Successfully saved assessment", "Ok");
            await Navigation.PopAsync();
        }

        private async void CancelAssessment_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}