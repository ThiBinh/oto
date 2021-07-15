using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebBanOto.Models
{
    public class Query
    {



        private DBConnection db;
        public Query()
        {
            db = new DBConnection();
        }
       
        public bool UpdateLh(LIENHE model)
        {
            string sql = "Update LIENHE set Diachi = N'" + model.Diachi + "',Email = '" + model.Email + "',SDT = '" + model.SDT + "',FB = '" + model.FB + "',INS = '" + model.INS + "' where MaLH = " + model.MaLH;
            SqlConnection con = db.GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();
            var kq = cmd.ExecuteNonQuery();
            con.Close();
            return kq > 0;
        }
        public bool UpdateHx(HANGXE model)
        {
            string sql = "Update HANGXE set TenHX = N'" + model.TenHX + "',Diachi = N'" + model.Diachi + "',DienThoai = '" + model.DienThoai  + "' where MaHX = " + model.MaHX;
            SqlConnection con = db.GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();
            var kq = cmd.ExecuteNonQuery();
            con.Close();
            return kq > 0;
        }
        public bool UpdateDx(DONGXE model)
        {
            string sql = "Update DONGXE set TenDX = '" + model.TenDX  + "' where MaDX = " + model.MaDX;
            SqlConnection con = db.GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();
            var kq = cmd.ExecuteNonQuery();
            con.Close();
            return kq > 0;
        }
        public bool UpdateXe(XE model)
        {
            string sql = "Update XE set Tenxe = '" + model.Tenxe + "',Giaban = '" + model.Giaban + "',Mota = N'" + model.Mota + "',Anhxe = '" + model.Anhxe + "',Namsanxuat = convert(datetime,'" + model.Namsanxuat + "',103),Soluongton = '" + model.Soluongton + "',Uudai = N'" + model.Uudai + "',MaHX = '" + model.MaHX + "' ,MaDX = '" + model.MaDX + "' where Maxe = " + model.Maxe;
            SqlConnection con = db.GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();
            var kq = cmd.ExecuteNonQuery();
            con.Close();
            return kq > 0;
        }
        
    }
}