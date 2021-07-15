using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebBanOto.Models
{
    public class DBConnection
    {
        String strCon;
        public DBConnection()
        {
            strCon = ConfigurationManager.ConnectionStrings["QLBANOTOConnectionString"].ConnectionString;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(strCon);
        }
    }
}