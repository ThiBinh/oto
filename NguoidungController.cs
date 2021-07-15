using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanOto.Models;
using WebBanOto.ViewModels;

namespace WebBanOto.Controllers
{
    public class NguoidungController : Controller
    {
        dbQLBanOtoDataContext db = new dbQLBanOtoDataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Feedback()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Feedback(FormCollection collection, GOPY g)
        {
            
            var hoten = collection["Hoten"];
            var dienthoai = collection["Dienthoai"];
            var email = collection["Email"];
            var tieude = collection["Tieude"];
            var noidung = collection["Noidung"];

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "You need to enter Full name";
            }

            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi2"] = "You need to enter Email";
            }
             if (String.IsNullOrEmpty(noidung))
            {
                ViewData["Loi3"] = "You need to enter Content or Question";
            }
            else
            {
                

                g.HoTen = hoten;
                g.Dienthoai = dienthoai;
                g.Email = email;
                g.Tieude = tieude;
                g.NoiDung = noidung;
                db.GOPies.InsertOnSubmit(g);
                db.SubmitChanges();
                ViewBag.Thongbao = "Thank you for your feedback";
                return RedirectToAction("Index","CarStore");
                
            }
            return this.Feedback();
        }
        [HttpGet]
        public ActionResult Re()
        {
            var viewModel = new PTViewModels
            {
                Phuongthucs = db.PHUONGTHUCs.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Re(PTViewModels viewModel)
        {
            
            if (!ModelState.IsValid)
            {
               viewModel.Phuongthucs = db.PHUONGTHUCs.ToList();
                return View("Re", viewModel);
            }
           
                var tuvan = new TUVAN
                {
                    HoTen = viewModel.Hoten,
                    Dienthoai = viewModel.Dienthoai,
                    Email = viewModel.Email,
                    MaPT = viewModel.PHUONGTHUC
                    

                };
                db.TUVANs.InsertOnSubmit(tuvan);


                db.SubmitChanges();
                
                return RedirectToAction("XNyeucau", "Nguoidung");


        }
        public ActionResult Contact()
        {
            var f = from ft in db.LIENHEs select ft;
            return View(f);
        }
        [HttpGet]
        public ActionResult Test()
        {

            var viewModel = new DKViewModels
            {
                Gios = db.GIOs.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Test(DKViewModels viewModel, int id)
        {
            
            
            if (!ModelState.IsValid)
            {
                viewModel.Gios = db.GIOs.ToList();
                return View("Test", viewModel);
            }
           
            var yeucau = new YEUCAU
            {
                HoTen = viewModel.Hoten,
                Dienthoai = viewModel.Dienthoai,
                Email = viewModel.Email,
                Ngay = DateTime.Parse(viewModel.Ngay),
                MaG = viewModel.GIO,
                Maxe = id

            };
            
            db.YEUCAUs.InsertOnSubmit(yeucau);


            db.SubmitChanges();
            
            return RedirectToAction("XNyeucau", "Nguoidung");


        }
        public ActionResult XNyeucau()
        {
            return View();
        }
    }
}