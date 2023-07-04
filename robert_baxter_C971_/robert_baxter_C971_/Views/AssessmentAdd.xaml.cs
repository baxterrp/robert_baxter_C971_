using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
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

            if (string.IsNullOrWhiteSpace(AssessmentTypePicker.SelectedItem?.ToString()))
            {
                await DisplayAlert("Error", "Please select assessment type", "Ok");
                return;
            }

            if (EndDatePicker.Date < StartDatePicker.Date)
            {
                await DisplayAlert("Error", "End date cannot precede start date", "Ok");
                return;
            }

            await DatabaseService.SaveNewAssessment(new Assessment
            {
                CourseId = _selectedCourse.Id,
                Name = AssessmentName.Text,
                Type = AssessmentTypePicker.SelectedItem.ToString(),
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date,
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