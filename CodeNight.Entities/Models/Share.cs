using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EOgrenme.Entities
{
    [Table("Shares")]
    public class Share
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ShareText { get; set; }
        public string ShareImagFileName { get; set; }
        public string ShareVideoFileName { get; set; }
        public string ShareSoundFileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Author Owner { get; set; }
        public Course Course { get; set; }
    }
}
