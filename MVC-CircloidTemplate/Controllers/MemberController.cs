using MVC_CircloidTemplate.App_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_CircloidTemplate.Controllers
{
    //[Authorize] // herkesin görmesi için eklenmemeli 

    public class MemberController : Controller
    {
       
        // GET: Member
        public ActionResult MemberLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MemberLogin(UserClass uc,string RememberMe) //Rememberme checkliyse on checksize off yazıyor
        {
           bool validationResult= Membership.ValidateUser(uc.UserName, uc.Password); //geçerli bir kullanıcı mı değil mi gidip db den kontrol eder.
            if(validationResult==true)
            {
                //Girilen kullanıcı ve şifre bilgileri doğru ise, kullanıcının web sitemize giriş yapabilmesi gerekir. Bunun için öncelikle web.config dosyasında authorization ayarlarının yapılması gerekir. Ayarlar yapıldıktan sonra FormsAuthentication.RedirectFromLoginPage() metodu çağırılacaktır.
                if(RememberMe=="on")
                {
                    FormsAuthentication.RedirectFromLoginPage(uc.UserName, true);//otomatik olarak login bilgilerini çerez olarak atar.
                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(uc.UserName, false);
                }
                //return RedirectToAction("Index","Home"); //home controllerdaki index action ına yönlendirildi.
            }
            else
            {
                ViewBag.Message = "Kullanıcı adı ya da Parola hatalı";
            }
            
            return View();

        }
        public ActionResult ForgotMyPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotMyPassword(UserClass cs)
        {
            
            return View();
        }
        public ActionResult CreateNewAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNewAccount(UserClass uc,string ConfirmPassword)
        {
            string createMessage = "";

            if (uc.Password != ConfirmPassword)
            {
                createMessage = "Şifreler uyuşmuyor";
                ViewBag.createMessage = createMessage;
                return View();
            }

            Membership.CreateUser(uc.UserName, uc.Password, uc.Email, uc.PasswordQuestion, uc.PasswordAnswer, true, out MembershipCreateStatus status);

            switch (status)
            {
                case MembershipCreateStatus.Success:
                    createMessage = "Kayıt başarılı";
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    createMessage = "Geçersiz kullanıcı adı";
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    createMessage = "Geçersiz şifre";
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    createMessage = "Geçersiz gizli soru";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    createMessage = "Geçersiz gizli cevap";
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    createMessage = "Geçersiz email";
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    createMessage = "Kullanılmış kullanıcı adı";
                    break;
                case MembershipCreateStatus.DuplicateEmail:
                    createMessage = "Kullanılmış email adresi";
                    break;
                case MembershipCreateStatus.UserRejected:
                    createMessage = "Kullanıcı engellendi";
                    break;
                case MembershipCreateStatus.InvalidProviderUserKey:
                    createMessage = "Geçersiz kullanıcı anahtarı";
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    createMessage = "Tekrarlanmış kullanıcı anahtarı";
                    break;
                case MembershipCreateStatus.ProviderError:
                    createMessage = "Sağlayıcı hatası";
                    break;
              
                default:
                    createMessage = "Bilinmeyen bir hata oluştu";
                    break;
            }

            ViewBag.createMessage = createMessage;

            if (status == MembershipCreateStatus.Success)
                return RedirectToAction("MemberLogin");

            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("MemberLogin");
        }
    }
}