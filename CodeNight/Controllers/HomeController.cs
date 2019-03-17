using EOgrenme.BusinessLayer;
using EOgrenme.BusinessLayer.Result;
using EOgrenme.Entities;
using EOgrenme.Models;
using EOgrenme.ViewModels;
using System.Web.Mvc;

namespace EOgrenme.Controllers
{
    public class HomeController : Controller
    {
        SurvayFirstManager survayFirstManager = new SurvayFirstManager();
        SurvayLastManager survayLastManager = new SurvayLastManager();
        UserManager userManager = new UserManager();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FirstSurvey()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FirstSurvey(Survey survey,int id)
        {
            if (ModelState.IsValid)
            {
                survayFirstManager.Insert(survey);
            }
            BusinessLayerResult<User> res = userManager.UserTypeOfCourse(CurrentSession.User.Id,id);
            return RedirectToAction("Index", "Home");
        }



        public ActionResult LastSurvey()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LastSurvey(Survey2 survey)
        {
            if (ModelState.IsValid)
            {
                survayLastManager.Insert(survey);
            }
            return RedirectToAction("Index", "Home");
        }
       
    }
}