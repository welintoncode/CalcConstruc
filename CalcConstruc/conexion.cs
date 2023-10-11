using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace CalcConstruc
{
    internal class conexion
    {      
        public void conect()
        {
            SQLiteConnection con = new SQLiteConnection("Data Source =db.db");
            try
            {
                con.Open();
                MessageBox.Show("Conectado a la base de datos");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Conectado a la base de datos" + ex.Message);
            }
        
        }

    }

}
