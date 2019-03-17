using Common.Helpers;
using EOgrenme.BusinessLayer;
using EOgrenme.BusinessLayer.Result;
using EOgrenme.Entities;
using EOgrenme.Entities.ValueObject;
using EOgrenme.Models;
using EOgrenme.ViewModels;
using System;
using System.Web;
using System.Web.Mvc;

namespace EOgrenme.Controllers.AccountController
{
    public class UserController : Controller
    {
        UserManager userManager = new UserManager();
        AuthorManager authorManager = new AuthorManager();

        // GET: User
        public ActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<User> res = userManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                CurrentSession.Set<User>("UserLogin", res.Result); // Session'a kullanıcı bilgi saklama..
                if (res.Result.TypeOfCourses == 0)
                {
                    return RedirectToAction("FirstSurvey", "Home");
                }
                return RedirectToAction("Index", "Home");   // yönlendirme..
            }

            return View(model);
        }
        //UserLogout
        public ActionResult UserLogout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        //UserRegister
        public ActionResult UserRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<User> res = userManager.UserRegister(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                OkVM notifyObj = new OkVM()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/User/UserLogin",
                };

                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");
                return View("Ok", notifyObj);
            }
            return View(model);
        }

        //UserEditProfile
        public ActionResult UserEditProfile()
        {
            BusinessLayerResult<User> res = userManager.GetUserById(CurrentSession.User.Id);
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult UserEditProfile(User model, HttpPostedFileBase ProfileImage)
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

                BusinessLayerResult<User> res = userManager.UpdateUserProfile(model);

                // Profil güncellendiği için session güncellendi.
                CurrentSession.Set<User>("UserLogin", res.Result);
                return RedirectToAction("Index", "Home");
            }
            return View(model);

        }

        //UserShowUserProfile
        public ActionResult UserShowUserProfile(int? id)
        {
            return View(userManager.Find(x => x.Id == id));
        } 
        //UserShowAuthorProfile
        public ActionResult UserShowAuthorProfile(int? id)
        {
            return View(authorManager.Find(x => x.Id == id));
        }

        //UserActivateUser
        public ActionResult UserActivateUser(Guid id)
        {
            BusinessLayerResult<User> res = userManager.ActivateUser(id);

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
                RedirectingUrl = "/User/UserLogin"
            };

            okNotifyObj.Items.Add("Hesabınız aktifleştirildi.");

            return View("Ok", okNotifyObj);
        }

        public ActionResult UserShowMyProfile()
        {
            BusinessLayerResult<User> res = userManager.GetUserById(CurrentSession.User.Id);
            return View(res.Result);
        }

        //UserFreezeProfile
        public ActionResult UserFreezeProfile()
        {
            BusinessLayerResult<User> res = userManager.UserFreezeProfile(CurrentSession.User.Id);
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
        public ActionResult UserForgetPassword()
        {
            return View();
        }
        //AuthorFreezeProfile
        [HttpPost]
        public ActionResult UserForgetPassword(string mail)
        {
            string pass = Guid.NewGuid().ToString().Substring(1, 10);
            BusinessLayerResult<User> res = userManager.UserForgetPassword(mail, pass);
            try
            {
                if (res.Result != null)
                {
                    string body = "Merhaba Değerli User;<br><br>Hesabınızın yeni şifresi :" + pass;
                    MailHelper.SendMail(body, mail, "EÖğrenme Forget Password");
                }
               
            }
            catch (Exception ex)
            {
                ViewData.ModelState.AddModelError("_HATA", ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}