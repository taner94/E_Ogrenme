using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EOgrenme.Entities
{
    [Table("TypeOfCourse")]
    public class TypeOfCourse
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public  List<Course> Course { get; set; }
        public TypeOfCourse()
        {

            Course = new List<Course>();

        }
    }
}



