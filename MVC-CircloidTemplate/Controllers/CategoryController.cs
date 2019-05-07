using MVC_CircloidTemplate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_CircloidTemplate.Controllers
{
    [Authorize(Roles = "Üye")]
    public class CategoryController : Controller
    {
        public CategoryController()
        {
            ViewBag.CategorySelected = "selected";
        }
        NorthwindEntities ctx = new NorthwindEntities();
        // GET: Category
        public ActionResult Index()
        {
            List<Category> ctgList = ctx.Categories.ToList();
            return View(ctgList);
        }
        public ActionResult AddCategory()
        {            
            ViewBag.catList = ctx.Categories.ToList(); 
            return View();
        }
        [HttpPost]
        //Category Picture nesnesi Database'de byte[] şeklinde tutulduğu için seçilen resmi byte[]'e çevrilmesini sağlayan method
        public ActionResult AddCategory([Bind(Include ="CategoryName,Description")] Category cat, HttpPostedFileBase Picture) //(Category c)
        {
            if (Picture == null)
                return View();
            cat.Picture = ConvertToBytes(Picture);
            ctx.Categories.Add(cat);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes(image.ContentLength);
            byte[] bytes = new byte[imageBytes.Length + 78];
            Array.Copy(imageBytes, 0, bytes, 78, imageBytes.Length);
            return bytes;
        }
        //public ActionResult DeleteCategory(int ctgID)
        //{
        //    Category cat = ctx.Categories.FirstOrDefault(x => x.CategoryID == ctgID);
        //    return View(cat);
        //}
        [HttpPost]
        //public ActionResult DeleteCategory(Category c)
        //{
        //    Category ct = ctx.Categories.FirstOrDefault(x => x.CategoryID == c.CategoryID);
        //    ctx.Categories.Remove(ct);
        //    ctx.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        public ActionResult DeleteCategory(int id)
        {
            Category ct = ctx.Categories.FirstOrDefault(x => x.CategoryID == id);
            ctx.Categories.Remove(ct);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UpdateCategory(int id)
        {

            Category cat = ctx.Categories.FirstOrDefault(x => x.CategoryID == id);

            return View(cat);
        }

        [HttpPost]
        public ActionResult UpdateCategory([Bind(Include = "CategoryID,CategoryName,Description")] Category cat, HttpPostedFileBase Picture)//(Category c)
        {
            if (ModelState.IsValid)
            {
                Category ct = ctx.Categories.Find(cat.CategoryID);
                ct.CategoryName = cat.CategoryName;
                ct.Description = cat.Description;
                if (Picture != null)
                {
                    ct.Picture = ConvertToBytes(Picture);
                }
                ctx.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}