using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DU_AN.Models;

namespace DU_AN.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        DBQLHiglandEntities database = new DBQLHiglandEntities();
        // GET: Product
        public ActionResult Index()
        {
            return View(database.Products.ToList());
        }
    }
}