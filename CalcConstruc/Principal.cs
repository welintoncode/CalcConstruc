using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data.SQLite;


namespace CalcConstruc
{   
    public partial class Principal : Form
    {
        frm_Configuracion conf = new frm_Configuracion();

        double anchoBlock;
        double alturaBlock;
        double largoBlock;
        double areaBlock = 0;
        double precioBlock;
        double precioCemento;
        double precioArena;
        double totalCostos = 0;
        double tipoArea = 0;

        double areaPared = 0, areaPared2 = 0, areParedTotal, areaParedMenosHuecos;
        double blockPorMetro = 0, blockPorMetroMasDesperdicio = 0, cantidadBlockTotal = 0, cantidadBlockTotalMasDesperdicio = 0;
        double volumenMortero = 0;
        double cementokg, arenaM3, aguaL, fundaCemento = 42.5;
        double cementoPorMetro = 0, cementoPorMetroMasDesperdicio = 0;
        double arenaPorMetro = 0, arenaPorMetroMasDesperdicio = 0;
        double aguaPorMetro = 0, aguaPorMetroMasDesperdicio = 0;

        double desperdicio;
        double junta;
        double tipoBlock;
        
 
        frm_Configuracion formConfig = new frm_Configuracion();
        frm_AcercaDe formAcerca = new frm_AcercaDe();
        private conexion con = new conexion();
        
        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formAcerca.ShowDialog();            
        }

