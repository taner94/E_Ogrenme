using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace EOgrenme.Entities
{
    [Table("Course")]
    public class Course
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int UserCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public Author Owner { get; set; }
        public TypeOfCourse TypeOfCourse { get; set; }
        public List<Comments> Comments { get; set; }
        public List<Share> Share { get; set; }
        public List<User> User { get; set; }
        public Boolean CourseActive { get; set; }

        public Course()
        {
            Comments = new List<Comments>();
            Share = new List<Share>();
            User = new List<User>();
        }

       
    }
}
