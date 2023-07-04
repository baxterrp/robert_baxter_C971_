using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AssessmentEdit : ContentPage
	{
        private Assessment _selectedAssessment;

        public AssessmentEdit ()
		{
			InitializeComponent ();
		}

        public AssessmentEdit(Assessment selectedAssessment)
        {
            InitializeComponent();
            _selectedAssessment = selectedAssessment;

            AssessmentName.Text = _selectedAssessment.Name;
            AssessmentTypePicker.SelectedItem = _selectedAssessment.Type;
            StartDatePicker.Date = _selectedAssessment.StartDate;
            EndDatePicker.Date = _selectedAssessment.EndDate;
        }

        private async void DeleteAssessment_Clicked(object sender, System.EventArgs e)
        {
            if(await DisplayAlert("Delete Assessment", "Are you sure you want to delete this assessment?", "Yes", "No"))
            {
                await DatabaseService.DeleteAssessment(_selectedAssessment);
                await DisplayAlert("Success", "Successfully deleted assessment", "Ok");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Canceled", "No assessment deleted", "Ok");
            }
        }

        private async void SaveAssessment_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AssessmentName.Text))
            {
                await DisplayAlert("Error", "Please enter an assessmenet name", "Ok");
                return;
            }

            if (string.IsNullOrWhiteSpace(AssessmentTypePicker.SelectedItem?.ToString()))
            {
                await DisplayAlert("Error", "Please select an assessment type", "Ok");
                return;
            }

            if(EndDatePicker.Date < StartDatePicker.Date)
            {
                await DisplayAlert("Error", "End date cannot precede start date", "Ok");
                return;
            }

            _selectedAssessment.Name = AssessmentName.Text;
            _selectedAssessment.Type = AssessmentTypePicker.SelectedItem.ToString();
            _selectedAssessment.StartDate = StartDatePicker.Date;
            _selectedAssessment.EndDate = EndDatePicker.Date;

            await DatabaseService.UpdateAssessment(_selectedAssessment);
            await DisplayAlert("Success", "Successfully saved assessment", "Ok");
            await Navigation.PopAsync();
        }

        private async void CancelAssessment_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}