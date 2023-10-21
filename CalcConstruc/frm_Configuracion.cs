using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CalcConstruc
{
    public partial class frm_Configuracion : Form
    {
        //SQLiteConnection con = new SQLiteConnection("Data Source =db.db; Pooling=true");
        private conexion con = new conexion();
        public string desperdicio;
        public frm_Configuracion()
        {
            InitializeComponent();

            CB_tipoBlock();            
            CB_tipoMortero();

            

            string tipoBlock = "SELECT tb.descripcion FROM tipoblock tb INNER JOIN datosconfig dc ON tb.id = dc.idTipoBlock";
            SQLiteCommand cmd_tipoBlock = new SQLiteCommand(tipoBlock, con.AbrirConexion());
            SQLiteDataReader dr_tipoBlock = cmd_tipoBlock.ExecuteReader();

            if (dr_tipoBlock.Read())
            {
                cbTipoBlock_CF.Text = dr_tipoBlock[0].ToString();
            }

            string tipoMortero = "SELECT tm.descripcion FROM TipoMortero tm INNER JOIN datosconfig dc ON tm.id = dc.idTipoMortero";
            SQLiteCommand cmd_tipoMortero = new SQLiteCommand(tipoMortero, con.AbrirConexion());
            SQLiteDataReader dr_tipoMortero = cmd_tipoMortero.ExecuteReader();

            if (dr_tipoMortero.Read())
            {
                cbTipoMortero_CF.Text = dr_tipoMortero[0].ToString();
            }

            string datos = "select desperdicio, junta, precioBlock, precioCemento, PrecioArena from datosconfig";
            SQLiteCommand cmd_datos = new SQLiteCommand(datos, con.AbrirConexion());
            SQLiteDataReader dr_datos = cmd_datos.ExecuteReader();

            if (dr_datos.Read())
            {
                desperdicio = dr_datos[0].ToString();
                txDesperdicio_CF.Text = desperdicio;
                txJunta_CF.Text = dr_datos[1].ToString();
                txPrecioBlock_CF.Text = dr_datos[2].ToString();
                txPrecioCemento_CF.Text = dr_datos[3].ToString();
                txPrecioArena_CF.Text= dr_datos[4].ToString();
            }

            con.CerrarConexion();
        }



        private void CB_tipoBlock()
        {
            try
            {
                string query = "select descripcion from tipoblock";
                SQLiteCommand cmd = new SQLiteCommand(query, con.AbrirConexion());

                DataTable dt = new DataTable();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(dt);

                cbTipoBlock_CF.DataSource = dt;
                cbTipoBlock_CF.DisplayMember = "descripcion";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.CerrarConexion();
            }
        }

        private void CB_tipoMortero()
        {
            try
            {
                string query = "select descripcion from tipomortero";
                SQLiteCommand cmd = new SQLiteCommand(query, con.AbrirConexion());

                DataTable dt = new DataTable();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(dt);

                cbTipoMortero_CF.DataSource = dt;
                cbTipoMortero_CF.DisplayMember = "descripcion";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.CerrarConexion();
            }
        }

        private void btnGuardar_CF_Click(object sender, EventArgs e)
        {
            try
            {
                string consulta = "UPDATE datosconfig SET desperdicio = @desper, junta = @junta, precioBlock = @pBlock , precioCemento = @pCemento, PrecioArena = @pArena, idTipoBlock = @tBlock, idTipoMortero = @tMortero WHERE id = 1";
                SQLiteCommand cmd = new SQLiteCommand(consulta, con.AbrirConexion());

                cmd.Parameters.AddWithValue("@desper", txDesperdicio_CF.Text);
                cmd.Parameters.AddWithValue("@junta", txJunta_CF.Text);
                cmd.Parameters.AddWithValue("@pBlock", int.Parse(txPrecioBlock_CF.Text));
                cmd.Parameters.AddWithValue("@pCemento", int.Parse(txPrecioCemento_CF.Text));
                cmd.Parameters.AddWithValue("@pArena", int.Parse(txPrecioArena_CF.Text));
                cmd.Parameters.AddWithValue("@tBlock", cbTipoBlock_CF.SelectedIndex + 1);
                cmd.Parameters.AddWithValue("@tMortero", cbTipoMortero_CF.SelectedIndex + 1);

                int filasAfectadas = cmd.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Registro actualizado exitosamente.");
                }
                else
                {
                    MessageBox.Show("No se encontró un registro con ese ID.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.CerrarConexion();
            }
         }

    }


}
