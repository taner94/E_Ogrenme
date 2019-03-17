using EOgrenme.BusinessLayer;
using EOgrenme.Entities;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CodeNight.Controllers.CourseController
{
    public class UserCourseController : Controller
    {
        CourseManager courseManager = new CourseManager();
        UserCourseManager userCourseManager = new UserCourseManager();

        // GET: UserCourse
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult ShowUser(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserCourses course = userCourseManager.ListQueryable().FirstOrDefault(x => x.Course_Id == id);
        //    if (course == null)
        //    {
        //        HttpNotFound();
        //    }
        //    return View(course);
        //}

        //[HttpPost]
        //public ActionResult Create(Course course, int? userid)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ModelState.Remove("CreatedDate");
        //        if (userid == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        Author user = userCourseManager.Find(x => x.Id == userid);
        //        if (course == null)
        //        {
        //            return new HttpNotFoundResult();
        //        }
        //        if (userCourseManager.Insert(user) > 0)
        //        {
        //            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Author user = userCourseManager.Find(x => x.Id == id);
        //    if (user == null)
        //    {
        //        return new HttpNotFoundResult();
        //    }
        //    if (userCourseManager.Delete(user) > 0)
        //    {
        //        return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        //}
    }
}