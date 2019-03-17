using EOgrenme.BusinessLayer;
using EOgrenme.Entities;
using EOgrenme.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeNight.Controllers.CourseController
{
    public class ShareController : Controller
    {

        ShareManager shareManager = new ShareManager();
        CourseManager courseManager = new CourseManager();
        // GET: Share
        public ActionResult SharesAuthor(int? id)
        {
            ViewData["deneme"] = courseManager.Find(x => x.Id == id).CourseName;
            var shares = shareManager.ListQueryable().Where(x => x.Course.Id == id).OrderByDescending(y => y.CreatedDate);
            return View(shares.ToList());

        }

        public ActionResult Shares(int? id)
        {
            ViewData["deneme"] = courseManager.Find(x => x.Id == id).CourseName;
            return View();
        }

        // GET: Share
        public ActionResult SharesUser(int? id)
        {
            var shares = shareManager.ListQueryable().Where(x => x.Course.Id == id).OrderByDescending(y => y.CreatedDate);
            return View(shares.ToList());
        }


        public ActionResult ShareDelete(int? id)
        {
            Share share = shareManager.Find(x => x.Id == id);
            shareManager.Delete(share);

            return RedirectToAction("Index", "Home");
        }


        //Text Upload
        public ActionResult ShareShare()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ShareShare(Share share, string ad)
        {
            if (ModelState.IsValid)
            {
                share.Owner = AuthorSession.Author;
                share.CreatedDate = DateTime.Now;
                share.Course = courseManager.Find(x => x.CourseName == ad);
                shareManager.Insert(share);
                return RedirectToAction("Index", "Home");

            }
            return View(share);
        }


        //Image Upload
        public ActionResult ImageUpload(string ad)
        {
            ViewData["id"] = ad;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImageUpload(Share share, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                //image Upload
                string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                string extension = Path.GetExtension(image.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmsfff") + extension;
                share.ShareImagFileName = fileName;
                fileName = Path.Combine(Server.MapPath("~/images/"), fileName);

                image.SaveAs(fileName);

                share.Course = courseManager.Find(x => x.CourseName == share.Course.CourseName);
                share.Owner = AuthorSession.Author;
                share.CreatedDate = DateTime.Now;

                shareManager.Insert(share);
                return RedirectToAction("Index", "Home");
            }
            return View(share);
        }


        //Video Upload
        public ActionResult VideoUpload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VideoUpload(Share share, HttpPostedFileBase video)
        {
            if (ModelState.IsValid)
            {
                //image Upload
                string fileName = Path.GetFileNameWithoutExtension(video.FileName);
                string extension = Path.GetExtension(video.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmsfff") + extension;
                share.ShareVideoFileName = fileName;
                fileName = Path.Combine(Server.MapPath("~/videos"), fileName);
                video.SaveAs(fileName);


                share.Owner = AuthorSession.Author;
                share.Course = courseManager.Find(x => x.CourseName == share.Course.CourseName);
                share.CreatedDate = DateTime.Now;

                shareManager.Insert(share);
                return RedirectToAction("Index", "Home");
            }

            return View(share);
        }


        //Sound Upload
        public ActionResult SoundUpload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SoundUpload(Share share, HttpPostedFileBase sound)
        {
            if (ModelState.IsValid)
            {
                //image Upload
                string fileName = Path.GetFileNameWithoutExtension(sound.FileName);
                string extension = Path.GetExtension(sound.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmsfff") + extension;
                share.ShareSoundFileName = fileName;
                fileName = Path.Combine(Server.MapPath("~/sounds/"), fileName);

                sound.SaveAs(fileName);

                share.Course = courseManager.Find(x => x.CourseName == share.Course.CourseName);
                share.Owner = AuthorSession.Author;
                share.CreatedDate = DateTime.Now;

                shareManager.Insert(share);
                return RedirectToAction("Index", "Home");
            }
            return View(share);
        }
    }
}
