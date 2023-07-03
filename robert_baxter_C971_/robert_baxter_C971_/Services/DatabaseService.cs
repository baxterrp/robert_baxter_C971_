using robert_baxter_C971_.Models;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace robert_baxter_C971_.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _dbConnection;

        public static async Task Initialize()
        {
            if (_dbConnection != null)
            {
                return;
            }

            _dbConnection =
                new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "wgu_db.db"));

            await _dbConnection.CreateTableAsync<Term>();
            await _dbConnection.CreateTableAsync<Instructor>();
            await _dbConnection.CreateTableAsync<Course>();
            await _dbConnection.CreateTableAsync<Assessment>();

            // cleanup
            await _dbConnection.CreateTableAsync<Gadget>();
            await _dbConnection.CreateTableAsync<Widget>();
        }

        #region Terms
        public static async Task SaveNewTerm(Term newTerm)
        {
            await Initialize();
            await _dbConnection.InsertAsync(newTerm);
        }

        public static async Task<IEnumerable<Term>> GetAllTerms()
        {
            await Initialize();
            return await _dbConnection.Table<Term>().ToListAsync();
        }

        internal static async Task DeleteTerm(Term term)
        {
            await Initialize();

            var courses = await _dbConnection.Table<Course>().Where(course => course.TermId == term.Id).ToListAsync();
            var courseIds = courses.Select(course => course.Id).ToArray();
            var assessments =
                await _dbConnection.Table<Assessment>()
                    .Where(assessment => courseIds.Contains(assessment.CourseId))
                    .ToListAsync();

            foreach (var assessment in assessments)
            {
                await _dbConnection.DeleteAsync(assessment);
            }

            foreach (var course in courses)
            {
                await _dbConnection.DeleteAsync(course);
            }

            await _dbConnection.DeleteAsync(term);
        }

        internal static async Task UpdateTerm(Term selectedTerm)
        {
            await Initialize();
            await _dbConnection.UpdateAsync(selectedTerm);
        }
        #endregion

        #region Courses
        internal static async Task SaveNewCourse(Course course)
        {
            await Initialize();
            await _dbConnection.InsertAsync(course);
        }

        internal static async Task<IEnumerable> GetCoursesByTerm(Term selectedTerm)
        {
            await Initialize();
            return await _dbConnection.Table<Course>().Where(course => course.TermId == selectedTerm.Id).ToListAsync();
        }

        internal static async Task UpdateCourse(Course course)
        {
            await Initialize();
            await _dbConnection.UpdateAsync(course);
        }

        internal static async Task<IEnumerable<Course>> GetAllCourses()
        {
            await Initialize();
            return await _dbConnection.Table<Course>().ToListAsync();
        }

        internal static async Task DeleteCourse(Course selectedCourse)
        {
            await Initialize();
            var assessments = await GetAssessmentsByCourse(selectedCourse);

            foreach(var assessment in assessments)
            {
                await _dbConnection.DeleteAsync(assessment);
            }

            await _dbConnection.DeleteAsync(selectedCourse);
        }
        #endregion

        #region Assessments
        internal static async Task SaveNewAssessment(Assessment assessment)
        {
            await Initialize();
            await _dbConnection.InsertAsync(assessment);
        }

        internal static async Task<IEnumerable<Assessment>> GetAssessmentsByCourse(Course selectedCourse)
        {
            await Initialize();
            return await _dbConnection.Table<Assessment>().Where(assessment => assessment.CourseId == selectedCourse.Id).ToListAsync();
        }

        internal static async Task DeleteAssessment(Assessment selectedAssessment)
        {
            await Initialize();
            await _dbConnection.DeleteAsync(selectedAssessment);
        }

        internal static async Task UpdateAssessment(Assessment selectedAssessment)
        {
            await Initialize();
            await _dbConnection.UpdateAsync(selectedAssessment);
        }

        internal static async Task<IEnumerable<Assessment>> GetAllAssessments()
        {
            await Initialize();
            return await _dbConnection.Table<Assessment>().ToListAsync();
        }
        #endregion
    }
}
