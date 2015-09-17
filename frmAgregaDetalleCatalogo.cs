using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EncuestaEmprende
{
    public partial class frmAgregaDetalleCatalogo : Form
    {
        List<catalogo> catalogos_ = new List<catalogo>();
        DataEmprendeEntities2 conexion;
        detalle_catalogo detalleCatalogoModificar;
        public MiDetalleCatalogo miDetalleCatalogoModificar;
        public MiDetalleCatalogo detalleCatalogoNuevo;

        public frmAgregaDetalleCatalogo(DataEmprendeEntities2 conexion,int numeral)
        {
            InitializeComponent();
            this.conexion = conexion;
            //catalogos_.Add(catalogo);
            //labCatalogo.Text = catalogo.catalogo1;
            labCatalogo.Visible = false;
            labelCatalogo.Visible = false;
            txBoxPeso.Text = "0";
            txBoxNumeral.Text = numeral.ToString();
            labelNumeral.Visible = false;
            txBoxNumeral.Visible = false;
        }

        
        
        
        public frmAgregaDetalleCatalogo(DataEmprendeEntities2 conexion,detalle_catalogo detalleCat)
        {
            InitializeComponent();
            this.conexion = conexion;
            this.detalleCatalogoModificar = detalleCat;//editar
            this.Text = "Editar Detalle de Catálogo";
            textBox1.Text = this.detalleCatalogoModificar.nombre;
            txBoxPeso.Text = this.detalleCatalogoModificar.peso!=null?this.detalleCatalogoModificar.peso.ToString():"";
            txBoxNumeral.Text = this.detalleCatalogoModificar.numeral.ToString();
            labCatalogo.Text = detalleCatalogoModificar.catalogo.catalogo1;

            labelNumeral.Visible = false;
            txBoxNumeral.Visible = false;
        }

        public frmAgregaDetalleCatalogo(DataEmprendeEntities2 conexion, MiDetalleCatalogo detalleCat)
        {
            InitializeComponent();
            this.conexion = conexion;
            this.miDetalleCatalogoModificar= detalleCat;//editar
            this.Text = "Editar Detalle de Catálogo";
            textBox1.Text = this.miDetalleCatalogoModificar.Nombre;
            txBoxPeso.Text = this.miDetalleCatalogoModificar.Peso != null ? this.miDetalleCatalogoModificar.Peso.ToString() : "";
            txBoxNumeral.Text = this.miDetalleCatalogoModificar.Numeral.ToString();
            //labCatalogo.Text = miDetalleCatalogoModificar.catalogo.catalogo1;
            labCatalogo.Visible = false;
            labelCatalogo.Visible = false;

            labelNumeral.Visible = false;
            txBoxNumeral.Visible = false;
        }

        
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            int peso;
            int numeral;
            try
            {                
                try
                {
                    peso = int.Parse(txBoxPeso.Text);
                    numeral = int.Parse(txBoxNumeral.Text);
                    if (peso < 0)
                    {
                        MessageBox.Show("El valor del peso tiene que ser positivo.");
                        return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Los valores de peso y el numeral tienen que ser numéricos.");
                    return;
                }
                if (textBox1.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("El valor de detalle del catálogo no puede ser vacio");
                    return;
                }

                if (this.detalleCatalogoModificar!=null)
                {                    
                    detalleCatalogoModificar.nombre = textBox1.Text;                    
                    //detalleCatalogoModificar.peso = int.Parse(txBoxPeso.Text);
                    //detalleCatalogoModificar.numeral = int.Parse(txBoxNumeral.Text);
                    detalleCatalogoModificar.peso = peso;
                    detalleCatalogoModificar.numeral = numeral;
                    detalleCatalogoModificar.catalogo= catalogos_[0];                    
                    conexion.SaveChanges();
                    this.Close();
                }
                else if (miDetalleCatalogoModificar != null)
                {
                    miDetalleCatalogoModificar.Nombre = textBox1.Text;
                    miDetalleCatalogoModificar.Peso = peso;
                    miDetalleCatalogoModificar.Numeral = numeral;
                    this.Close();
                }
                else
                {
                    //detalle_catalogo detalle = new detalle_catalogo();
                    //int max = conexion.detalle_catalogo.Max(it => it.id);
                    //detalle.id = mia.nuevoid(max);
                    //detalle.nombre = textBox1.Text;
                    //detalle.peso = int.Parse(txBoxPeso.Text);
                    //detalle.numeral = int.Parse(txBoxNumeral.Text);
                    //detalle.catalogo= catalogos_[0];
                    //conexion.detalle_catalogo.AddObject(detalle);
                    //conexion.SaveChanges();
                    //detalleCatalogoNuevo = detalle;
                    //this.Close();

                    MiDetalleCatalogo detalle = new MiDetalleCatalogo();
                    //int max = conexion.detalle_catalogo.Max(it => it.id);
                    detalle.ID = -1987;
                    detalle.Nombre = textBox1.Text;
                    //detalle.Peso = int.Parse(txBoxPeso.Text);
                    detalle.Peso = peso;
                    //detalle.Numeral = int.Parse(txBoxNumeral.Text);
                    detalle.Numeral = numeral;
                    detalleCatalogoNuevo = detalle;
                    this.Close();

                }                
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "No se pudo modificar o salvar la entidad");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }



    public class MiDetalleCatalogo
    {
        public int ID;
        public string Nombre;
        public int Peso;
        public int Numeral;
    }
}
