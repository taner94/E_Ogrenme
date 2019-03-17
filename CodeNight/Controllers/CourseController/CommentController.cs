using EOgrenme.BusinessLayer;
using EOgrenme.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOgrenme.Models;
using System.IO;
using System.Net;
using System.Data.Entity;

namespace EOgrenme.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        CourseManager courseManager = new CourseManager();
        CommentManager commentManager = new CommentManager();

        public ActionResult ShowShareComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course share = courseManager.ListQueryable().FirstOrDefault(x => x.Id == id);
            if (share == null)
            {
                HttpNotFound();
            }
            return PartialView("_PartialComments", share.Comments);
        }



        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comment = commentManager.Find(x => x.Id == id);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
            comment.CreatedDate = DateTime.Now;
            comment.CommentText = text;

            if (commentManager.Update(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comment = commentManager.Find(x => x.Id == id);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
            if (commentManager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(Comments comment, int? shareid)
        {
            if (ModelState.IsValid)
            {
                ModelState.Remove("CreatedDate");
                if (shareid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Course course = courseManager.Find(x => x.Id == shareid);


                if (course == null)
                {
                    return new HttpNotFoundResult();
                }
                comment.Course = course;
                comment.Owner = CurrentSession.User;
                comment.CreatedDate = DateTime.Now;
                if (commentManager.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}
