using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Infrastructure.DAL;

public class BdHealthMedSession :IDisposable
{
    public IDbConnection Connection { get; set; }
    public IDbTransaction Transaction { get; set; }

    public BdHealthMedSession(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
    }

    public void Dispose() => Connection?.Close();

}
