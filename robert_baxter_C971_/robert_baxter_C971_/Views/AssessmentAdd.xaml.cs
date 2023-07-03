using robert_baxter_C971_.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace robert_baxter_C971_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AssessmentAdd : ContentPage
	{
        private Course selectedCourse;

        public AssessmentAdd ()
		{
			InitializeComponent ();
		}

        public AssessmentAdd(Course selectedCourse)
        {
            this.selectedCourse = selectedCourse;
        }
    }
}