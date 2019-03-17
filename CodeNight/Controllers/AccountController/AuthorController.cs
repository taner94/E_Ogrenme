using Common;
using Common.Helpers;
using EOgrenme.BusinessLayer;
using EOgrenme.BusinessLayer.Result;
using EOgrenme.Entities;
using EOgrenme.Entities.ValueObject;
using EOgrenme.Models;
using EOgrenme.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace EOgrenme.Controllers.AccountController
{
    public class AuthorController : Controller
    {
        // GET: Author
        public ActionResult Index()
        {
            return View();
        }
        AuthorManager authorManager = new AuthorManager();
        UserManager userManager = new UserManager();
        ShareManager shareManager = new ShareManager();
        CourseManager courseManager = new CourseManager();

        //AuthorLogin
        public ActionResult AuthorLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AuthorLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Author> res = authorManager.LoginAuthor(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                AuthorSession.Set<Author>("AuthorLogin", res.Result); // Session'a kullanıcı bilgi saklama..
                return RedirectToAction("Index", "Home");   // yönlendirme..
            }

            return View(model);
        }



        //UserLogout
        public ActionResult AuthorLogout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        //UserRegister
        public ActionResult AuthorRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AuthorRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Author> res = authorManager.AuthorRegister(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                OkVM notifyObj = new OkVM()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Author/AuthorLogin",
                };

                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");
                return View("Ok", notifyObj);
            }
            return View(model);
        }

        //UserEditProfile
        public ActionResult AuthorEditProfile()
        {
            BusinessLayerResult<Author> res = authorManager.GetUserById(AuthorSession.Author.Id);
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult AuthorEditProfile(Author model, HttpPostedFileBase ProfileImage)
        {
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png" ||
                    ProfileImage.ContentType == "image/gif"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFileName = filename;
                }

                BusinessLayerResult<Author> res = authorManager.UpdateAuthorProfile(model);

                // Profil güncellendiği için session güncellendi.
                CurrentSession.Set<Author>("AuthorLogin", res.Result);
                return RedirectToAction("Index", "Home");
            }
            return View(model);

        }


        public ActionResult AuthorShowMyProfile()
        {
            BusinessLayerResult<Author> res = authorManager.GetUserById(AuthorSession.Author.Id);
            return View(res.Result);
        }

        //AuthorShowUserProfile
        public ActionResult AuthorShowUserProfile(int? id)
        {
            //return RedirectToAction("ShowProfile/"+id.Value, "Account");
            return View(userManager.Find(x => x.Id == id));
        }

        //AuthorShowAuthorProfile
        public ActionResult AuthorShowAuthorProfile(int? id)
        {
            //return RedirectToAction("ShowProfile/"+id.Value, "Account");
            return View(authorManager.Find(x => x.Id == id));
        }

        //UserActivateUser
        public ActionResult AuthorActivateUser(Guid id)
        {
            BusinessLayerResult<Author> res = authorManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorVM errorNotifyObj = new ErrorVM()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            OkVM okNotifyObj = new OkVM()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Author/AuthorLogin"
            };

            okNotifyObj.Items.Add("Hesabınız aktifleştirildi.");

            return View("Ok", okNotifyObj);
        }

        public ActionResult AuthorForgetPassword()
        {
            return View();
        }
        //AuthorFreezeProfile
        [HttpPost]
        public ActionResult AuthorForgetPassword(string mail)
        {
            string pass = Guid.NewGuid().ToString().Substring(1, 10);
            BusinessLayerResult<Author> res = authorManager.AuthorForgetPassword(mail, pass);
            try
            {
                if (res.Result != null)
                {
                    string body = "Merhaba Değerli Author;<br><br>Hesabınızın yeni şifresi :" + pass;
                    MailHelper.SendMail(body, mail, "EÖğrenme Forget Password");
                }

            }
            catch (Exception ex)
            {
                ViewData.ModelState.AddModelError("_HATA", ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }


        //AuthorFreezeProfile
        [HttpGet]
        public ActionResult AuthorFreezeProfile()
        {
            BusinessLayerResult<Author> res = authorManager.UserFreezeProfile(AuthorSession.Author.Id);
            if (res.Errors.Count > 0)
            {
                ErrorVM errorNotifyObj = new ErrorVM()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/User/UserLogin"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AuthorSharedCourse()
        {
            var couses = courseManager.ListQueryable().Include("Owner").Where(
            x => x.Owner.Id == AuthorSession.Author.Id).OrderByDescending(
            x => x.CreatedDate);
            return View(couses.ToList());
        }
        public ActionResult AuthorSharedCourseShares()
        {
            var share = shareManager.ListQueryable().Include("Owner").Where(
            x => x.Owner.Id == AuthorSession.Author.Id).OrderByDescending(
            x => x.CreatedDate);
            return View(share.ToList());
        }
    }
}