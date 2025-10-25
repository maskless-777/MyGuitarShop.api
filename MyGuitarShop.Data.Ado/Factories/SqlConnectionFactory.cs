using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace MyGuitarShop.Data.Ado.Factories
{
    public class SqlConnectionFactory(string connectionString)
    {
        public SqlConnection OpenSqlConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
