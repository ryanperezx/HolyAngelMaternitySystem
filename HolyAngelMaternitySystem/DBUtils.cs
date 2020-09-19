using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace HolyAngelMaternitySystem
{
    class DBUtils
    {
        public static SqlConnection GetDBConnection()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string datasource = "192.168.0.100";
            string database = "HolyAngelMaternityPolyclinicDB;" +
                "User ID=admin;" +
                "Password=Deaths123";

            return DBSQLServerUtils.GetDBConnection(datasource, database);
        }
    }
}
