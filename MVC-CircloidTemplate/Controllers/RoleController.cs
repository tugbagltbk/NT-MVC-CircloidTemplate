﻿using MVC_CircloidTemplate.App_Classes;
using MVC_CircloidTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_CircloidTemplate.Controllers
{
    [Authorize(Roles = "Admin")]

    public class RoleController : Controller
    {
       

        public RoleController()
        {
            ViewBag.RoleSelected = "selected";
        }
        NorthwindEntities ctx = new NorthwindEntities();
        // GET: Role
        public ActionResult Index()
        {
            List<string>rolesList= Roles.GetAllRoles().ToList();
            return View(rolesList);
        }
        public ActionResult AddRole(string message=null) //hiçbişey göndermezsek null al,default olarak
        {
            ViewBag.Message = message;
            return View();
        }
        //[HttpPost]
        //public ActionResult AddRole(RoleClass rc)
        //{
        //    string createMessage = "Kullanılmış rol adı";

        //    if (Roles.RoleExists(rc.RoleName))
        //    {
        //        ViewBag.createMessage = createMessage;
        //        return View();
        //    }
        //    else
        //    {
        //        Roles.CreateRole(rc.RoleName);
        //        return RedirectToAction("Index");
        //    }
        //}
        [HttpPost]
        [ActionName("AddRole")]
        public ActionResult AddRolePost(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return RedirectToAction("AddRole", new { message = "Rol boş olamaz" });

            if (Roles.RoleExists(role))
                return RedirectToAction("AddRole", new { message = "Rol zaten kayıtlı" });

             Roles.CreateRole(role);
             return RedirectToAction("AddRole", new { message = "Başarılı" });
        }
        //public ActionResult DeleteRole()
        //{
        //  return View();
        //}
        [HttpPost]
        public string DeleteRole(string id)
        {
            try
            {
                Roles.DeleteRole(id);
                ctx.SaveChanges();
              //  return RedirectToAction("Index");
                return "OK";
            }
            catch (Exception)
            {
                 return "ERROR";
               // throw;

            }
           
        }
        //public ActionResult UpdateRole(string id)
        //{

        //    Roles.

        //    return View(prd);
        //}

        [HttpPost]
        public ActionResult UpdateRole(Product p)//(Category c)
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