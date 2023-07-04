using SQLite;
using System;

namespace robert_baxter_C971_.Models
{
    public class Term
    {
        [PrimaryKey,  AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
