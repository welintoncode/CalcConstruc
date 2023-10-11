using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace CalcConstruc
{
    internal class conexion
    {      
        private SQLiteConnection con = new SQLiteConnection("Data Source =db.db; Pooling=true");

        public SQLiteConnection AbrirConexion()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            return con;
        }

        public SQLiteConnection CerrarConexion()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            return con;
        }

    }

}
