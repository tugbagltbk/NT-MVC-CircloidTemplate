using MVC_CircloidTemplate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_CircloidTemplate.Controllers
{
    [Authorize(Roles = "Üye")]

    public class SupplierController : Controller
    {
        public SupplierController()
        {
            ViewBag.SupplierSelected = "selected";
        }
        NorthwindEntities ctx = new NorthwindEntities();
        // GET: Supplier
        public ActionResult Index()
        {
            List<Supplier> supList = ctx.Suppliers.ToList();
            return View(supList);
        }
        public ActionResult AddSupplier()
        {           
            ViewBag.supList = ctx.Suppliers.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddSupplier(Supplier sp)
        {
            ctx.Suppliers.Add(sp);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }
        //public ActionResult DeleteSupplier(int id) //GEREK YOK ÇÜNKÜ VIEW AÇMIYOR,YENİ BİR SAYFA AÇMIYOR
        //{
        //    Supplier spl = ctx.Suppliers.FirstOrDefault(x => x.SupplierID == id);
        //    return View(spl);
        //}
        //Bu methodun içinde oluşan hata Ajax'ı etkilemez.Ajax için success Ajax'ın doğru bir şekilde action'a ulaşmış olmasıyla ilgilidir. Bu methodda veritabanındaki ilişkilerden dolayı kayıt silinmez ve benzeri hatalar Ajax'ı ilgilendirmez. Bu yüzden bu method içinde oluşan hatalarla ilgili Ajax tarafına bilgi göndermeliyiz.
        [HttpPost]
        public string DeleteSupplier(int id)
        {
            try
            {
                Supplier sup = ctx.Suppliers.Find(id);
                ctx.Suppliers.Remove(sup);
                ctx.SaveChanges();

                return "OK";
            }
            catch (Exception)
            {
                return "ERROR";
            }          
        }       
        public ActionResult UpdateSupplier(int spID)
        {
            Supplier sup = ctx.Suppliers.FirstOrDefault(x => x.SupplierID == spID);
            return View(sup);
        }

        [HttpPost]
        public ActionResult UpdateSupplier([Bind(Include ="SupplierID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone")]Supplier s)
        {
            //güncelleme diğer yol
            //if(ModelState.IsValid)
            //{
            //    ctx.Entry(s).State = EntityState.Modified; //şuanda aktif olan supplier entitysinin contentini modified yapıyo
            //    ctx.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return RedirectToAction("UpdateSupplier", new { id = s.SupplierID });

            Supplier sp = ctx.Suppliers.FirstOrDefault(x => x.SupplierID == s.SupplierID);
            sp.CompanyName=s.CompanyName;
            sp.ContactName=s.ContactName;

            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
