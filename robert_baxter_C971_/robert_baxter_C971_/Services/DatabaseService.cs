using robert_baxter_C971_.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace robert_baxter_C971_.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _dbConnection;

        public static async Task Initialize()
        {
            if(_dbConnection != null)
            {
                return;
            }

            var path = Path.Combine(FileSystem.AppDataDirectory, "Gadgets.db");
            _dbConnection = new SQLiteAsyncConnection(path);

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
    }
}
