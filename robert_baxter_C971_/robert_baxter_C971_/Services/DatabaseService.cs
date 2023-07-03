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

        public static async Task AddGadget(string name, string color, int inStock, decimal price, DateTime creationDate)
        {
            await Initialize();
            await _dbConnection.InsertAsync(new Gadget
            {
                Name = name,
                Color = color,
                InStock = inStock,
                Price = price,
                CreationDate = creationDate
            });
        }

        public static async Task RemoveGadget(int id)
        {
            await Initialize();

            await _dbConnection.DeleteAsync<Gadget>(id);
        }

        public static async Task<IEnumerable<Gadget>> GetGadgets()
        {
            await Initialize();

            return await _dbConnection.Table<Gadget>().ToListAsync();
        }

        public static async Task UpdateGadget(int id, string name, string color, int inStock, decimal price, DateTime creationDate)
        {
            await Initialize();

            var gadget = await _dbConnection.Table<Gadget>().Where(g => g.Id == id).FirstOrDefaultAsync();

            if (gadget != null)
            {
                gadget.Name = name;
                gadget.Color = color;
                gadget.InStock = inStock;
                gadget.Price = price;
                gadget.CreationDate = creationDate;

                await _dbConnection.UpdateAsync(gadget);
            }
        }

        public static async Task AddWidget(
            int gadgetId,
            string name,
            string color,
            int inStock,
            decimal price,
            DateTime creationDate,
            bool notificationStart,
            string notes)
        {
            await Initialize();
            await _dbConnection.InsertAsync(new Widget
            {
                GadgetId = gadgetId,
                Name = name,
                Color = color,
                InStock = inStock,
                Price = price,
                CreationDate = creationDate,
                StartNotification = notificationStart,
                Notes = notes
            });
        }

        public static async Task RemoveWidget(int id)
        {
            await Initialize();
            await _dbConnection.DeleteAsync<Widget>(id);
        }

        public static async Task<IEnumerable<Widget>> GetWidgets(int gadgetId)
        {
            await Initialize();
            return await _dbConnection.Table<Widget>().Where(w => w.GadgetId == gadgetId).ToListAsync();
        }

        public static async Task<IEnumerable<Widget>> GetWidgets()
        {
            await Initialize();
            return await _dbConnection.Table<Widget>().ToListAsync();
        }

        public static async Task UpdateWidget(
            int id,
            string name,
            string color,
            int inStock,
            decimal price,
            DateTime creationDate,
            bool notificationStart,
            string notes)
        {
            await Initialize();

            var widget = await _dbConnection.Table<Widget>().FirstOrDefaultAsync(w => w.Id == id);

            if (widget != null)
            {
                widget.Name = name;
                widget.Color = color;
                widget.InStock = inStock;
                widget.Price = price;
                widget.CreationDate = creationDate;
                widget.StartNotification = notificationStart;
                widget.Notes = notes;

                await _dbConnection.UpdateAsync(widget);
            }
        }

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
            await _dbConnection.DeleteAsync(selectedCourse);
        }
    }
}
