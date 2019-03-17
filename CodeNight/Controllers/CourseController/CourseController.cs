using EOgrenme.BusinessLayer;
using EOgrenme.Entities;
using EOgrenme.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EOgrenme.Controllers
{
    public class CourseController : Controller
    {

        CourseManager courseManager = new CourseManager();
        ShareManager shareManager = new ShareManager();
        UserManager UserManager = new UserManager();
        UserCourseManager userCourseManager = new UserCourseManager();

        // GET: CourseByOwnerId
        public ActionResult Course()
        {
            var couses = courseManager.ListQueryable().Include("Owner").Where(
              x => x.Owner.Id == AuthorSession.Author.Id).OrderByDescending(
              x => x.CreatedDate);
            return View(couses.ToList());


        }

        //public ActionResult CourseOfUser(int? id)
        //{
        //    var user = userCourseManager.ListQueryable().Where(x => x.Course_Id == id).OrderByDescending(y => y.User_Id);
        //    return View(user.ToList());
        //}

        public ActionResult TypeOfCourseList()
        {
            var couses = courseManager.ListQueryable().Include("Owner").Where(
               x => x.TypeOfCourse.Id == CurrentSession.User.Id).OrderByDescending(
               x => x.CreatedDate);
            return View(couses.ToList());
        }


        TypeOfCourseManager typeOfCourseManager = new TypeOfCourseManager();
        [HttpPost]
        public ActionResult CreateCourse(Course course, int id)
        {
            if (ModelState.IsValid)
            {

                course.Owner = AuthorSession.Author;
                course.CreatedDate = DateTime.Now;
                course.TypeOfCourse = typeOfCourseManager.GetCourseTypeById(id);
                courseManager.Insert(course);
                return RedirectToAction("Index", "Home");
            }

            return View(course);
        }       


        public ActionResult Delete(int? id)
        {
            Course course = courseManager.Find(x => x.Id == id);
            courseManager.Delete(course);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult IsAvtiveCourse(int id)
        {
            if (ModelState.IsValid)
            {
                Course course = courseManager.Find(x => x.Id == id);
                course.CourseActive = false;
                courseManager.Update(course);
            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult IsNotAvtiveCourse(int id)
        {
            if (ModelState.IsValid)
            {
                Course course = courseManager.Find(x => x.Id == id);
                course.CourseActive = true;
                courseManager.Update(course);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult UpdateCourse(Course course, int id)
        {
            if (ModelState.IsValid)
            {
                course.Owner = AuthorSession.Author;
                course.TypeOfCourse = typeOfCourseManager.GetCourseTypeById(id);

                course.CreatedDate = DateTime.Now;
                courseManager.Update(course);
                return RedirectToAction("Index", "Home");

            }
            return View(course);

        }
    }
}