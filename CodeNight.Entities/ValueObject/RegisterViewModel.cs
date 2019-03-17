using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace EOgrenme.Entities.ValueObject
{
    public class RegisterViewModel
    {
        [DisplayName("Ad"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Name { get; set; }

        [DisplayName("Soyad"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Lastname { get; set; }

        [DisplayName("Kullanıcı adı"),
        Required(ErrorMessage = "{0} alanı boş geçilemez."),
        StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Username { get; set; }

        [DisplayName("E-posta"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            StringLength(70, ErrorMessage = "{0} max. {1} karakter olmalı."),
            EmailAddress(ErrorMessage = "{0} alanı için geçerli bir e-posta adresi giriniz.")]
        public string EMail { get; set; }

        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
            DataType(DataType.Password),
            StringLength(25, ErrorMessage = "{0} max. {1} karakter olmalı."),
            Compare("Password", ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        public string RePassword { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Range(typeof(DateTime), "1/1/1900", "30/12/2017",
            ErrorMessage = "{0} {1} ile {2} arasında olmalıdır.")]

        [DisplayName("Doğum Tarihi"),
            Required(ErrorMessage = "{0} alanı boş geçilemez."),
  
          DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Telefon No"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(12, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Tel { get; set; }

    }
}