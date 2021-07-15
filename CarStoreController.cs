using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanOto.Models;

using PagedList;
using PagedList.Mvc;

namespace WebBanOto.Controllers
{
    public class CarStoreController : Controller
    {
        // GET: CarStore
        dbQLBanOtoDataContext data = new dbQLBanOtoDataContext();
        private List<XE> Layxemoi(int count)
        {
            return data.XEs.OrderByDescending(a => a.Namsanxuat).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 6;
            //Tao bien so trang
            int pageNum = (page ?? 1);
            var xemoi = Layxemoi(10);
            return View(xemoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult DongXe()
        {
            var dongxe = from dx in data.DONGXEs select dx;
            return PartialView(dongxe);
        }
        public ActionResult HangXe()
        {
            var hangxe = from hx in data.HANGXEs select hx;
            return PartialView(hangxe);
        }
        public ActionResult SPTheoDX(int id)
        {
            
            var xe = from x in data.XEs where x.MaDX == id select x;
            return View(xe);
        }
        public ActionResult SPTheoHX(int id)
        {
           
            var xe = from x in data.XEs where x.MaHX == id select x;
            return View(xe);
        }

        public ActionResult Details(int id)
        {
            var xe = from x in data.XEs
                       where x.Maxe == id
                       select x;
           
            return View(xe.Single());
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Video()
        {
            return View();
        }
        public ActionResult Discount()
        {

            var xe = from x in data.XEs
                     where x.Uudai != null
                     select x;

            return View(xe);
        }
        private List<XE> Layxe()
        {
            return data.XEs.ToList();
        }
        public ActionResult Product(int ? page)
        {
            int pageSize = 6;
            //Tao bien so trang
            int pageNum = (page ?? 1);
            var xe = Layxe();
            return View(xe.ToPagedList(pageNum, pageSize));
        }
    }
}