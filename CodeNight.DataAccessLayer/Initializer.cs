using EOgrenme.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml;

namespace EOgrenme.DataAccessLayer
{
    public class Initializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {

            
            string[] a = { "Türk Edebiyatı","Fizik", "Dil ve Anlatım", "Kimya", "Sağlık Bilgisi", "Matematik", "Biyoloji", "İngilizce", "Tarih", "Almanca", "Coğrafya"};
            string[] b = { "Metin", "Resim", "Video", "Ses" };
            XmlTextReader reader = new XmlTextReader("Soru.xml");





            //Yazarlar
            for (int i = 1; i < 20; i++)
            {
                Author author = new Author()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Username = $"author{i}",
                    Password = "123456",
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfileImageFileName = "default.jpeg",
                    PhoneNumber = FakeData.PhoneNumberData.GetPhoneNumber(),
                    DateOfBirth = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-50), DateTime.Now.AddYears(-12)),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    Freeze = false
                                  
                };
                context.Authors.Add(author);
            }
            context.SaveChanges();


            
            for (int s = 0; s < b.Length; s++)
            {
                TypeOfCourse Tcourse = new TypeOfCourse()
                {
                    TypeName = b[s]
                };
                context.TypeOfCourse.Add(Tcourse);
            }
            context.SaveChanges();

            List<TypeOfCourse> typeList = context.TypeOfCourse.ToList();
            for (int i = 1; i < 20; i++)
            {
                User user = new User()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Username = $"user{i}",
                    Password = "123456",
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfileImageFileName = "default.jpeg",
                    PhoneNumber = FakeData.PhoneNumberData.GetPhoneNumber(),
                    DateOfBirth = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-50), DateTime.Now.AddYears(-12)),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    Freeze = false,
                    TypeOfCourses = typeList[FakeData.NumberData.GetNumber(0, typeList.Count)].Id
            };
                context.Users.Add(user);
            }
            context.SaveChanges();

            //Derslerin Oluşturulması
            List<User> userList = context.Users.ToList();     
            List<Author> authorList = context.Authors.ToList();

            for (int i = 0; i < a.Length; i++)
            {
                //Ders
                Author owner = authorList[FakeData.NumberData.GetNumber(0, authorList.Count - 1)];
                Course course = new Course()
                {
                    CourseName = a[i],
                    UserCount = FakeData.NumberData.GetNumber(1, 15),
                    Owner = owner,
                    CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    CourseActive =true
                };
                //Userlar
                for (int l = 0; l < course.UserCount; l++)
                {
                    User us= userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    course.User.Add(us);
                }
                //Ders Tipleri
                TypeOfCourse tpc = typeList[FakeData.NumberData.GetNumber(0, typeList.Count - 1)];
                course.TypeOfCourse = tpc;
                //Paylasimlar
                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 20); k++)
                {
                    Share share = new Share()
                    {
                        ShareText = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Owner = owner,
                        Course = course,
                        CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ShareImagFileName = "ay.jpg",
                        ShareSoundFileName = null
                    };
                    course.Share.Add(share);
                }
                //Yorumlar
                for (int j = 0; j < FakeData.NumberData.GetNumber(5, 20); j++)
                {
                    User comment_owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Comments comment = new Comments()
                    {
                        CommentText = FakeData.TextData.GetSentence(),
                        Owner = comment_owner,
                        CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now)
                    };
                    course.Comments.Add(comment);
                }
                context.Course.Add(course);
            }
            context.SaveChanges();
        }
    }
}