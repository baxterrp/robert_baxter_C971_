using SQLite;
using System;

namespace robert_baxter_C971_.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }
        public string Progress { get; set; }
        public string Instructor { get; set; }
        public string InstructorEmail { get; set; }
        public string InstructorPhone { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
    }
}
