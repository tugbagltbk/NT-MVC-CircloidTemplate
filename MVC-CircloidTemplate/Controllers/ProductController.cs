using MVC_CircloidTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_CircloidTemplate.Controllers
{
    [Authorize(Roles = "Üye")]

    public class ProductController : Controller
    {

        public ProductController()
        {
            ViewBag.ProductSelected = "selected";
        }

        // GET: Product
        NorthwindEntities ctx = new NorthwindEntities();
        public ActionResult Index()
        {
            List<Product> prdList = ctx.Products.ToList();
            return View(prdList);
        }
        public ActionResult AddProduct()
        {
            //yeni bir view kategori listelenmesi edarikçi listelenmesi,seçicek get yapıcak, post etmek istedğimizde hangi action çalışcak. Bu neenle hem get hem post action gerekli
            ViewBag.catList = ctx.Categories.ToList(); //List yazmak yerine viewbag ile yapıyoruz.
            ViewBag.supList = ctx.Suppliers.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product p)
        {
            ctx.Products.Add(p);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }
        //Sil işlemini üç farklı yolla yapacağız. İlk çözümümüz sil butonuna basılınca yeni bir View açılacak.Yani kullanıcı yeni bir sayfaya yönlendirilecek ve evet derse silinecek.
        //2.yol, sil butonuna basılınca yukarıda alert kutusu çıkacak ve kayıt silinsin mi? diye soracak evet denirse silinecek. Bu yöntemin dezavantajı alert kutusu birkaç kez görüntülendikten sonra browser otomatik olarak alert kutusunun altına checkbox ekliyor ve bu mesajı tekrar gösterme seçeneği sunuyor. Eğer kullanıcı checkbox ı işaretlerse tekrar alert kutusu gözükmeyeceği için silme işlemi gerçekleştirilemiyor.(AJAX KODU YAZILACAK)
        //3.yol, Sil butonuna basılınca küçük bir pencere açılacak (alert kutusu değil) yani başka bir sayfaya yönlendirilmeyecek,evet seçilirse silme işlemi gerçekleştirilecek. (AJAX KODU YAZILACAK)
        public ActionResult DeleteProduct(int prdID)
        {
            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == prdID);   
            return View(prd);
        }
        [HttpPost]
        public ActionResult DeleteProduct(Product p)
        {
            Product prod = ctx.Products.FirstOrDefault(x => x.ProductID == p.ProductID);
            ctx.Products.Remove(prod);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UpdateProduct(int id)
        {

            Product prd = ctx.Products.FirstOrDefault(x => x.ProductID == id);

            return View(prd);
        }

        [HttpPost]
        public ActionResult UpdateProduct(Product p)//(Category c)
        {
            if (ModelState.IsValid)
            {
                Product pr = ctx.Products.Find(p.ProductID);
                pr.ProductName = p.ProductName;
                pr.UnitPrice = p.UnitPrice;
                
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}