using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EOgrenme.Entities
{
    [Table("Comments")]
    public class Comments
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedDate { get; set; }
        public Course Course { get; set; }
        public User Owner { get; set; }
        
    }
}