        private void configuracionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formConfig.ShowDialog();
        }


        public Principal()
        {
            InitializeComponent();

            cargaDatos();
            cargaDatosBlock();
            cargaDatosTipoMortero();
        }

        public void cargaDatos()
        {
            string datos = "select desperdicio, junta, precioBlock, precioCemento, PrecioArena from datosconfig";
            SQLiteCommand cmd_datos = new SQLiteCommand(datos, con.AbrirConexion());
            SQLiteDataReader dr_datos = cmd_datos.ExecuteReader();

            if (dr_datos.Read())
            {
                desperdicio = Convert.ToDouble(dr_datos[0].ToString());
                junta = Convert.ToDouble(dr_datos[1].ToString());
                precioBlock = Convert.ToDouble(dr_datos[2].ToString());
                precioCemento = Convert.ToDouble(dr_datos[3].ToString());
                precioArena = Convert.ToDouble(dr_datos[4].ToString());
            }

            con.CerrarConexion();
        }

        public void cargaDatosBlock()
        {
            string datos = "SELECT tb.descripcion, tb.anchoBlock, tb.alturaBlock, tb.largoBlock FROM tipoblock tb INNER JOIN datosconfig dc ON tb.id = dc.idTipoBlock";
            SQLiteCommand cmd_datos = new SQLiteCommand(datos, con.AbrirConexion());
            SQLiteDataReader dr_datos = cmd_datos.ExecuteReader();

            if (dr_datos.Read())
            {
                tipoBlock = Convert.ToDouble(dr_datos[0].ToString());
                anchoBlock = Convert.ToDouble(dr_datos[1].ToString());
                alturaBlock = Convert.ToDouble(dr_datos[2].ToString());
                largoBlock = Convert.ToDouble(dr_datos[3].ToString());
            }

            con.CerrarConexion();
        }

        public void cargaDatosTipoMortero()
        {
            string datos = "SELECT tm.descripcion, tm.cementokg, tm.arenaM3, tm.aguaL FROM TipoMortero tm INNER JOIN datosconfig dc ON tm.id = dc.idTipoMortero";
            SQLiteCommand cmd_datos = new SQLiteCommand(datos, con.AbrirConexion());
            SQLiteDataReader dr_datos = cmd_datos.ExecuteReader();

            if (dr_datos.Read())
            {
                //tipoBlock = Convert.ToDouble(dr_datos[0].ToString());
                cementokg = Convert.ToDouble(dr_datos[1].ToString());
                arenaM3 = Convert.ToDouble(dr_datos[2].ToString());
                aguaL = Convert.ToDouble(dr_datos[3].ToString());
            }

            con.CerrarConexion();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txBase2D.Hide(); txAltura2D.Hide(); lbB2.Hide(); lbA2.Hide();
        }

        //VALIDA EL TIPO DE BLOCK
        private void cbTipoBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cbTipoBlock.SelectedItem != null)
            //{
            //    string valor = cbTipoBlock.SelectedItem.ToString();

            //    switch (Convert.ToInt32(valor))
            //    {
            //        case 8:
            //            anchoBlock = 0.20;
            //            alturaBlock = 0.20;
            //            largoBlock = 0.40;
            //            break;
            //        case 6:
            //            anchoBlock = 0.15;
            //            alturaBlock = 0.20;
            //            largoBlock = 0.40;
            //            break;
            //        case 4:
            //            anchoBlock = 0.10;
            //            alturaBlock = 0.20;
            //            largoBlock = 0.40;
            //            break;
            //    }
            //}
        }

        //VALIDA EL TIPO DE MORTERO
        private void cbTipoMorteroD_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cbTipoMorteroD.SelectedItem != null)
            //{
            //    int selectedIndex = cbTipoMorteroD.SelectedIndex;

            //    switch (Convert.ToInt32(selectedIndex))
            //    {
            //        case 0:
            //            cementokg = 610; arenaM3 = 0.97; aguaL = 250;
            //            break;
            //        case 1:
            //            cementokg = 454; arenaM3 = 1.10; aguaL = 250;
            //            break;
            //        case 2:
            //            cementokg = 364; arenaM3 = 1.16; aguaL = 240;
            //            break;
            //    }
            //}
        }

        //VALIDA TIPO DE AREA
        private void cbTipoAreaD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTipoAreaD.SelectedItem != null)
            {
                string valor = cbTipoAreaD.SelectedItem.ToString();

                switch (Convert.ToString(valor))
                {
                    case "Cuadrado":
                        lbBaseD.Text = "Base";
                        txBase2D.Hide(); txAltura2D.Hide(); lbB2.Hide(); lbA2.Hide();
                        txBase2D.Text = "0"; txAltura2D.Text = "0";
                        txAltura.Visible = true; lbAlturaD.Visible = true;
                        areaPared = (Convert.ToDouble(txBase.Text) * Convert.ToDouble(txAltura.Text));
                        areParedTotal = areaPared * 4;
                        if (chCompleta.Checked)
                        {
                            areParedTotal.ToString();
                        }
                        else
                        {
                            areParedTotal = (Convert.ToDouble(txBase.Text) * Convert.ToDouble(txAltura.Text));
                        }
                        break;
                    case "Rectangulo":
                        chCompleta.Checked = true;
                        lbBaseD.Text = "Base";
                        txAltura.Visible = true; txBase2D.Visible = true; txAltura2D.Visible = true; lbAlturaD.Visible = true; lbB2.Visible = true; lbA2.Visible = true;
                        areaPared = (Convert.ToDouble(txBase.Text) * Convert.ToDouble(txAltura.Text));
                        areaPared2 = (Convert.ToDouble(txBase2D.Text) * Convert.ToDouble(txAltura2D.Text));
                        areParedTotal = (areaPared * 2) + (areaPared2 * 2);

                        break;
                    case "Otro":
                        chCompleta.Checked = false;
                        txAltura.Hide(); txBase2D.Hide(); txAltura2D.Hide(); lbAlturaD.Hide(); lbB2.Hide(); lbA2.Hide();
                        txAltura.Text = "0"; txBase2D.Text = "0"; txAltura2D.Text = "0";
                        lbBaseD.Text = "Area";
                        areParedTotal = Convert.ToDouble(txBase.Text); ;
                        break;
                }
            }
        }

        //BOTON LIMPIAR
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            chCompleta.Checked = false;
            cbTipoAreaD.Text = "Cuadrado";
            txBase.Text = "0";
            txAltura.Text = "0";
            txBase2D.Text = "0";
            txAltura2D.Text = "0";
            txPuertasD.Text = "0";
            txVentanasD.Text = "0";
            txVigasColumnasD.Text = "0";
            txCantidadBlockR.Text = "0";
            txCantidadCementoR.Text = "0";
            txCantidadArenaR.Text = "0";
            txCantidadAguaR.Text = "0";
            txAreaTotalR.Text = "0";

            dgMateriales.Rows.Clear();
            dgCosto.Rows.Clear();
            lbCostoR.Text = "0";
        }



        //BOTON CALCULAR
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            cargaDatos();
            cargaDatosBlock();
            cargaDatosTipoMortero();
            //ACTUALIZA LOS CAMBIOS DEL COMBOBOX TIPO DE AREA
            cbTipoAreaD_SelectedIndexChanged(sender, e);

            //AREA DE UN BLOCK
            areaBlock = (alturaBlock + junta) * (largoBlock + junta);

            //CANTIDAD DE BLOCK POR m2
            blockPorMetro = 1 / areaBlock;
            blockPorMetroMasDesperdicio = Math.Ceiling(((desperdicio / 100) * blockPorMetro) + blockPorMetro);

            //AREA DE PARED NETA
            areaParedMenosHuecos = areParedTotal - (Convert.ToDouble(txPuertasD.Text) + Convert.ToDouble(txVentanasD.Text) + Convert.ToDouble(txVigasColumnasD.Text));

            //CANTIDAD DE BLOCK
            cantidadBlockTotal = areaParedMenosHuecos / areaBlock;
            cantidadBlockTotalMasDesperdicio = Math.Ceiling(((desperdicio / 100) * cantidadBlockTotal) + cantidadBlockTotal);

            //VOLUMEN DE MORTERO DE JUNTAS
            volumenMortero = ((areaParedMenosHuecos * anchoBlock) - (largoBlock * alturaBlock * anchoBlock * cantidadBlockTotal));

            //CANTIDAD DE CEMMENTO
            cementoPorMetro = cementokg * volumenMortero;
            cementoPorMetroMasDesperdicio = Math.Ceiling((((desperdicio / 100) * cementoPorMetro) + cementoPorMetro) / fundaCemento);

            //CANTIDAD DE ARENA
            arenaPorMetro = volumenMortero * arenaM3;            
            //arenaPorMetroMasDesperdicio = ((Convert.ToDouble(txDesperdicioD.Text) / 100) * arenaPorMetro) + arenaPorMetro;
            arenaPorMetroMasDesperdicio = ((desperdicio / 100) * arenaPorMetro) + arenaPorMetro;

            //CANTIDAD DE AGUA
            aguaPorMetro = volumenMortero * aguaL;
            aguaPorMetroMasDesperdicio = Math.Round(((desperdicio / 100) * aguaPorMetro) + aguaPorMetro);

            //COSTO DE BLOCKS
            precioBlock = precioBlock * cantidadBlockTotalMasDesperdicio;

            //COSTO CEMENTO
            precioCemento = precioCemento * cementoPorMetroMasDesperdicio;

            //COSTO ARENA
            precioArena = precioArena * arenaPorMetroMasDesperdicio;


            //RESULTADOS MATERIALES
            txCantidadBlockR.Text = cantidadBlockTotalMasDesperdicio.ToString();
            txCantidadCementoR.Text = cementoPorMetroMasDesperdicio.ToString();
            txCantidadArenaR.Text = arenaPorMetroMasDesperdicio.ToString("F3");
            txCantidadAguaR.Text = aguaPorMetroMasDesperdicio.ToString();
            txAreaTotalR.Text = areaParedMenosHuecos.ToString();

            //RESULTADOS COSTOS
            totalCostos = precioBlock + precioCemento + precioArena;

            //TABLA DE MATERIALES
            dgMateriales.Rows.Clear();
            string Tblock = anchoBlock.ToString("f2") + "X" + alturaBlock.ToString("f2") + "X" + largoBlock.ToString("f2");
            dgMateriales.Rows.Add("Block", cantidadBlockTotalMasDesperdicio, Tblock);
            dgMateriales.Rows.Add("Cemento", cementoPorMetroMasDesperdicio, "Funda 42.5kg");
            dgMateriales.Rows.Add("Arena", arenaPorMetroMasDesperdicio.ToString("f3"), "m3");
            dgMateriales.Rows.Add("Agua", aguaPorMetroMasDesperdicio, "Litros");

            //TABLA DE COSTOS
            dgCosto.Rows.Clear();
            dgCosto.Rows.Add("Costo Block", precioBlock.ToString("C"));
            dgCosto.Rows.Add("Costo Cemento", precioCemento.ToString("C"));
            dgCosto.Rows.Add("Costo Arena", precioArena.ToString("C"));
            lbCostoR.Text = totalCostos.ToString("C");

        }
    }
}
