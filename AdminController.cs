using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanOto.Models;

namespace WebBanOto.Controllers
{
    public class AdminController : Controller
    {
        dbQLBanOtoDataContext db = new dbQLBanOtoDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Lienhe()
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                return View(db.LIENHEs.ToList());
            }
        }
        public ActionResult Xe(int? page,int? MaHX,int? MaDX)
        {
           
           
            ViewBag.MaDX = new SelectList(db.DONGXEs.ToList().OrderBy(n => n.TenDX), "MaDX", "TenDX");
            ViewBag.MaHX = new SelectList(db.HANGXEs.ToList().OrderBy(n => n.TenHX), "MaHX", "TenHX");
            
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else if(MaHX != null || MaDX != null)
            {
                var xe = from l in db.XEs
                         where l.MaHX == MaHX 
                         where l.MaDX == MaDX
                         select l;
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(xe.ToList().OrderBy(n => n.Maxe).ToPagedList(pageNumber, pageSize));
            }

            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;

                return View(db.XEs.ToList().OrderBy(n => n.Maxe).ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult Hangxe(int? page)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;

                return View(db.HANGXEs.ToList().OrderBy(n => n.MaHX).ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult Dongxe(int? page)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;

                return View(db.DONGXEs.ToList().OrderBy(n => n.MaDX).ToPagedList(pageNumber, pageSize));
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
           
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (ad)        

                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Themxemoi()
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                ViewBag.MaDX = new SelectList(db.DONGXEs.ToList().OrderBy(n => n.TenDX), "MaDX", "TenDX");
                ViewBag.MaHX = new SelectList(db.HANGXEs.ToList().OrderBy(n => n.TenHX), "MaHX", "TenHX");
                return View();
            }
        }
        [HttpGet]
        public ActionResult Themhangxe()
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Themdongxe()
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                return View();
            }
        }
        public ActionResult Themlienhe()
        {


            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themxemoi(XE xe, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Dua du lieu vao dropdownload
                ViewBag.MaDX = new SelectList(db.DONGXEs.ToList().OrderBy(n => n.TenDX), "MaDX", "TenDX");
                ViewBag.MaHX = new SelectList(db.HANGXEs.ToList().OrderBy(n => n.TenHX), "MaHX", "TenHX");
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh xe";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);
                        }
                        xe.Anhxe = fileName;
                        //Luu vao CSDL
                        db.XEs.InsertOnSubmit(xe);
                        db.SubmitChanges();
                    }
                    return RedirectToAction("Xe");
                }
            }
        }
        [HttpPost]
        
        public ActionResult Themhangxe(HANGXE hx)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {

                db.HANGXEs.InsertOnSubmit(hx);
                db.SubmitChanges();

                return RedirectToAction("Hangxe");
            }
            
        }
        [HttpPost]
        
        public ActionResult Themdongxe(DONGXE dx)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {

                db.DONGXEs.InsertOnSubmit(dx);
                db.SubmitChanges();

                return RedirectToAction("Dongxe");
            }
        }
        [HttpPost]

        public ActionResult Themlienhe(LIENHE lh)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {

                db.LIENHEs.InsertOnSubmit(lh);
                db.SubmitChanges();

                return RedirectToAction("Lienhe");
            }

        }
        public ActionResult Chitietxe(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach theo ma
                XE xe = db.XEs.SingleOrDefault(n => n.Maxe == id);
                ViewBag.Maxe = xe.Maxe;
                if (xe == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(xe);
            }
        }
        [HttpGet]
        public ActionResult Xoaxe(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                XE xe = db.XEs.SingleOrDefault(n => n.Maxe == id);
                ViewBag.Maxe = xe.Maxe;
                if (xe == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(xe);
            }
        }
        [HttpGet]
        public ActionResult Xoahx(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                HANGXE hx = db.HANGXEs.SingleOrDefault(n => n.MaHX == id);
                ViewBag.MaHX = hx.MaHX;
                if (hx == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(hx);
            }
        }
        [HttpGet]
        public ActionResult Xoadx(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                DONGXE dx = db.DONGXEs.SingleOrDefault(n => n.MaDX == id);
                ViewBag.MaDX = dx.MaDX;
                if (dx == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(dx);
            }
        }

        [HttpPost, ActionName("Xoaxe")]
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                XE xe = db.XEs.SingleOrDefault(n => n.Maxe == id);
                ViewBag.Maxe = xe.Maxe;
                if (xe == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.XEs.DeleteOnSubmit(xe);
                db.SubmitChanges();
                return RedirectToAction("Xe");
            }
        }
        [HttpPost, ActionName("Xoahx")]
        public ActionResult Xacnhanxoahx(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                HANGXE hx = db.HANGXEs.SingleOrDefault(n => n.MaHX == id);
                ViewBag.MaHX = hx.MaHX;
                if (hx == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.HANGXEs.DeleteOnSubmit(hx);
                db.SubmitChanges();
                return RedirectToAction("Hangxe");
            }
        }
        [HttpPost, ActionName("Xoadx")]
        public ActionResult Xacnhanxoadx(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                DONGXE dx = db.DONGXEs.SingleOrDefault(n => n.MaDX == id);
                ViewBag.MaDX = dx.MaDX;
                if (dx == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.DONGXEs.DeleteOnSubmit(dx);
                db.SubmitChanges();
                return RedirectToAction("Dongxe");
            }
        }
        [HttpGet]
        public ActionResult Sua(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach theo ma
                XE xe = db.XEs.SingleOrDefault(n => n.Maxe == id);
                ViewBag.Maxe = xe.Maxe;
                if (xe == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }


                ViewBag.MaDX = new SelectList(db.DONGXEs.ToList().OrderBy(n => n.TenDX), "MaDX", "TenDX", xe.MaDX);
                ViewBag.MaHX = new SelectList(db.HANGXEs.ToList().OrderBy(n => n.TenHX), "MaHX", "TenHX", xe.MaHX);
                return View(xe);
            }
        }
        [HttpPost,ActionName("Sua")]
        [ValidateInput(false)]
        public ActionResult Capnhat(XE xe, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Dua du lieu vao dropdownload
                ViewBag.MaDX = new SelectList(db.DONGXEs.ToList().OrderBy(n => n.TenDX), "MaDX", "TenDX");
                ViewBag.MaHX = new SelectList(db.HANGXEs.ToList().OrderBy(n => n.TenHX), "MaHX", "TenHX");
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh xe";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);
                        }
                        xe.Anhxe = fileName;
                        //Luu vao CSDL   
                        UpdateModel(xe);
                        db.SubmitChanges();

                        
                    }
                }
                return RedirectToAction("Xe");
            }
        }
       
        public ActionResult Yeucau(int? page,int? MaPT)
        {
            ViewBag.MaPT = new SelectList(db.PHUONGTHUCs.ToList().OrderBy(n => n.TenPT),"MaPT", "TenPT");
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
           
            else if (MaPT != null)
            {
                var yc = from x in db.TUVANs
                         where x.MaPT == MaPT
                         select x;
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(yc.ToList().OrderBy(n => n.MaTV).ToPagedList(pageNumber, pageSize));
            }
            else 
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;

                return View(db.TUVANs.ToList().OrderByDescending(n => n.MaTV).ToPagedList(pageNumber, pageSize));
            }
           
        }
        private List<YEUCAU> Laytv()
        {
            return db.YEUCAUs.OrderByDescending(a => a.Ngay).ToList();
        }
        public ActionResult Dangki(int? page, string ngay)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else if (String.IsNullOrEmpty(ngay))
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                var tv = Laytv();
                return View(tv.ToPagedList(pageNumber, pageSize));
            }
            else {
                var dk = from x in db.YEUCAUs
                         where x.Ngay == DateTime.Parse(ngay.ToString())
                         select x;
                int pageNumber = (page ?? 1);
                int pageSize = 7;
               
                return View(dk.ToList().OrderBy(n => n.MaYC).ToPagedList(pageNumber, pageSize));
            }
           

        }
        public ActionResult Gopy(int? page)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;

                return View(db.GOPies.ToList().OrderByDescending(n => n.MaGY).ToPagedList(pageNumber, pageSize));
            }
        }
        [HttpGet]
        public ActionResult Xoayc(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                TUVAN tv = db.TUVANs.SingleOrDefault(n => n.MaTV == id);
                ViewBag.MaTV = tv.MaTV;
                if (tv == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(tv);
            }
        }
        [HttpGet]
        public ActionResult Xoagy(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                GOPY gy = db.GOPies.SingleOrDefault(n => n.MaGY == id);
                ViewBag.MaGY = gy.MaGY;
                if (gy == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(gy);
            }
        }
        [HttpGet]
        public ActionResult Xoadk(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                YEUCAU yc = db.YEUCAUs.SingleOrDefault(n => n.MaYC == id);
                ViewBag.MaYC = yc.MaYC;
                if (yc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(yc);
            }
        }
        [HttpPost, ActionName("Xoayc")]
        public ActionResult Xacnhanxoayc(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                TUVAN tv = db.TUVANs.SingleOrDefault(n => n.MaTV == id);
                ViewBag.MaTV = tv.MaTV;
                if (tv == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.TUVANs.DeleteOnSubmit(tv);
                db.SubmitChanges();
                return RedirectToAction("Yeucau");
            }
        }
        [HttpPost, ActionName("Xoagy")]
        public ActionResult Xacnhanxoagy(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                GOPY gy = db.GOPies.SingleOrDefault(n => n.MaGY == id);
                ViewBag.MaGY = gy.MaGY;
                if (gy == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.GOPies.DeleteOnSubmit(gy);
                db.SubmitChanges();
                return RedirectToAction("Gopy");
            }
        }
        [HttpPost, ActionName("Xoadk")]
        public ActionResult Xacnhanxoadk(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                YEUCAU yc = db.YEUCAUs.SingleOrDefault(n => n.MaYC == id);
                ViewBag.MaYC = yc.MaYC;
                if (yc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.YEUCAUs.DeleteOnSubmit(yc);
                db.SubmitChanges();
                return RedirectToAction("Dangki");
            }
        }
        [HttpGet]
        public ActionResult Sualh(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach theo ma
                LIENHE lh = db.LIENHEs.SingleOrDefault(n => n.MaLH == id);
                ViewBag.MaLH = lh.MaLH;
                if (lh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                //Dua du lieu vao dropdownList
                //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude

                return View(lh);
            }
        }
        [HttpPost]
        
        public ActionResult Sualh(LIENHE lh)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {

                Query qr = new Query();
                qr.UpdateLh(lh);


                return RedirectToAction("Lienhe");
            }
            
        }
        [HttpGet]
        public ActionResult Xoalh(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                LIENHE lh = db.LIENHEs.SingleOrDefault(n => n.MaLH == id);
                ViewBag.MaLH = lh.MaLH;
                if (lh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(lh);
            }
        }
        [HttpPost, ActionName("Xoalh")]
        public ActionResult Xacnhanxoalh(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach can xoa theo ma
                LIENHE lh = db.LIENHEs.SingleOrDefault(n => n.MaLH == id);
                ViewBag.MaLH = lh.MaLH;
                if (lh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                db.LIENHEs.DeleteOnSubmit(lh);
                db.SubmitChanges();
                return RedirectToAction("Lienhe");
            }
        }
        [HttpGet]
        public ActionResult SuaHx(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach theo ma
                HANGXE hx = db.HANGXEs.SingleOrDefault(n => n.MaHX == id);
                ViewBag.MaHX = hx.MaHX;
                if (hx == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                //Dua du lieu vao dropdownList
                //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude

                return View(hx);
            }
        }
        [HttpPost]

        public ActionResult SuaHx(HANGXE hx)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {

                Query qr = new Query();
                qr.UpdateHx(hx);


                return RedirectToAction("Hangxe");
            }

        }
        [HttpGet]
        public ActionResult SuaDx(int id)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {
                //Lay ra doi tuong sach theo ma
                DONGXE dx = db.DONGXEs.SingleOrDefault(n => n.MaDX == id);
                ViewBag.MaDX = dx.MaDX;
                if (dx == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                //Dua du lieu vao dropdownList
                //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude

                return View(dx);
            }
        }
        [HttpPost]

        public ActionResult SuaDx(DONGXE dx)
        {
            if (Session["Taikhoanadmin"] == null) { return RedirectToAction("Login", "Admin"); }
            else
            {

                Query qr = new Query();
                qr.UpdateDx(dx);


                return RedirectToAction("Dongxe");
            }

        }
       
       
    }
}