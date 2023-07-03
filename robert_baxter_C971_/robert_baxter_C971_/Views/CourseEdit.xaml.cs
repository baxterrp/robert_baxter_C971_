﻿using robert_baxter_C971_.Models;
using robert_baxter_C971_.Services;
using System;
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
            StartDatePicker.Date = _selectedCourse.StartDate;
            EndDatePicker.Date = _selectedCourse.EndDate;
            CourseNotes.Text = _selectedCourse.Notes;
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

            _selectedCourse.Name = CourseName.Text;
            _selectedCourse.Instructor = InstructorName.Text;
            _selectedCourse.StartDate = StartDatePicker.Date;
            _selectedCourse.EndDate = EndDatePicker.Date;
            _selectedCourse.Notes = CourseNotes.Text;

            await DatabaseService.UpdateCourse(_selectedCourse);
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
    }
}