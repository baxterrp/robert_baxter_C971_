using SQLite;
using System;

namespace robert_baxter_C971_.Models
{
    public class Widget
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int GadgetId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int InStock { get; set; }
        public decimal Price { get; set; }
        public bool StartNotification { get; set; }
        public DateTime CreationDate { get; set; }
        public string Notes { get; set; }
    }
}
