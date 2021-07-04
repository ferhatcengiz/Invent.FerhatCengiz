using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Invent.FerhatCengiz.ADO
{
    public class DbCon
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["Constr"].ConnectionString);
        }
    }
}