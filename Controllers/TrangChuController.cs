using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DU_AN.Controllers
{
    public class TrangChuController : Controller
    {
        // GET: TrangChu
        public ActionResult QuanCF()
        {
            return View();
        }

        public ActionResult ThucDon()
        {
            return View();
        }
       
        public ActionResult CongDong()
        {
            return View();
        }     

        public ActionResult MuaNgay()
        {
            return View();
        }
    }
}