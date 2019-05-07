using MVC_CircloidTemplate.App_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_CircloidTemplate.Controllers
{
    [Authorize(Roles="Admin")] //sadece admin olanlar bu sayfaya girebilsin

    public class UserController : Controller
    {
        public UserController()
        {
            ViewBag.UserSelected = "selected";
        }
        // GET: User
        public ActionResult Index()
        {
            //Veritabanındaki tüm kullanıcıları alarak users isimli collection'da toplayacak.
            MembershipUserCollection users = Membership.GetAllUsers();
            return View(users);
        }
        public ActionResult AddUser()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddUser(UserClass uc)
        {
            Membership.CreateUser(uc.UserName, uc.Password, uc.Email, uc.PasswordQuestion, uc.PasswordAnswer, true, out MembershipCreateStatus status);
            string createMessage = "";
            switch (status)
            {
                case MembershipCreateStatus.Success:
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
                    createMessage = "Başarılı";
                    break;
            }
            ViewBag.createMessage = createMessage;
            if (createMessage == "")
                return View();
            else
                return View();
        }
        public ActionResult AssignRole(string username,string message=null)
        {
            /*Parametre olarak id yazmak zorundayız, sebebi projenin App_Start klasörünün altında Route_config.cs dosyasında "{controller}/{action}/{id}" bu parametre adının default adı id olduğu için parametre adının da id olması gerekiyor. 
             *Kullanıcı RolAta ya tıkladığında kullanıcı adını parametre olarak buraya alıyoruz. Buradan da kullanıcının adını View'e gönderiyoruz. Amacımız parametre bilgisini View'e taşımak. View tarafında Ekle butonuna basınca tekrar kullanıcı adını ve rol adını View'den alıp Post tarafına taşımak.
             */
            if (string.IsNullOrWhiteSpace(username))
                return RedirectToAction("Index");
            MembershipUser user = Membership.GetUser(username);
            if (user == null)
                return HttpNotFound();
            string[] userRoles = Roles.GetRolesForUser(username);
            string[] allRoles = Roles.GetAllRoles();
            List<string> availableRoles = new List<string>();
            foreach (string role in allRoles)
            {
                if (!userRoles.Contains(role))
                    availableRoles.Add(role);
            }
            ViewBag.AvailableRoles = availableRoles;
            ViewBag.UserRoles = userRoles;
            ViewBag.Username = username;
            ViewBag.Message = message;

            return View();
        }
        [HttpPost]
        public ActionResult AssignRole(string username, List<string> addedRoles)
        {
            if(addedRoles==null)
                return RedirectToAction("AssignRole", new { username = username, message = "Önce rol seçiniz" });
            if (addedRoles.Count < 1)
                return RedirectToAction("AssignRole", new { username = username, message = "Hata" });
            Roles.AddUserToRoles(username, addedRoles.ToArray());
            
            return RedirectToAction("AssignRole", new {username=username,message="Başarılı" });
        }
        [HttpPost]
        public string DeleteRole(string username,string removedRoles)
        {
            string[] removedRolesArray = removedRoles.Split(',');
            if (removedRolesArray.Length < 1 || string.IsNullOrWhiteSpace(removedRolesArray[0]))
                return "Hata";
            Roles.RemoveUserFromRoles(username, removedRolesArray);
            return "Başarılı";
        }
        [HttpPost]
        public ActionResult DeleteUser(string id)
        {            
                Membership.DeleteUser(id);
                return RedirectToAction("Index");         

        }
        public ActionResult ShowRole(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return RedirectToAction("Index");
            MembershipUser user = Membership.GetUser(username);
            if (user == null)
                return HttpNotFound();
            string[] userRoles = Roles.GetRolesForUser(username);
            List<string> allUserRoles = new List<string>();
            foreach (string role in userRoles)
            {
                if (!userRoles.Contains(role))
                    allUserRoles.Add(role);
            }
            ViewBag.UserRoles = userRoles;

            return View();
        }
        public string ShowRole2(string username)
        {
            string roles = "";
            List<string> role = Roles.GetRolesForUser(username).ToList();
            foreach (string r in role)
            {
                roles += r + ",";
            }
            if (roles.Length > 0)
                roles = roles.Substring(0, (roles.Length - 1));
            return roles;
        }
    }
}