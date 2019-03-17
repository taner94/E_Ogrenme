
using EOgrenme.Entities;
using System.Data.Entity;


namespace EOgrenme.DataAccessLayer
{
   public class DatabaseContext:DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }       
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<TypeOfCourse> TypeOfCourse { get; set; }
        public DbSet<Survey> FirstSurveys { get; set; }
        public DbSet<Survey2> LastSurveys { get; set; }
      


        public DatabaseContext()
        {
            Database.SetInitializer(new Initializer());
        }
    }
}
