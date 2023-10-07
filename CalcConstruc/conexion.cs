using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace CalcConstruc
{
    internal class conexion
    {

        SqliteConnection con = new SqliteConnection("Data Source=db.db");

    }
}
