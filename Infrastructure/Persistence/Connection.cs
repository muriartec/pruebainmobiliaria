using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Infrastructure.Persistence
{
    internal class Connection
    {
        public static SqlConnection ConnectToSQL()
        {
            //string ls_connection = Environment.GetEnvironmentVariable("SECRET_CONNECTIONSTRINGS");
            string ls_connection = "Server=localhost;Database=BD_INMUEBLES;User ID=sa;Password=dotaallstar;Trusted_Connection=False;";
            return new SqlConnection(ls_connection);
        }
    }
}
