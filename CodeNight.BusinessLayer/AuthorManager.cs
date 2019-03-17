using EOgrenme.Entities;
using System;
using EOgrenme.BusinessLayer.Abstract;
using Common;
using Common.Helpers;
using EOgrenme.BusinessLayer.Result;
using EOgrenme.Entities.Messages;
using EOgrenme.Entities.ValueObject;


namespace EOgrenme.BusinessLayer
{
    public class AuthorManager : ManagerBase<Author>
    {
        public BusinessLayerResult<Author> AuthorRegister(RegisterViewModel data)
        {
            Author author = Find(x => x.Username == data.Username);
            BusinessLayerResult<Author> res = new BusinessLayerResult<Author>();
            if (author != null)
            {
                if (author.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (author.Email == data.EMail)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-mail adresi kayıtlı");
                }
                if (Convert.ToDateTime(data.DateOfBirth) > DateTime.Now)
                {
                    res.AddError(ErrorMessageCode.UpdatedDate, "Tarihi yanlış girdiniz");
                }
            }

            else
            {
                int dbResult = base.Insert(new Author()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    Password = data.Password,
                    Name = data.Name,
                    ActivateGuid = Guid.NewGuid(),
                    PhoneNumber = data.Tel,
                    Surname = data.Lastname,
                    ProfileImageFileName = "default.jpeg",
                    DateOfBirth = Convert.ToDateTime(data.DateOfBirth)
                });

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.EMail && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Author/AuthorActivateUser/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, res.Result.Email, "EOgrenme Hesap Aktifleştirme");
                }
            }
            return res;
        }
        public BusinessLayerResult<Author> LoginAuthor(LoginViewModel data)
        {
            BusinessLayerResult<Author> res = new BusinessLayerResult<Author>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);


            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");
                }
                if (res.Result.Freeze)
                {
                    res.Result.Freeze = false;
                    Update(res.Result);
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı yada şifre uyuşmuyor.");
            }
            return res;
        }
        public BusinessLayerResult<Author> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<Author> res = new BusinessLayerResult<Author>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;
        }


        public BusinessLayerResult<Author> GetUserById(int id)
        {
            BusinessLayerResult<Author> res = new BusinessLayerResult<Author>();
            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }
            return res;
        }
        public BusinessLayerResult<Author> UpdateAuthorProfile(Author data)
        {

            Author db_Author = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<Author> res = new BusinessLayerResult<Author>();
            if (db_Author != null && db_Author.Id != data.Id)
            {
                if (db_Author.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_Author.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Username = data.Username;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.PhoneNumber = data.PhoneNumber;
            res.Result.DateOfBirth = data.DateOfBirth;

            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenemedi");
            }
            return res;
        }
        public BusinessLayerResult<Author> RemoveAuthorById(int id)
        {
            BusinessLayerResult<Author> res = new BusinessLayerResult<Author>();
            Author author = Find(x => x.Id == id);

            if (author != null)
            {
                if (Delete(author) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı");
            }
            return res;
        }

        //Method Hiding
        public new BusinessLayerResult<Author> Insert(Author data)
        {
            Author User = Find(x => x.Username == data.Username);
            BusinessLayerResult<Author> res = new BusinessLayerResult<Author>();
            res.Result = data;
            if (User != null)
            {
                if (User.Username == res.Result.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (User.Email == res.Result.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-mail adresi kayıtlı");
                }
            }

            else
            {
                data.ProfileImageFileName = "user.png";
                data.ActivateGuid = Guid.NewGuid();
                data.Username = res.Result.Username;
                data.DateOfBirth = Convert.ToDateTime(data.DateOfBirth);
                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "User Eklenemedi");
                }
            }
            return res;
        }
        public new BusinessLayerResult<Author> Update(Author data)
        {
            Author db_User = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<Author> res = new BusinessLayerResult<Author>();
            res.Result = data;
            if (db_User != null && db_User.Id != data.Id)
            {
                if (db_User.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_User.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
                return res;
            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Username = data.Username;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.PhoneNumber = data.PhoneNumber;

            res.Result.DateOfBirth = data.DateOfBirth;
            res.Result.IsActive = data.IsActive;


            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı Güncellenemedi");
            }
            return res;
        }
        public BusinessLayerResult<Author> UserFreezeProfile(int id)
        {
            BusinessLayerResult<Author> res = new BusinessLayerResult<Author>();
            res.Result = Find(x => x.Id == id);

            if (res.Result != null)
            {
                res.Result.Freeze = true;
                Update(res.Result);
            }

            return res;
        }

        public BusinessLayerResult<Author> AuthorForgetPassword(string email,string pass)
        {
            BusinessLayerResult<Author> res = new BusinessLayerResult<Author>();
            res.Result = Find(x => x.Email == email);

            if (res.Result != null)
            {
                res.Result.Password = pass;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }

            return res;
        }
    }
}

