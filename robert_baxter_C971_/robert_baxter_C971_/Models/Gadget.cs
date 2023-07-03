using SQLite;
using System;

namespace robert_baxter_C971_.Models
{
    public class Gadget
    {
        [PrimaryKey,  AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int InStock { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
