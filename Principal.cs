using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Configuration;
using System.IO;
using DevExpress.XtraBars;
using DevExpress.XtraReports.UI;

namespace EncuestaEmprende
{
    public partial class Principal : Form
    {
        //List<EventArgs> eventos_a_actualizar = new List<EventArgs>(new EventArgs[] { });


        #region VARIABLES GLOBALES
        //string quien;
        int paginacion;
        DataEmprendeEntities2 DataEmprendeconextion;
        enum tipos_entidades { encuesta,encuestas, tematica,tematicas, pregunta, inciso, catalogo, detalle_catalogo, tipo_inciso, aplicacion_encuesta,aplicaciones_encuesta, resultado_encuesta };



        #endregion

        #region CONSTRUCTORES
        public Principal()
        {
            this.DataEmprendeconextion = new DataEmprendeEntities2();
            InitializeComponent();
        }
        #endregion


       

        private bool LeerValores()
        {
            DatosConexion.DatosConexion d = new DatosConexion.DatosConexion();
            List<string> cadenas = d.LeerDatosConexion("conexion.txt");
            if (cadenas.Count() == 4)
            {
                string dataSource = cadenas[0].Split('=')[1];
                string initialCatalog = cadenas[1].Split('=')[1];
                string userId = cadenas[2].Split('=')[1];
                string password = cadenas[3].Split('=')[1];
                //Form_respaldoBD.ModificarCadenaConexion(dataSource, initialCatalog, userId, password);
                Form_respaldoBD.ModificarCadenaConexion(System.Windows.Forms.Application.ExecutablePath,dataSource, initialCatalog, userId, password);
                File.Delete("conexion.txt");
                return true;
            }            
            return false;
        }

        

        
     

        #region FUNCIONES DEL FORMULARIO
        //refresca los tableAdapters que alimentan el formulario
        public void recargar()
        {
            try
            {
                //this.caracterizacion_entidadTableAdapter.Fill(this.dataEmprendeDataSet.caracterizacion_entidad);
                //this.encuestaTableAdapter.Fill(this.dataEmprendeDataSet.encuesta);
                //dbgrid.DataSource = DataEmprendeconextion.encuesta;
                //dbgrid.Update();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "¡Error de Datos!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Muestra un grid DEvExpress teniendo en cuenta un binding  source
        private void mostrar_dbgrid(BindingSource grid, int pag)
        {
            //Habilito y deshabilito botones o pestañas

            //
            recargar();
            //if (dbgrid.DataSource != grid)
            //{
            //    //paginacion = pag;
            //    //gridView1.Columns.Clear();
            //    //dbgrid.DataSource = grid;
            //}
            ///////////SCROLL//////////////

            //this.gridView1.OptionsView.ColumnAutoWidth = false;
            //this.gridView1.ScrollStyle = ScrollStyleFlags.LiveHorzScroll | ScrollStyleFlags.LiveVertScroll;
            //this.gridView1.HorzScrollVisibility = ScrollVisibility.Always;
            //this.gridView1.BestFitColumns();

            //////////////////////////////

            paginacion = pag;

            //Ocultando no mostrables
            try
            {
                //gridView1.Columns["id"].Visible = false;
                //gridView1.Columns["id_centro"].Visible = false;
            }
            catch { }
        }

        //Borrar registro en el grid
        public void borrar_regitro()
        {
            //if (gridView1.RowCount != 0)
            //{
            //    switch (quien)
            //    {
            //        case "Encuestas":
            //            int idEncuesta = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id").ToString());
            //            DialogResult Resp = MessageBox.Show(mia.mensaje_eliminando, mia.mensaje_eliminando_superior, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            //            if (Resp == DialogResult.Yes)
            //            {
            //                try
            //                {
            //                   // this.encuestaTableAdapter.DeleteSegunID(idEncuesta);
            //                    Func<encuesta, bool> selector = enc => enc.id==idEncuesta;
            //                    encuesta todelete = DataEmprendeconextion.encuesta.First(selector);                          //this.DataEmprendeconextion.encuesta.DeleteObject(todelete);
            //                    recargar();
            //                }
            //                catch (Exception ex)
            //                {
            //                    MessageBox.Show("Error al borrar el registro. " + ex.Message, "Error de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                }

            //                //object sender = null;
            //                //DevExpress.XtraBars.ItemClickEventArgs e = null;

            //            }
            //            break;
            //    }
            //}
            //else 
            //{
            //    MessageBox.Show("No se ha seleccionado ningún" +"\n"+" registro. " , "Sin acción", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            //}

        }


        //Edita una encuesta seleccionada
        public void editar_encuestas()
        {
            try
            {
                //if (gridView1.RowCount != 0)
                //{

                //    int idEncuesta = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id"));
                //    frmEditarEncuesta f = new frmEditarEncuesta(idEncuesta);
                //    f.lbEncuesta.Text = this.encuestaTableAdapter.GetDataByID(idEncuesta)[0].encuesta.ToString();
                //    f.ShowDialog();
                //    recargar();

                //}
            }
            catch
            {
                MessageBox.Show("No está seleccionado ningún registro", "Error al editar dato", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        #endregion

        #region EVENTOS
        //Mostrar la lista de encuestas en el DataGrid
        private void bnListarEncuestas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IQueryable<encuesta> encus = DataEmprendeconextion.encuesta;
            ActualizaGridEncuesta(dataGridView1, encus);
        }

        private void ActualizaGridview1(DataGridView dataGridView1)
        {
            dataGridView1.Rows.Clear();


            foreach (var encuesta in DataEmprendeconextion.encuesta)
            {
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "", "");
                fila.Cells[0].Value = encuesta.id;
                fila.Cells[1].Value = encuesta.encuesta1;
                fila.Cells[2].Value = encuesta.descripción;

                DataGridViewComboBoxColumn Tematica = new DataGridViewComboBoxColumn();

                foreach (var tematica in encuesta.tematica)
                {
                    Tematica.Items.Add(tematica.tematica1);
                }
                //this.Tematica.HeaderText = "Temática";
                ////this.Tematica.Name = "Temática";
                //this.Tematica.Resizable = System.Windows.Forms.DataGridViewTriState.True;
                //this.Tematica.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;

                dataGridView1.Columns.RemoveAt(dataGridView1.Columns.Count - 1);
                dataGridView1.Columns.Add(Tematica);
                dataGridView1.Rows.Add(fila);
            }


            //dataGridView1.Columns["Temática"];
        }


        private void ActualizaGridview(DataGridView dataGridView1)
        {
            dataGridView1.Rows.Clear();
            foreach (encuesta x in DataEmprendeconextion.encuesta)
            {

                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "", new ComboBox());
                fila.Cells[0].Value = x.id;
                fila.Cells[1].Value = x.encuesta1;
                fila.Cells[2].Value = x.descripción;
                //foreach (tematica t in DataEmprendeconextion.tematica)
                //{
                //    ((ComboBox)fila.Cells[3].Value).Items.Add(t.tematica1);
                //}
                dataGridView1.Rows.Add(fila);
                dataGridView1.Refresh();
            }
            // dataGridView1.Update();
        }
        //Al cargar el formulario relleno los table adapters
        private void Principal_Load(object sender, EventArgs e)
        {
            try
            {
                mia.id_centro = DataEmprendeconextion.caracterizacion_entidad.First().id_centro;
                mia.camino = DataEmprendeconextion.caracterizacion_entidad.First().camino;
            }
            catch (Exception)
            {
                //SalvarValores();
                if (LeerValores())
                {
                    //MessageBox.Show("El sistema será cerrado, por favor ejecútelo nuevamente, para su correcta ejecución ", "Información de restaura de la  Base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //MessageBox.Show("El sistema será reiniciado, para su correcta ejecución ", "Información de restaura de la  Base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Restart();
                    
                    this.Close();
                }
                else
                {
                    Form_respaldoBD formulario = new Form_respaldoBD();
                    formulario.ShowDialog();
                    //MessageBox.Show("Por favor configure el acceso al servidor, ejecutando InfoEmprende, la aplicación será cerrada, ejecútela nuevamente después de realizar la configuración de acceso.", "Información de configuración de la  Base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //MessageBox.Show("El sistema será cerrado, por favor ejecútelo nuevamente, para su correcta ejecución ", "Información de restaura de la  Base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("El sistema será reiniciado, para su correcta ejecución ", "Información de restaura de la  Base de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Restart();
                    this.Close();
                }
                return;
            }
            Form_respaldoBD.ConfigEncryption(System.Windows.Forms.Application.ExecutablePath);
            if (mia.cerrar)
            {
                Form_logueo form_logueo1 = new Form_logueo(DataEmprendeconextion);
                form_logueo1.ShowDialog();
                if (mia.cerrar) { Close(); }
                else
                {
                    //para sacar nombre de la tabla caracterizacion de la entidad
                    //this.caracterizacion_entidadTableAdapter.Fill(dS_DataEmprende.caracterizacion_entidad);
                    //DS_DataEmprende.caracterizacion_entidadDataTable dsentidad = this.caracterizacion_entidadTableAdapter.GetData();
                    caracterizacion_entidad dsentidad = DataEmprendeconextion.caracterizacion_entidad.First();
                    //Text = "Bievenido: " + mia.nombre + " a " + Text + dsentidad[0]["nombre"].ToString() + ".  Nivel de Acceso: " + mia.nivel_acceso_texto;
                    Text = "Bienvenido: " + mia.nombre + " a " + Text +" "+ dsentidad.nombre + ".  Nivel de Acceso: " + mia.nivel_acceso_texto;
                    //mia.centro = dsentidad[0]["nombre"].ToString();
                    mia.centro = dsentidad.nombre;

                }
            }

            Utiles.LLenarTreeView(DataEmprendeconextion, treeView1);
            //LLenarDatosReporteGeneral();

        }

        
      

        #endregion EVENTOS





        private void ActualizaGridAplicaciones(DataGridView dataGridView1, IList<aplicacion_encuesta> aplicaciones)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("ID_", "ID");
            dataGridView1.Columns[i++].Visible = false;

            dataGridView1.Columns.Add("Descripcion_", "Descripción");
            dataGridView1.Columns[i].Visible = true;
            //dataGridView1.Columns[i++].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns.Add("Fecha_", "Fecha");
            dataGridView1.Columns[i++].Visible = true;

            dataGridView1.Columns.Add("Centro_", "Centro");
            dataGridView1.Columns[i++].Visible = true;

            dataGridView1.Columns.Add("Curso_", "Curso");
            dataGridView1.Columns[i++].Visible = true;

            dataGridView1.Columns.Add("Encuesta_", "Encuesta");
            dataGridView1.Columns[i++].Visible = true;

            dataGridView1.Columns.Add("Sujeto_", "Sujetos");
            dataGridView1.Columns[i++].Visible = true;


            foreach (var aplicacion in aplicaciones)
            {
                int j = 0;
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "", "", "", "");
                fila.Cells[j++].Value = aplicacion.id;
                fila.Cells[j++].Value = aplicacion.descripcion;
                fila.Cells[j++].Value = aplicacion.fecha.ToShortDateString();
                if (aplicacion.id_centro != null)
                {
                    fila.Cells[j++].Value = DataEmprendeconextion.centro.Where(item => item.id == aplicacion.id_centro).First().nombre;
                }
                else
                    fila.Cells[j++].Value = "";
                if (aplicacion.curso != null)
                {
                    fila.Cells[j++].Value = aplicacion.curso.descripcion;
                }
                else
                    fila.Cells[j++].Value = "";
                fila.Cells[j++].Value = aplicacion.encuesta.encuesta1;
                string sujetos = "";
                //foreach (var resultado in aplicacion.resultados_encuesta.OrderBy(item=>item.id_sujeto).Distinct(item=>item.id_sujeto))
                if (aplicacion.resultados_encuesta.Count > 0)
                {
                    foreach (var resultado in aplicacion.resultados_encuesta.GroupBy(item => item.id_sujeto).Select(item => item.FirstOrDefault()))
                    {
                        if (sujetos == "")
                        {
                            sujetos += resultado.id_sujeto;
                        }
                        else
                        {
                            sujetos += " ," + resultado.id_sujeto;
                        }
                    }
                    fila.Cells[j++].Value = sujetos;
                }


                dataGridView1.Rows.Add(fila);
            }
            dataGridView1.Update();

        }

        private void ActualizaGridAplicaciones(DataGridView dataGridView1, IQueryable<aplicacion_encuesta> aplicaciones)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("ID_", "ID");
            dataGridView1.Columns[i++].Visible = false;

            dataGridView1.Columns.Add("Descripcion_", "Descripción");
            dataGridView1.Columns[i].Visible = true;
            //dataGridView1.Columns[i++].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns.Add("Fecha_", "Fecha");
            dataGridView1.Columns[i++].Visible = true;

            dataGridView1.Columns.Add("Centro_", "Centro");
            dataGridView1.Columns[i++].Visible = true;

            dataGridView1.Columns.Add("Curso_", "Curso");
            dataGridView1.Columns[i++].Visible = true;

            dataGridView1.Columns.Add("Encuesta_", "Encuesta");
            dataGridView1.Columns[i++].Visible = true;

            dataGridView1.Columns.Add("Sujeto_", "Sujetos");
            dataGridView1.Columns[i++].Visible = true;
                        

            foreach (var aplicacion in aplicaciones)
            {
                int j = 0;
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "", "","","");
                fila.Cells[j++].Value = aplicacion.id;
                fila.Cells[j++].Value = aplicacion.descripcion;
                fila.Cells[j++].Value = aplicacion.fecha.ToShortDateString();
                if (aplicacion.id_centro != null)
                {
                    fila.Cells[j++].Value = DataEmprendeconextion.centro.Where(item => item.id == aplicacion.id_centro).First().nombre;
                }
                else
                    fila.Cells[j++].Value = "";
                if (aplicacion.curso != null)
                {
                    fila.Cells[j++].Value = aplicacion.curso.descripcion;
                }
                else
                    fila.Cells[j++].Value = "";
                fila.Cells[j++].Value = aplicacion.encuesta.encuesta1;
                string sujetos = "";
                //foreach (var resultado in aplicacion.resultados_encuesta.OrderBy(item=>item.id_sujeto).Distinct(item=>item.id_sujeto))
                if (aplicacion.resultados_encuesta.Count > 0)
                {
                    foreach (var resultado in aplicacion.resultados_encuesta.GroupBy(item=>item.id_sujeto).Select(item=>item.FirstOrDefault()))
                    {
                        if (sujetos == "")
                        {
                            sujetos += resultado.id_sujeto;
                        }
                        else
                        {
                            sujetos += " ," + resultado.id_sujeto;
                        }
                    }
                    fila.Cells[j++].Value = sujetos;
                }


                dataGridView1.Rows.Add(fila);
            }
            dataGridView1.Update();

        }

        private void ActualizaGridResultados(DataGridView dataGridView1, IQueryable<resultados_encuesta> resenc)
        {
            dataGridView1.Rows.Clear();


            foreach (var encuesta in resenc)
            {
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "", "");
                fila.Cells[0].Value = encuesta.id_aplicacion_encuesta;
                fila.Cells[1].Value = encuesta.resultado;
                fila.Cells[2].Value = encuesta.resultado_texto;

                dataGridView1.Rows.Add(fila);
            }
            dataGridView1.Update();

        }
        private void ActualizaGridEncuesta(DataGridView dataGridView1, IQueryable<encuesta> encuestas)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("ID_", "ID");
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns.Add("Encuesta_", "Encuesta");
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns.Add("Descripcion_", "Descripción");
            dataGridView1.Columns[2].Visible = true;


            foreach (var encuesta in encuestas)
            {
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "");
                fila.Cells[0].Value = encuesta.id;
                fila.Cells[1].Value = encuesta.encuesta1;
                fila.Cells[2].Value = encuesta.descripción;

                dataGridView1.Rows.Add(fila);
            }
            dataGridView1.Update();

        }
        private void ActualizaGridTematicas(DataGridView dataGridView1, IQueryable<tematica> tematicas)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("ID_", "ID");
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns.Add("Tematica_", "Temática");
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns.Add("Encuesta_", "Encuesta");
            dataGridView1.Columns[2].Visible = true;


            foreach (var encuesta in tematicas)
            {
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "");
                fila.Cells[0].Value = encuesta.id;
                fila.Cells[1].Value = encuesta.tematica1;
                fila.Cells[2].Value = encuesta.encuesta.encuesta1;

                dataGridView1.Rows.Add(fila);
            }
            dataGridView1.Update();

        }
        private void ActualizaGridPreguntas(DataGridView dataGridView1, IQueryable<preguntas> preguntas)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("ID_", "ID");
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns.Add("Pregunta_", "Pregunta");
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns.Add("Tematica_", "Temática");
            dataGridView1.Columns[2].Visible = true;


            foreach (var encuesta in preguntas)
            {
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "");
                fila.Cells[0].Value = encuesta.id_pregunta;
                fila.Cells[1].Value = encuesta.pregunta;
                fila.Cells[2].Value = encuesta.tematica.tematica1;

                dataGridView1.Rows.Add(fila);
            }
            dataGridView1.Update();

        }
        private void ActualizaGridIncisos(DataGridView dataGridView1, IQueryable<incisos> incisos)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("ID_", "ID");
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns.Add("Inciso_", "Inciso");
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns.Add("Tipo_", "Tipo");
            dataGridView1.Columns[2].Visible = true;

            dataGridView1.Columns.Add("Pregunta_", "Pregunta");
            dataGridView1.Columns[3].Visible = true;

            dataGridView1.Columns.Add("Catalogo_", "Catálogo");
            dataGridView1.Columns[4].Visible = true;

            dataGridView1.Columns.Add("Numeral_", "Numeral");
            dataGridView1.Columns[5].Visible = true;

            foreach (var encuesta in incisos)
            {
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "", "", "", "");
                fila.Cells[0].Value = encuesta.id_inciso;
                fila.Cells[1].Value = encuesta.inciso.Trim();
                fila.Cells[2].Value = encuesta.tipo_inciso.tipo_inciso1;
                fila.Cells[3].Value = encuesta.preguntas.pregunta;
                fila.Cells[4].Value = encuesta.catalogo.catalogo1;
                fila.Cells[5].Value = encuesta.numeral;

                dataGridView1.Rows.Add(fila);
            }
            dataGridView1.Update();

        }

        private void ActualizaGridDetalleCatalogo(DataGridView dataGridView1, IQueryable<detalle_catalogo> detcatalogos)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("ID_", "ID");
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns.Add("Catalogo_", "Catálogo");
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns.Add("Nombre_", "Nombre");
            dataGridView1.Columns[2].Visible = true;

            dataGridView1.Columns.Add("Peso_", "Peso");
            dataGridView1.Columns[3].Visible = true;

            dataGridView1.Columns.Add("Numeral_", "Numeral");
            dataGridView1.Columns[4].Visible = true;

            foreach (var encuesta in detcatalogos)
            {
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "");
                fila.Cells[0].Value = encuesta.id;
                fila.Cells[1].Value = encuesta.catalogo.catalogo1;
                fila.Cells[2].Value = encuesta.nombre;
                fila.Cells[3].Value = encuesta.peso;
                fila.Cells[4].Value = encuesta.numeral;

                dataGridView1.Rows.Add(fila);
            }
            dataGridView1.Update();
        }

        private void ActualizaGridCatalogos(DataGridView dataGridView1, catalogo catalogo)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("ID_", "ID");
            dataGridView1.Columns[i++].Visible = false;

            //dataGridView1.Columns.Add("Catalogo_", "Catálogo");
            //dataGridView1.Columns[1].Visible = true;
            //dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns.Add("Nombre_", "Nombre");
            dataGridView1.Columns[i++].Visible = true;

            dataGridView1.Columns.Add("Peso_", "Peso");
            dataGridView1.Columns[i++].Visible = true;

            //dataGridView1.Columns.Add("Numeral_", "Numeral");
            //dataGridView1.Columns[i++].Visible = true;

            foreach (var detalle in catalogo.detalle_catalogo.OrderBy(item => item.numeral))
            {
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dataGridView1, "", "", "");
                fila.Cells[0].Value = detalle.id;                
                fila.Cells[1].Value = detalle.nombre;
                fila.Cells[2].Value = detalle.peso;
                //fila.Cells[4].Value = encuesta.numeral;

                dataGridView1.Rows.Add(fila);
            }
            dataGridView1.Update();

        }


        //private void ActualizaGridCatalogos(DataGridView dataGridView1, catalogo catalogo)
        //{
        //    dataGridView1.Rows.Clear();
        //    //dataGridView1.Columns.Clear();

        //    foreach (var detalle in catalogo.detalle_catalogo.OrderBy(item => item.numeral))
        //    {
        //        DataGridViewRow fila = new DataGridViewRow();
        //        fila.CreateCells(dataGridView1, "", "");
        //        fila.Cells[0].Value = detalle.nombre;
        //        if (detalle.peso != null)
        //        {
        //            fila.Cells[1].Value = detalle.peso;
        //        }
        //        dataGridView1.Rows.Add(fila);
        //    }
        //    dataGridView1.Update();

        //}


        //private void ActualizaBotones(tipos_entidades tipos_enti)
        //{
        //    switch (tipos_enti)
        //    {

        //        case tipos_entidades.encuestas:
        //            ribbonPageGroupEncuesta.Text = "Encuestas";
        //            bnEncuestaNuevaEncuesta.Caption = "Nueva Encuesta";
        //            bnEncuestaEliminaEncuesta.Caption = "Eliminar Encuesta";
        //            bnEditarEcuesta.Caption = "Editar Encuesta"; 

        //            bnEncuestaNuevaEncuesta.ItemClick += bnNuevaEncuesta_ItemClick;
        //            bnEditarEcuesta.ItemClick += bnEditarEcuesta_ItemClick;
        //            bnEncuestaEliminaEncuesta.ItemClick += bnEliminarEncuesta_ItemClick;
                   
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
                    
        //            break;
        //        case tipos_entidades.encuesta:
        //            ribbonPageGroupEncuesta.Text = "Temática";
        //            bnEncuestaNuevaEncuesta.Caption = "Nueva Temática";
        //            bnEncuestaEliminaEncuesta.Caption = "Eliminar Encuesta";
        //            bnEditarEcuesta.Caption = "Editar Encuesta Seleccionada";

        //            bnEncuestaNuevaEncuesta.ItemClick += barButtonItem13_ItemClick;
        //            bnEncuestaEliminaEncuesta.ItemClick += barButtonItem14_ItemClick;
        //            bnEditarEcuesta.ItemClick += barButtonItem12_ItemClick;
                    
                   
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = true;
                   
        //            break;
        //        case tipos_entidades.tematicas://me qued'e
        //            ribbonPageGroupEncuesta.Text = "Temáticas";
        //            bnEncuestaNuevaEncuesta.Caption = "Nueva Temática";
        //            bnEncuestaEliminaEncuesta.Caption = "Eliminar Temática";
        //            bnEditarEcuesta.Caption = "Editar Seleccionada"; 

        //            bnEncuestaNuevaEncuesta.ItemClick += bnNuevaEncuesta_ItemClick;
        //            bnEditarEcuesta.ItemClick += bnEditarEcuesta_ItemClick;
        //            bnEncuestaEliminaEncuesta.ItemClick += bnEliminarEncuesta_ItemClick;
                   
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
                    
                   
        //            break;
        //        case tipos_entidades.tematica:
        //            ribbonPageGroupEncuesta.Text = "Temáticas";
        //            bnEncuestaNuevaEncuesta.Caption = "Nueva Pregunta";
        //            bnEncuestaEliminaEncuesta.Caption = "Eliminar Temática";
        //            bnEditarEcuesta.Caption = "Editar Temática Seleccionada"; 

        //            bnEncuestaNuevaEncuesta.ItemClick += bnNuevaEncuesta_ItemClick;
        //            bnEditarEcuesta.ItemClick += bnEditarEcuesta_ItemClick;
        //            bnEncuestaEliminaEncuesta.ItemClick += bnEliminarEncuesta_ItemClick;
                   
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
                    
                   
        //            break;
        //        case tipos_entidades.pregunta:
        //            ribbonPageGroupEncuesta.Text = "Preguntas";
        //            bnEncuestaNuevaEncuesta.Caption = "Nuevo Inciso";
        //            bnEncuestaEliminaEncuesta.Caption = "Eliminar Pregunta";
        //            bnEditarEcuesta.Caption = "Editar Pregunta Seleccionada"; 
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
                    

        //            bnEncuestaNuevaEncuesta.ItemClick += bnNuevaEncuesta_ItemClick;
        //            bnEditarEcuesta.ItemClick += bnEditarEcuesta_ItemClick;
        //            bnEncuestaEliminaEncuesta.ItemClick += bnEliminarEncuesta_ItemClick;
                   
        //            break;
        //        case tipos_entidades.inciso:
        //            ribbonPageGroupEncuesta.Text = "Incisos";
        //            bnEncuestaNuevaEncuesta.Caption = "Nuevo Inciso";
        //            bnEncuestaEliminaEncuesta.Caption = "Eliminar Inciso";
        //            bnEditarEcuesta.Caption = "Editar Inciso"; 
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
                    

        //            bnEncuestaNuevaEncuesta.ItemClick += bnNuevaEncuesta_ItemClick;
        //            bnEditarEcuesta.ItemClick += bnEditarEcuesta_ItemClick;
        //            bnEncuestaEliminaEncuesta.ItemClick += bnEliminarEncuesta_ItemClick;
                   
        //            break;
        //        case tipos_entidades.catalogo:
        //           ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
        //            break;
        //        case tipos_entidades.detalle_catalogo:
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
        //            break;
        //        case tipos_entidades.tipo_inciso:
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
        //            break;
        //        case tipos_entidades.aplicaciones_encuesta:
        //            ribbonPageGroupEncuesta.Visible = false;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = true;
        //            break;
        //        case tipos_entidades.aplicacion_encuesta:
        //            ribbonPageGroupEncuesta.Visible = false;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = true;
        //            break;
        //        case tipos_entidades.resultado_encuesta:
        //            ribbonPageGroupEncuesta.Visible = false;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = true;
        //            break;
        //        default:
        //            break;
        //    }

        //}



        private void ActualizaBotones(tipos_entidades tipos_enti)
        {
            ribbonPageGroupEncuesta.Visible = false;
            ribbonPageGroupAplicacionEncuesta.Visible = false;
            ribbonPageGroupTematica.Visible = false;
            ribbonPageGroupPregunta.Visible = false;
            ribbonPageGroupIncisos.Visible = false;
            //ribbonPageGroupCatalogo.Visible = false;

            bnOpcionesCatalogos.Visibility = EncuestaAcceso.Opciones_Catalogos ? BarItemVisibility.Always : BarItemVisibility.Never;
            switch (tipos_enti)
            {

                case tipos_entidades.encuestas:
                    //ribbonPageGroupEncuesta.Visible = true;
                    ribbonPageGroupEncuesta.Visible = EncuestaAcceso.Encuestas;

                    bnEncuestaAplicarEncuesta.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    //bnEncuestaEditarEncuesta.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnEncuestaEditarEncuesta.Visibility = EncuestaAcceso.Encuesta_EditarEncuesta? BarItemVisibility.Always : BarItemVisibility.Never;
                    
                    //bnEncuestaEliminaEncuesta.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnEncuestaEliminaEncuesta.Visibility = EncuestaAcceso.Encuesta_EliminarEncuesta ? BarItemVisibility.Always : BarItemVisibility.Never;

                    //bnEncuestaNuevaEncuesta.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnEncuestaNuevaEncuesta.Visibility = EncuestaAcceso.Encuesta_NuevaEncuesta ? BarItemVisibility.Always : BarItemVisibility.Never;

                    bnEncuestaNuevaTematica.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    bnEncuestaInforme.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnEncuestaImprimir.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    break;
                case tipos_entidades.encuesta:
                    //ribbonPageGroupEncuesta.Visible = true;
                    ribbonPageGroupEncuesta.Visible = EncuestaAcceso.Encuestas;

                    //bnEncuestaAplicarEncuesta.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnEncuestaAplicarEncuesta.Visibility = EncuestaAcceso.Encuesta_AplicarEncuesta ? BarItemVisibility.Always : BarItemVisibility.Never;

                    //bnEncuestaEditarEncuesta.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnEncuestaEditarEncuesta.Visibility = EncuestaAcceso.Encuesta_EditarEncuesta ? BarItemVisibility.Always : BarItemVisibility.Never;

                    //bnEncuestaEliminaEncuesta.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnEncuestaEliminaEncuesta.Visibility = EncuestaAcceso.Encuesta_EliminarEncuesta ? BarItemVisibility.Always : BarItemVisibility.Never;

                    bnEncuestaNuevaEncuesta.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                    //bnEncuestaNuevaTematica.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnEncuestaNuevaTematica.Visibility = EncuestaAcceso.Encuesta_NuevaTematica ? BarItemVisibility.Always : BarItemVisibility.Never;

                    bnEncuestaInforme.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnEncuestaImprimir.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    break;
                case tipos_entidades.tematicas:
                    //ribbonPageGroupTematica.Visible = true;
                    ribbonPageGroupTematica.Visible = EncuestaAcceso.Tematicas;

                    bnTematicaEditarTematica.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnTematicaEliminarTematica.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnTematicaNuevaTematica.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnTematicaNuevaPregunta.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    break;
                case tipos_entidades.tematica:
                    //ribbonPageGroupTematica.Visible = true;
                    ribbonPageGroupTematica.Visible = EncuestaAcceso.Tematicas;

                    bnTematicaEditarTematica.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnTematicaEliminarTematica.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnTematicaNuevaTematica.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    bnTematicaNuevaPregunta.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    break;
                case tipos_entidades.pregunta:
                    //ribbonPageGroupPregunta.Visible = true;
                    ribbonPageGroupPregunta.Visible = EncuestaAcceso.Preguntas;

                    bnPreguntaNuevoInciso.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnPreguntaEditarPreguntar.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnPreguntaEliminarPregunta.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    break;
                case tipos_entidades.inciso:
                    //ribbonPageGroupIncisos.Visible = true;
                    ribbonPageGroupIncisos.Visible = EncuestaAcceso.Incisos;

                    bnIncisoEditarInciso.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnIncisoEliminarInciso.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    break;
                case tipos_entidades.catalogo:
                   
                    break;
                case tipos_entidades.detalle_catalogo:                   
                    break;
                case tipos_entidades.tipo_inciso:                  
                    break;
                case tipos_entidades.aplicaciones_encuesta:  
                    //ribbonPageGroupAplicacionEncuesta.Visible = true;
                    ribbonPageGroupAplicacionEncuesta.Visible = EncuestaAcceso.Aplicaciones;

                    //bnAplicacionEditarAplicacion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnAplicacionEditarAplicacion.Visibility = EncuestaAcceso.Aplicacion_EditarAplicacion ? BarItemVisibility.Always : BarItemVisibility.Never;

                    //bnAplicacionEliminarAplicacion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnAplicacionEliminarAplicacion.Visibility = EncuestaAcceso.Aplicacion_EliminarAplicacion ? BarItemVisibility.Always : BarItemVisibility.Never;

                    //bnAplicacionNuevaAplicacion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnAplicacionNuevaAplicacion.Visibility = EncuestaAcceso.Aplicacion_NuevaAplicacion ? BarItemVisibility.Always : BarItemVisibility.Never;

                    //bnAplicacionVolverAplicar.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnAplicacionContinuarAplicar.Visibility = EncuestaAcceso.Aplicacion_ContinuarAplicacion ? BarItemVisibility.Always : BarItemVisibility.Never;

                    bnAplicacionInforme.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    
                    //bnAplicacionRevisar.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnAplicacionRevisar.Visibility = EncuestaAcceso.Aplicacion_RevisarAplicacion ? BarItemVisibility.Always : BarItemVisibility.Never;
                    break;
                case tipos_entidades.aplicacion_encuesta:
                    //ribbonPageGroupAplicacionEncuesta.Visible = true;
                    ribbonPageGroupAplicacionEncuesta.Visible = EncuestaAcceso.Aplicaciones;

                    //bnAplicacionEditarAplicacion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnAplicacionEditarAplicacion.Visibility = EncuestaAcceso.Aplicacion_EditarAplicacion ? BarItemVisibility.Always : BarItemVisibility.Never;

                    //bnAplicacionEliminarAplicacion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnAplicacionEliminarAplicacion.Visibility = EncuestaAcceso.Aplicacion_EliminarAplicacion ? BarItemVisibility.Always : BarItemVisibility.Never;

                    bnAplicacionNuevaAplicacion.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    //bnAplicacionNuevaAplicacion.Visibility = EncuestaAcceso.Aplicacion_NuevaAplicacion ? BarItemVisibility.Always : BarItemVisibility.Never;

                    //bnAplicacionContinuarAplicar.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnAplicacionContinuarAplicar.Visibility = EncuestaAcceso.Aplicacion_ContinuarAplicacion ? BarItemVisibility.Always : BarItemVisibility.Never;

                    bnAplicacionInforme.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    
                    //bnAplicacionRevisar.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bnAplicacionRevisar.Visibility = EncuestaAcceso.Aplicacion_RevisarAplicacion ? BarItemVisibility.Always : BarItemVisibility.Never;
                    break;
                case tipos_entidades.resultado_encuesta:
                    break;
                default:
                    break;
            }


        }



        //private void DesactualizaBotones(tipos_entidades tipos_enti)
        //{
        //    switch (tipos_enti)
        //    {

        //        case tipos_entidades.encuestas:
                    
        //            //bnNuevaEncuesta.ItemClick -= bnNuevaEncuesta_ItemClick;
        //            //bnEditarEcuesta.ItemClick -= bnEditarEcuesta_ItemClick;
        //            //bnEliminarEncuesta.ItemClick -= bnEliminarEncuesta_ItemClick;

        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
        //            riboPreguntas.Visible = false;

        //            break;
        //        case tipos_entidades.encuesta:
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = true;
        //            riboPreguntas.Visible = false;

        //            break;
        //        case tipos_entidades.tematicas://me qued'e
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
        //            riboPreguntas.Visible = false;
        //            break;
        //        case tipos_entidades.tematica:
        //            ribbonPageGroupEncuesta.Visible = true;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
        //            riboPreguntas.Visible = false;
        //            break;
        //        case tipos_entidades.pregunta:
        //            ribbonPageGroupEncuesta.Visible = false;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
        //            ribbonPageGroupIncisos.Visible = false;
        //            riboPreguntas.Visible = true;
        //            break;
        //        case tipos_entidades.inciso:
        //            ribbonPageGroupEncuesta.Visible = false;
        //            ribbonPageGroupCatalogo.Visible = false;
        //            ribbonPageGroupTematica.Visible = false;
        //            ribbonPageAplicacion.Visible = false;
        //            ribbonPageGroupPregunta.Enabled = false;
        //            riboPreguntas.Visible = true;
        //            break;
        //        case tipos_entidades.catalogo:
                   
        //            break;
        //        case tipos_entidades.detalle_catalogo:
                    
        //            break;
        //        case tipos_entidades.tipo_inciso:
                    
        //            break;
        //        case tipos_entidades.aplicaciones_encuesta:
                    
        //            break;
        //        case tipos_entidades.aplicacion_encuesta:
                    
        //            break;
        //        case tipos_entidades.resultado_encuesta:
                    
        //            break;
        //        default:
        //            break;
        //    }

        //}

      
        

        #region Agregar
        TreeNode nodoSeleccionado;
        Font fontNodoSeleccionado;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
            TreeNode nuevonodo = e.Node;
            if (nodoSeleccionado != null)
            {
                nodoSeleccionado.NodeFont = fontNodoSeleccionado;
                fontNodoSeleccionado = nuevonodo.NodeFont;
                nuevonodo.NodeFont = new Font("Arial", 8, FontStyle.Underline | FontStyle.Italic);
                nodoSeleccionado = nuevonodo;
            }
            else
            {
                fontNodoSeleccionado = nuevonodo.NodeFont;
                nuevonodo.NodeFont = new Font("Arial", 8, FontStyle.Underline | FontStyle.Italic);
                nodoSeleccionado = nuevonodo;
            }
            treeView1.Invalidate();

            //LlenarDataGridView(e.Node.Name, e);
            LlenarDataGridView(nodoSeleccionado);
        }


        private void LlenarDataGridView(TreeNode nodeSeleccionado)
        {
            switch (nodoSeleccionado.Name)
            {
                case "00":
                    LlenarDataGridViewEntidades(nodoSeleccionado);
                    break;
                default:
                    LlenarDataGridViewEntidad(nodoSeleccionado);
                    break;
            }
        }


        private void LlenarDataGridViewEntidades(TreeNode nodoSeleccionado)
        {
            int id = 0;
            if (nodoSeleccionado.Parent != null)
            {
                string[] valores = nodoSeleccionado.Parent.Name.Split(' ');
                //id = int.Parse(e.Node.Parent.Name);
                id = int.Parse(valores[1]);
            }
            switch (nodoSeleccionado.Text)
            {
                case "Encuestas":
                    //EncuestasTodas
                    var encuestas = DataEmprendeconextion.encuesta;
                    ActualizaGridEncuesta(dataGridView1, encuestas);
                    ActualizaBotones(tipos_entidades.encuestas);
                    break;
                case "Temáticas":
                    //tematicas de la encuesta con este id
                    var tematicas = DataEmprendeconextion.tematica.Where(item => item.id_encuesta == id);
                    ActualizaGridTematicas(dataGridView1, tematicas);
                    ActualizaBotones(tipos_entidades.tematicas);
                    break;
                //case "Preguntas":
                //    //preguntas de la tematica con este id
                //    var preguntas = DataEmprendeconextion.preguntas.Where(item => item.id_tematica == id);
                //    ActualizaGridPreguntas(dataGridView1, preguntas);
                //    ActualizaBotones(tipos_entidades.preguntas);
                //    break;
                //case "Incisos":
                //    //tematicas de las encuesta con este id
                //    var incisos = DataEmprendeconextion.incisos.Where(item => item.id_pregunta == id);
                //    ActualizaGridIncisos(dataGridView1, incisos);
                //    ActualizaBotones(tipos_entidades.incisos);
                //    break;
                //case "Detalles de Catálogo":
                //case "Detalles Catálogo":
                //    //detalles de catalogo del inciso con este id
                //    incisos inciso = DataEmprendeconextion.incisos.Where(item => item.id_inciso == id).First();
                //    var detalles = DataEmprendeconextion.detalle_catalogo.Where(item => item.id_catalogo == inciso.id_catalogo);
                //    ActualizaGridDetalleCatalogo(dataGridView1, detalles);
                //    ActualizaBotones(tipos_entidades.detalle_catalogo);
                //    break;
                case "Aplicaciones de Encuesta":
                    //aplicaciones de encuesta de la encuesta con este id
                    var aplicaciones = DataEmprendeconextion.aplicacion_encuesta.Where(item => item.id_encuesta == id).ToList();
                    if (EncuestaAcceso.Profesor)
                    {                        
                        for (int i = aplicaciones.Count-1; i >= 0; i--)
                        {
                            if (aplicaciones[i].permisos_aplicaciones.Where(item => item.id_profesor == mia.id_profesor).FirstOrDefault() == null)
                            {
                                aplicaciones.RemoveAt(i);
                            }
                        }
                    }
                    ActualizaGridAplicaciones(dataGridView1, aplicaciones);
                    ActualizaBotones(tipos_entidades.aplicaciones_encuesta);
                    break;
                //case "Resultados de Encuesta":
                //    //resultados de encuesta de la aplicacion con este id
                //    var resultados = DataEmprendeconextion.resultados_encuesta.Where(item => item.id_aplicacion_encuesta == id);
                //    ActualizaGridResultados(dataGridView1, resultados);
                //    ActualizaBotones(tipos_entidades.resultados_encuesta);
                //    break;
                default:
                    break;
            }
        }

        private void LlenarDataGridViewEntidad(TreeNode nodoSeleccionado)
        {
            string[] valores = nodoSeleccionado.Name.Split(new string[] { " " }, StringSplitOptions.None);
            int id = 0;
            //switch (e.Node.Parent.Text)
            switch (valores[0])
            {
                case "Encuesta":
                    //case "Encuestas":
                    //Encuesta(llave)
                    id = int.Parse(valores[1]);
                    var encuestas = DataEmprendeconextion.encuesta.Where(item => item.id == id);
                    ActualizaGridEncuesta(dataGridView1, encuestas);
                    //ActualizaBotones(tipos_entidades.aplicacion_encuesta, tipos_entidades.tematica);
                    ActualizaBotones(tipos_entidades.encuesta);
                    break;
                case "Tematica":
                case "Temática":
                    //case "Temáticas":
                    //case "Tematicas":
                    //Tematica(llave)
                    id = int.Parse(valores[1]);
                    var tematicas = DataEmprendeconextion.tematica.Where(item => item.id == id);
                    ActualizaGridTematicas(dataGridView1, tematicas);
                    //ActualizaBotones(tipos_entidades.preguntas);
                    ActualizaBotones(tipos_entidades.tematica);
                    break;
                case "Pregunta":
                    //case "Preguntas":
                    //Preguntas(llave)
                    id = int.Parse(valores[1]);
                    var preguntas = DataEmprendeconextion.preguntas.Where(item => item.id_pregunta == id);
                    ActualizaGridPreguntas(dataGridView1, preguntas);
                    //ActualizaBotones(tipos_entidades.incisos);
                    ActualizaBotones(tipos_entidades.pregunta);
                    break;
                case "Inciso":
                    //case "Incisos":
                    //Incisos(llave)
                    id = int.Parse(valores[1]);
                    var incisos = DataEmprendeconextion.incisos.Where(item => item.id_inciso == id);
                    ActualizaGridIncisos(dataGridView1, incisos);
                    //ActualizaBotones(tipos_entidades.catalogo);
                    ActualizaBotones(tipos_entidades.inciso);
                    break;
                //case "DetalleCatalogo":
                //    //case "Detalles de Catálogo":
                //    //case "Detalles Catálogo":
                //    //Detalles de Catalogo(llave)
                //    id = int.Parse(valores[1]);
                //    var detalles = DataEmprendeconextion.detalle_catalogo.Where(item => item.id == id);
                //    ActualizaGridDetalleCatalogo(dataGridView1, detalles);
                //    ActualizaBotones();
                //break;
                case "AplicacionEncuesta":
                    //case "Aplicaciones de Encuesta":
                    //Aplicaciones de Encuesta(llave)
                    id = int.Parse(valores[1]);
                    var aplicaciones = DataEmprendeconextion.aplicacion_encuesta.Where(item => item.id == id);
                    ActualizaGridAplicaciones(dataGridView1, aplicaciones);
                    //ActualizaBotones(tipos_entidades.resultados_encuesta);
                    ActualizaBotones(tipos_entidades.aplicacion_encuesta);
                    break;
                //case "ResultadoEncuesta":
                //    //case "Resultados de Encuesta":
                //    string[] llaves = valores[1].Split('-');
                //    int id_aplicacion = int.Parse(llaves[0]);
                //    int id_inciso = int.Parse(llaves[1]);
                //    int id_sujeto = int.Parse(llaves[2]);
                //    var resultados = DataEmprendeconextion.resultados_encuesta.Where(item => (item.id_aplicacion_encuesta == id_aplicacion &&
                //        item.id_inciso == id_inciso && item.id_sujeto == id_sujeto));
                //    ActualizaGridResultados(dataGridView1, resultados);
                //    ActualizaBotones();
                //    break;
                case "Catalogo":
                    id = int.Parse(valores[1]);
                    var catalogo = DataEmprendeconextion.catalogo.Where(item => item.id_catalogo == id).First();
                    ActualizaGridCatalogos(dataGridView1, catalogo);
                    //ActualizaBotones(tipos_entidades.detalle_catalogo);
                    ActualizaBotones(tipos_entidades.catalogo);
                    break;
                default:
                    break;
            }
        }

        

        private void LlenarDataGridView(string llave, TreeViewEventArgs e)
        {
            switch (llave)
            {
                case "00":
                    LlenarDataGridViewEntidades(e);
                    break;
                default:
                    LlenarDataGridViewEntidad(llave, e);
                    break;
            }
        }


        private void LlenarDataGridViewEntidades(TreeViewEventArgs e)
        {
            int id = 0;
            if (e.Node.Parent != null)
            {
                string[] valores = e.Node.Parent.Name.Split(' ');
                //id = int.Parse(e.Node.Parent.Name);
                id = int.Parse(valores[1]);
            }
            switch (e.Node.Text)
            {
                case "Encuestas":
                    //EncuestasTodas
                    var encuestas = DataEmprendeconextion.encuesta;
                    ActualizaGridEncuesta(dataGridView1, encuestas);
                    ActualizaBotones(tipos_entidades.encuestas);
                    break;
                case "Temáticas":
                    //tematicas de la encuesta con este id
                    var tematicas = DataEmprendeconextion.tematica.Where(item => item.id_encuesta == id);
                    ActualizaGridTematicas(dataGridView1, tematicas);
                    ActualizaBotones(tipos_entidades.tematicas);
                    break;
                //case "Preguntas":
                //    //preguntas de la tematica con este id
                //    var preguntas = DataEmprendeconextion.preguntas.Where(item => item.id_tematica == id);
                //    ActualizaGridPreguntas(dataGridView1, preguntas);
                //    ActualizaBotones(tipos_entidades.preguntas);
                //    break;
                //case "Incisos":
                //    //tematicas de las encuesta con este id
                //    var incisos = DataEmprendeconextion.incisos.Where(item => item.id_pregunta == id);
                //    ActualizaGridIncisos(dataGridView1, incisos);
                //    ActualizaBotones(tipos_entidades.incisos);
                //    break;
                //case "Detalles de Catálogo":
                //case "Detalles Catálogo":
                //    //detalles de catalogo del inciso con este id
                //    incisos inciso = DataEmprendeconextion.incisos.Where(item => item.id_inciso == id).First();
                //    var detalles = DataEmprendeconextion.detalle_catalogo.Where(item => item.id_catalogo == inciso.id_catalogo);
                //    ActualizaGridDetalleCatalogo(dataGridView1, detalles);
                //    ActualizaBotones(tipos_entidades.detalle_catalogo);
                //    break;
                case "Aplicaciones de Encuesta":
                    //aplicaciones de encuesta de la encuesta con este id
                    var aplicaciones = DataEmprendeconextion.aplicacion_encuesta.Where(item => item.id_encuesta == id);
                    ActualizaGridAplicaciones(dataGridView1, aplicaciones);
                    ActualizaBotones(tipos_entidades.aplicaciones_encuesta);
                    break;
                //case "Resultados de Encuesta":
                //    //resultados de encuesta de la aplicacion con este id
                //    var resultados = DataEmprendeconextion.resultados_encuesta.Where(item => item.id_aplicacion_encuesta == id);
                //    ActualizaGridResultados(dataGridView1, resultados);
                //    ActualizaBotones(tipos_entidades.resultados_encuesta);
                //    break;
                default:
                    break;
            }
        }

        private void LlenarDataGridViewEntidad(string llave, TreeViewEventArgs e)
        {
            string[] valores = llave.Split(new string[] { " " }, StringSplitOptions.None);
            int id = 0;
            //switch (e.Node.Parent.Text)
            switch (valores[0])
            {
                case "Encuesta":
                    //case "Encuestas":
                    //Encuesta(llave)
                    id = int.Parse(valores[1]);
                    var encuestas = DataEmprendeconextion.encuesta.Where(item=>item.id==id);
                    ActualizaGridEncuesta(dataGridView1, encuestas);
                    //ActualizaBotones(tipos_entidades.aplicacion_encuesta, tipos_entidades.tematica);
                    ActualizaBotones(tipos_entidades.encuesta);
                    break;
                case "Tematica":
                case "Temática":
                    //case "Temáticas":
                    //case "Tematicas":
                    //Tematica(llave)
                    id = int.Parse(valores[1]);
                    var tematicas = DataEmprendeconextion.tematica.Where(item => item.id == id);
                    ActualizaGridTematicas(dataGridView1, tematicas);
                    //ActualizaBotones(tipos_entidades.preguntas);
                    ActualizaBotones(tipos_entidades.tematica);
                    break;
                case "Pregunta":
                    //case "Preguntas":
                    //Preguntas(llave)
                    id = int.Parse(valores[1]);
                    var preguntas = DataEmprendeconextion.preguntas.Where(item => item.id_pregunta == id);
                    ActualizaGridPreguntas(dataGridView1, preguntas);
                    //ActualizaBotones(tipos_entidades.incisos);
                    ActualizaBotones(tipos_entidades.pregunta);
                    break;
                case "Inciso":
                    //case "Incisos":
                    //Incisos(llave)
                    id = int.Parse(valores[1]);
                    var incisos = DataEmprendeconextion.incisos.Where(item => item.id_inciso == id);
                    ActualizaGridIncisos(dataGridView1, incisos);
                    //ActualizaBotones(tipos_entidades.catalogo);
                    ActualizaBotones(tipos_entidades.inciso);
                    break;
                //case "DetalleCatalogo":
                //    //case "Detalles de Catálogo":
                //    //case "Detalles Catálogo":
                //    //Detalles de Catalogo(llave)
                //    id = int.Parse(valores[1]);
                //    var detalles = DataEmprendeconextion.detalle_catalogo.Where(item => item.id == id);
                //    ActualizaGridDetalleCatalogo(dataGridView1, detalles);
                //    ActualizaBotones();
                    //break;
                case "AplicacionEncuesta":
                    //case "Aplicaciones de Encuesta":
                    //Aplicaciones de Encuesta(llave)
                    id = int.Parse(valores[1]);
                    var aplicaciones = DataEmprendeconextion.aplicacion_encuesta.Where(item => item.id == id);
                    ActualizaGridAplicaciones(dataGridView1, aplicaciones);
                    //ActualizaBotones(tipos_entidades.resultados_encuesta);
                    ActualizaBotones(tipos_entidades.aplicacion_encuesta);
                    break;
                //case "ResultadoEncuesta":
                //    //case "Resultados de Encuesta":
                //    string[] llaves = valores[1].Split('-');
                //    int id_aplicacion = int.Parse(llaves[0]);
                //    int id_inciso = int.Parse(llaves[1]);
                //    int id_sujeto = int.Parse(llaves[2]);
                //    var resultados = DataEmprendeconextion.resultados_encuesta.Where(item => (item.id_aplicacion_encuesta == id_aplicacion &&
                //        item.id_inciso == id_inciso && item.id_sujeto == id_sujeto));
                //    ActualizaGridResultados(dataGridView1, resultados);
                //    ActualizaBotones();
                //    break;
                case "Catalogo":
                    id = int.Parse(valores[1]);
                    var catalogo = DataEmprendeconextion.catalogo.Where(item => item.id_catalogo == id).First();
                    ActualizaGridCatalogos(dataGridView1, catalogo);
                    //ActualizaBotones(tipos_entidades.detalle_catalogo);
                    ActualizaBotones(tipos_entidades.catalogo);
                    break;
                default:
                    break;
            }
        }


        #endregion 



        //private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        //{
        //    switch (e.Node.Name)
        //    {
        //        case "00":
        //            ClearEventos(e);
        //            break;
        //        default:
        //            ClearEventosEntidad(e.Node.Name, e);
        //            break;
        //    }            
        //}


        //private void ClearEventos(TreeViewCancelEventArgs e)
        //{
        //    switch (e.Node.Text)
        //    {
        //        case "Encuestas":
        //            //EncuestasTodas
        //            DesactualizaBotones(tipos_entidades.encuestas);
        //            break;
        //        case "Temáticas":
        //            //tematicas de la encuesta con este id                    
        //            DesactualizaBotones(tipos_entidades.tematicas);
        //            break;                
        //        case "Aplicaciones de Encuesta":                    
        //            DesactualizaBotones(tipos_entidades.aplicaciones_encuesta);
        //            break;
        //        default:Temática
        //            break;
        //    }
        //}


        //private void ClearEventosEntidad(string llave, TreeViewCancelEventArgs e)
        //{
        //    string[] valores = llave.Split(new string[] { " " }, StringSplitOptions.None);                        
        //    switch (valores[0])
        //    {
        //        case "Encuesta":
        //            DesactualizaBotones(tipos_entidades.encuesta);
        //            break;
        //        case "Tematica":
        //        case "Temática":
        //            DesactualizaBotones(tipos_entidades.tematica);
        //            break;
        //        case "Pregunta":
        //            DesactualizaBotones(tipos_entidades.pregunta);
        //            break;
        //        case "Inciso":
        //            DesactualizaBotones(tipos_entidades.inciso);
        //            break;
        //        case "AplicacionEncuesta":
        //            DesactualizaBotones(tipos_entidades.aplicacion_encuesta);
        //            break;
        //        case "Catalogo":
        //            DesactualizaBotones(tipos_entidades.catalogo);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        


        
        
        #region Eventos Botones Superior

        #region Encuesta

        private void bnEncuestaNuevaEncuesta_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmNuevaEncuesta f = new frmNuevaEncuesta(DataEmprendeconextion, this, nodoSeleccionado);
            f.ShowDialog();
            LlenarDataGridView(nodoSeleccionado);
        }
            
        private void bnEncuestaNuevaTematica_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    encuesta encuesta;
                    if (nodoSeleccionado.Name == "00")
                    {
                        int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                        encuesta = (from item in DataEmprendeconextion.tematica where item.id == id select item).First().encuesta;
                    }
                    else
                    {
                        int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                        encuesta = (from item in DataEmprendeconextion.encuesta where item.id == id select item).First();
                    }
                    frmAgregaTematica f = new frmAgregaTematica(DataEmprendeconextion, nodoSeleccionado,encuesta);
                    f.ShowDialog();
                    //ActualizaGridTematicas(dataGridView1, DataEmprendeconextion.tematica);
                }
            }
            catch (Exception)
            { 
            }
        }

        private void bnEncuestaEliminaEncuesta_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult d=MessageBox.Show(" Esta acción borrará la encuesta con todas sus preguntas, así como los resultados de aplicación que puedan existir en el sistema. Desea continuar.",
                "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (d == DialogResult.No)
            {
                return;
            }

            try
            {
                //this.borrar_regitro(); //La rutina encuesta la variable quien para saber que TableAdapter usar
                int idenc = dataGridView1.SelectedRows[0] != null ? (int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString())) : -1;
                encuesta s = (from enc in DataEmprendeconextion.encuesta where enc.id == idenc select enc).First();

                EliminarEncuesta(s);
                
                LlenarDataGridView(nodoSeleccionado);
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Seleccione la encuesta que desea eliminar");
            }
        }

        

        private void bnEncuestaEditarEncuesta_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //aqui cargamos la vista de la encuesta seleccionada para editarla
            try
            {
                int idenc = dataGridView1.SelectedRows[0] != null ? (int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString())) : -1;
                encuesta s = (from enc in DataEmprendeconextion.encuesta where enc.id == idenc select enc).First();
                if (Utiles.TieneAplicaciones(s))
                {
                    MessageBox.Show("La encuesta seleccionada no puede ser editada ya que tiene aplicaciones asociadas","Encuesta Emprende",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }
                frmNuevaEncuesta f = new frmNuevaEncuesta(DataEmprendeconextion, s, nodoSeleccionado);
                f.ShowDialog();
                LlenarDataGridView(nodoSeleccionado);
                //actualizar el arbol
            }
            catch (Exception e1)
            {

                MessageBox.Show(e1.Message, "Seleccione una Encuesta para Editarla");
            }
        }

        private void bnEncuestaAplicarEncuesta_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try 
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int )dataGridView1.SelectedRows[0].Cells[0].Value;
                    encuesta enc=(from item in DataEmprendeconextion.encuesta where item.id==identificador select item).First();

                    frmAplicarEncuesta formulario = new frmAplicarEncuesta(DataEmprendeconextion,nodoSeleccionado, enc);
                    formulario.ShowDialog();
                    //ActualizaGridAplicaciones(dataGridView1,DataEmprendeconextion.aplicacion_encuesta);
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show("Seleccione la encuesta que desea aplicar");
            }
            
        }

        private void bnEncuestaInforme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int idenc = dataGridView1.SelectedRows[0] != null ? (int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString())) : -1;
                encuesta s = (from enc in DataEmprendeconextion.encuesta where enc.id == idenc select enc).First();

                //LLenarDatosReporteEncuesta(DataEmprendeconextion, s);
                frmInformeEncuesta frm = new frmInformeEncuesta(DataEmprendeconextion,s);
                DialogResult result=frm.ShowDialog();
                //if (frm.todas)
                //{
                //    LLenarDatosReporteEncuesta(DataEmprendeconextion, s.aplicacion_encuesta.ToList());
                //}
                //else
                //{
                //    DateTime first = frm.first;
                //    DateTime last = frm.last;
                //    LLenarDatosReporteEncuesta(DataEmprendeconextion, s.aplicacion_encuesta.Where(item => (item.fecha.Date >= first.Date && item.fecha.Date <= last.Date)).ToList());
                //}
                //if (result == DialogResult.OK)
                if (frm.Ok)
                {
                    if (frm.aplicaciones.Count > 0)
                    {
                        LLenarDatosReporteEncuesta(DataEmprendeconextion, frm.aplicaciones);
                    }
                    else
                    {
                        MessageBox.Show("No existen datos para este reporte","Encuesta Emprende");
                        return;
                    }
                }
                else
                {
                    return;
                }

                MostrarReporte();

                //XtraReportAplicacionPromediado reporte = new XtraReportAplicacionPromediado();
                //reporte.CreateDocument();
                //Form_reportes Form_reportes1 = new Form_reportes();
                //Form_reportes1.printBarManager1.PrintControl.PrintingSystem = reporte.PrintingSystem;
                //Form_reportes1.ShowDialog();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Seleccione una Encuesta para Mostrar el Informe");
            }
        }

        private void bnEncuestaImprimir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int idenc = dataGridView1.SelectedRows[0] != null ? (int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString())) : -1;
                encuesta s = (from enc in DataEmprendeconextion.encuesta where enc.id == idenc select enc).First();
                Utiles.ImprimirEncuesta(s, DataEmprendeconextion);      
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Seleccione una Encuesta para Imprimir");
            }
        }

        //todo reporte entre fechas
        /*
        private void bnEncuestaInformeFechas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int idenc = dataGridView1.SelectedRows[0] != null ? (int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString())) : -1;
                encuesta s = (from enc in DataEmprendeconextion.encuesta where enc.id == idenc select enc).First();

                DateTime first=DateTime.MinValue;
                DateTime last=DateTime.Today;
                
                
                

                //LLenarDatosReporteEncuesta(DataEmprendeconextion, s, first,  last );
                LLenarDatosReporteEncuesta(DataEmprendeconextion,s.aplicacion_encuesta.Where(item=>(item.fecha>=first&&item.fecha<=last)).ToList());

                XtraReportAplicacion reporte = new XtraReportAplicacion();
                reporte.CreateDocument();
                Form_reportes Form_reportes1 = new Form_reportes();
                Form_reportes1.printBarManager1.PrintControl.PrintingSystem = reporte.PrintingSystem;
                Form_reportes1.ShowDialog();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Seleccione una Encuesta para Mostrar el Informe");
            }
        }
        */
        #endregion 

        #region Aplicacion

        private void bnAplicacionNuevaAplicacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //SelexionarEncuesta s = new SelexionarEncuesta(DataEmprendeconextion);
            //s.ShowDialog();
            //ActualizaGridAplicaciones(dataGridView1, DataEmprendeconextion.aplicacion_encuesta);
            try
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    encuesta encuesta;
                    if (nodoSeleccionado.Name == "00")
                    {
                        int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                        encuesta = (from item in DataEmprendeconextion.aplicacion_encuesta where item.id == id select item).First().encuesta;
                    }
                    else
                    {
                        int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                        encuesta = (from item in DataEmprendeconextion.encuesta where item.id == id select item).First();
                    }
                    frmAplicarEncuesta f = new frmAplicarEncuesta(DataEmprendeconextion, nodoSeleccionado, encuesta);
                    f.ShowDialog();
                    //ActualizaGridAplicaciones(dataGridView1, DataEmprendeconextion.aplicacion_encuesta);
                    LlenarDataGridView(nodoSeleccionado);
                }
            }
            catch (Exception)
            {
            }
        }

        private void bnAplicacionEliminarAplicacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DialogResult d = MessageBox.Show(" Esta acción borrará los resultados asociados a la aplicación de la encuesta. Desea continuar.",
                "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d == DialogResult.No)
                {
                    return;
                }
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    aplicacion_encuesta aplicacion= (from item in DataEmprendeconextion.aplicacion_encuesta where item.id == identificador select item).First();
                    EliminarAplicacionEncuesta(aplicacion);
                    //ActualizaGridAplicaciones(dataGridView1, DataEmprendeconextion.aplicacion_encuesta);
                    LlenarDataGridView(nodoSeleccionado);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione la Aplicación que desea eliminar");
            }
        }

        private void bnAplicacionEditarAplicacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int idenc = dataGridView1.SelectedRows[0] != null ? (int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString())) : -1;
                aplicacion_encuesta aplicacion= (from item in DataEmprendeconextion.aplicacion_encuesta where item.id == idenc select item).First();
                
                frmAplicarEncuesta f = new frmAplicarEncuesta(DataEmprendeconextion, aplicacion,tipo_Accion.EditarAplicacion);
                f.ShowDialog();
                ActualizaGridAplicaciones(dataGridView1, DataEmprendeconextion.aplicacion_encuesta);
                Utiles.ModificaAplicacion(nodoSeleccionado, aplicacion);
                LlenarDataGridView(nodoSeleccionado);
            }
            catch (Exception e1)
            {

                MessageBox.Show(e1.Message, "Seleccione una la Aplicación para Editarla");
            }
        }

        private void bnAplicacionVolverAplicar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    aplicacion_encuesta aplicacion = (from item in DataEmprendeconextion.aplicacion_encuesta where item.id == identificador select item).First();
                    frmAplicarEncuesta formulario = new frmAplicarEncuesta(DataEmprendeconextion, aplicacion,tipo_Accion.VolverAplicar);
                    formulario.ShowDialog();
                    //ActualizaGridAplicaciones(dataGridView1, DataEmprendeconextion.aplicacion_encuesta);
                    LlenarDataGridView(nodoSeleccionado);
                }                
            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione la Aplicación que desea reaplicar");
            }
        }


        private void bnAplicacionInforme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    aplicacion_encuesta aplicacion = (from item in DataEmprendeconextion.aplicacion_encuesta where item.id == identificador select item).First();

                    LLenarDatosReporteAplicacion(DataEmprendeconextion, aplicacion);

                    MostrarReporte();
                    //XtraReportAplicacion reporte = new XtraReportAplicacion();
                    //XtraReportAplicacionPromediado reporte = new XtraReportAplicacionPromediado();
                    //reporte.CreateDocument();
                    //Form_reportes Form_reportes1 = new Form_reportes();
                    //Form_reportes1.printBarManager1.PrintControl.PrintingSystem = reporte.PrintingSystem;
                    //Form_reportes1.ShowDialog();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione la Aplicación que desea visualizar el Informe");
            }
        }

        private void bnAplicacionRevisar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    aplicacion_encuesta aplicacion = (from item in DataEmprendeconextion.aplicacion_encuesta where item.id == identificador select item).First();
                    frmAplicarEncuesta formulario = new frmAplicarEncuesta(DataEmprendeconextion, aplicacion, tipo_Accion.ModificarAplicaciones);
                    formulario.ShowDialog();
                    //ActualizaGridAplicaciones(dataGridView1, DataEmprendeconextion.aplicacion_encuesta);
                    LlenarDataGridView(nodoSeleccionado);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione la Aplicación que desea revisar");
            }
        }

        #endregion 

        #region Temática

        private void bnTematicaNuevaTematica_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    encuesta encuesta;
                    if (nodoSeleccionado.Name == "00")
                    {
                        int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                        encuesta = (from item in DataEmprendeconextion.tematica where item.id == id select item).First().encuesta;
                    }
                    else
                    {
                        int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                        encuesta = (from item in DataEmprendeconextion.encuesta where item.id == id select item).First();
                    }
                    frmAgregaTematica f = new frmAgregaTematica(DataEmprendeconextion, nodoSeleccionado, encuesta);
                    f.ShowDialog();
                    //ActualizaGridTematicas(dataGridView1, DataEmprendeconextion.tematica);
                    LlenarDataGridView(nodoSeleccionado);
                }
            }
            catch (Exception)
            {
            }
        }

        private void bnTematicaNuevaPregunta_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    tematica tematica;
                    if (nodoSeleccionado.Name == "00")
                    {
                        int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                        tematica = (from item in DataEmprendeconextion.preguntas where item.id_pregunta == id select item).First().tematica;
                    }
                    else
                    {
                        int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                        tematica = (from item in DataEmprendeconextion.tematica where item.id == id select item).First();
                    }
                    frmAgregaPregunta f = new frmAgregaPregunta(DataEmprendeconextion, nodoSeleccionado, tematica);
                    f.ShowDialog();
                    //ActualizaGridPreguntas(dataGridView1, DataEmprendeconextion.preguntas);
                }
            }
            catch (Exception)
            {
            }
            
            
        }

        private void bnTematicaEliminarTematica_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DialogResult d = MessageBox.Show(" Esta acción borrará la temática junto con todas sus preguntas. Desea continuar.",
               "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d == DialogResult.No)
                {
                    return;
                }
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    tematica tematicaEliminar= (from item in DataEmprendeconextion.tematica where item.id == identificador select item).First();

                    if (Utiles.TieneAplicaciones(tematicaEliminar))
                    {
                        MessageBox.Show("La temática seleccionada no puede ser eliminada ya que tiene aplicaciones asociadas", "Encuesta Emprende", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    EliminarTematica(tematicaEliminar);
                    //ActualizaGridTematicas(dataGridView1, DataEmprendeconextion.tematica);
                    LlenarDataGridView(nodoSeleccionado);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione la Temática que desea eliminar");
            }
        }

        private void bnTematicaEditarTematica_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0]!=null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    tematica tematicaEditar = (from item in DataEmprendeconextion.tematica where item.id == identificador select item).First();

                    if (Utiles.TieneAplicaciones(tematicaEditar))
                    {
                        MessageBox.Show("La temática seleccionada no puede ser editada ya que tiene aplicaciones asociadas", "Encuesta Emprende", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    frmAgregaTematica formulario = new frmAgregaTematica(DataEmprendeconextion, nodoSeleccionado, tematicaEditar);
                    formulario.ShowDialog();
                    //ActualizaGridTematicas(dataGridView1, DataEmprendeconextion.tematica);
                    LlenarDataGridView(nodoSeleccionado);
                    Utiles.ModificaTematica(nodoSeleccionado, tematicaEditar);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione la Temática que desea editar");
            }
        }

        #endregion 


        #region Pregunta

        private void bnPreguntaNuevoInciso_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    preguntas pregunta;
                    if (nodoSeleccionado.Name == "00")
                    {
                        int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                        pregunta = (from item in DataEmprendeconextion.incisos where item.id_inciso== id select item).First().preguntas;
                    }
                    else
                    {
                        int id = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                        pregunta = (from item in DataEmprendeconextion.preguntas where item.id_pregunta == id select item).First();
                    }
                    frmAgregaInciso formulario = new frmAgregaInciso(DataEmprendeconextion, nodoSeleccionado, pregunta);
                    formulario.ShowDialog();
                    //ActualizaGridPreguntas(dataGridView1, DataEmprendeconextion.preguntas);
                }
            }
            catch (Exception)
            {
            }         
        }

        private void bnPreguntaEditarPreguntar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    preguntas preguntaEditar = (from item in DataEmprendeconextion.preguntas where item.id_pregunta == identificador select item).First();

                    if (Utiles.TieneAplicaciones(preguntaEditar))
                    {
                        MessageBox.Show("La pregunta seleccionada no puede ser editada ya que tiene aplicaciones asociadas", "Encuesta Emprende", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    frmAgregaPregunta formulario = new frmAgregaPregunta(DataEmprendeconextion, nodoSeleccionado, preguntaEditar);
                    formulario.ShowDialog();
                    //ActualizaGridPreguntas(dataGridView1, DataEmprendeconextion.preguntas);
                    LlenarDataGridView(nodoSeleccionado);
                    Utiles.ModificaPregunta(nodoSeleccionado, preguntaEditar);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione la Pregunta que desea editar");
            }
        }

        private void bnPreguntaEliminarPregunta_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DialogResult d = MessageBox.Show(" Esta acción borrará la pregunta junto con todos sus incisos. Desea continuar.",
               "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d == DialogResult.No)
                {
                    return;
                }
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    preguntas preguntaEliminar = (from item in DataEmprendeconextion.preguntas where item.id_pregunta == identificador select item).First();

                    if (Utiles.TieneAplicaciones(preguntaEliminar))
                    {
                        MessageBox.Show("La pregunta seleccionada no puede ser eliminada ya que tiene aplicaciones asociadas", "Encuesta Emprende", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    EliminarPregunta(preguntaEliminar);
                    //ActualizaGridPreguntas(dataGridView1, DataEmprendeconextion.preguntas);
                    LlenarDataGridView(nodoSeleccionado);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione la Pregunta que desea eliminar");
            }
        }

        #endregion 

        #region Inciso

        private void bnIncisoEditarInciso_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    incisos incisoEditar = (from item in DataEmprendeconextion.incisos where item.id_inciso == identificador select item).First();
                    //preguntas pregunta = incisoEditar.preguntas;

                    if (Utiles.TieneAplicaciones(incisoEditar))
                    {
                        MessageBox.Show("El inciso seleccionado no puede ser editado ya que tiene aplicaciones asociadas", "Encuesta Emprende", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }


                    frmAgregaInciso formulario = new frmAgregaInciso(DataEmprendeconextion, nodoSeleccionado, incisoEditar);
                    formulario.ShowDialog();
                    //ActualizaGridIncisos(dataGridView1, DataEmprendeconextion.incisos);
                    LlenarDataGridView(nodoSeleccionado);
                    Utiles.ModificaInciso(nodoSeleccionado, incisoEditar );
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione el inciso que desea editar");
            }
        }

        private void bnIncisoEliminarInciso_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DialogResult d = MessageBox.Show(" Esta acción borrará el inciso. Desea continuar.",
               "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d == DialogResult.No)
                {
                    return;
                }
                if (dataGridView1.SelectedRows[0] != null)
                {
                    int identificador = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                    incisos incisoEliminar = (from item in DataEmprendeconextion.incisos where item.id_inciso == identificador select item).First();

                    if (Utiles.TieneAplicaciones(incisoEliminar))
                    {
                        MessageBox.Show("El inciso seleccionado no puede ser eliminado ya que tiene aplicaciones asociadas", "Encuesta Emprende", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }


                    EliminarInciso(incisoEliminar);
                    //ActualizaGridIncisos(dataGridView1, DataEmprendeconextion.incisos);
                    //Utiles.EliminaInciso(nodoSeleccionado,incisoEliminar);
                    LlenarDataGridView(nodoSeleccionado);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Seleccione el inciso que desea eliminar");
            }
        }

        #endregion 

        private void bnReporteGeneral_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MostrarReporte();
        }


        private void bnReporteTreeActualizar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Utiles.LLenarTreeView(DataEmprendeconextion, treeView1);
        }

        private void bnOpcionesCatalogos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCatalogo formulario = new frmCatalogo(DataEmprendeconextion);
            formulario.ShowDialog();

            //frmCatalogos formularios = new frmCatalogos();
            //formularios.ShowDialog();
        }
      
        #endregion

        private void EliminarEncuesta(encuesta s)
        {
            Utiles.EliminaEncuesta(nodoSeleccionado, s);

            var tematicasList = s.tematica.ToList();
            var aplicacionesList = s.aplicacion_encuesta.ToList();
            foreach (var item in tematicasList)
            {
                DataEmprendeconextion.DeleteObject(item);
                DataEmprendeconextion.SaveChanges();
            }
           
            foreach (var item in aplicacionesList)
            {
                DataEmprendeconextion.DeleteObject(item);
                DataEmprendeconextion.SaveChanges();
            }
            
            
            DataEmprendeconextion.DeleteObject(s);
            //DataEmprendeconextion.encuesta.DeleteObject(s);
            DataEmprendeconextion.SaveChanges();
        }

        private void EliminarAplicacionEncuesta(aplicacion_encuesta aplicacion)
        {
            Utiles.EliminaAplicacion(nodoSeleccionado, aplicacion);
            DataEmprendeconextion.aplicacion_encuesta.DeleteObject(aplicacion);
            DataEmprendeconextion.SaveChanges();
        }

        private void EliminarTematica(tematica tematicaEliminar)
        {
            Utiles.EliminaTematica(nodoSeleccionado, tematicaEliminar);
            DataEmprendeconextion.tematica.DeleteObject(tematicaEliminar);
            DataEmprendeconextion.SaveChanges();
        }

        private void EliminarPregunta(preguntas preguntaEliminar)
        {
            Utiles.EliminaPregunta(nodoSeleccionado, preguntaEliminar);
            DataEmprendeconextion.preguntas.DeleteObject(preguntaEliminar);
            DataEmprendeconextion.SaveChanges();
        }

        private void EliminarInciso(incisos incisoEliminar)
        {
            
            Utiles.EliminaInciso(nodoSeleccionado, incisoEliminar);
            DataEmprendeconextion.incisos.DeleteObject(incisoEliminar);
            DataEmprendeconextion.SaveChanges();
        }


        #region Reporte
        //no utilizado Reporte General
        /*
        private void LLenarDatosReporteGeneral()
        {
            try
            {
                IList<temporal_encuesta> temporalEncuestaList = DataEmprendeconextion.temporal_encuesta.ToList();
                foreach (var item in temporalEncuestaList)
                {
                    DataEmprendeconextion.temporal_encuesta.DeleteObject(item);
                }
                DataEmprendeconextion.SaveChanges();

                IList<aplicacion_encuesta> aplicacionList = DataEmprendeconextion.aplicacion_encuesta.ToList();
                IList<tematica> tematicaList;
                IList<preguntas> preguntaList;
                IList<incisos> incisoList;
                IList<resultados_encuesta> resultadosList;
                IList<detalle_catalogo> detallesList;
                foreach (var aplicacion in aplicacionList)
                {
                    tematicaList = aplicacion.encuesta.tematica.ToList();
                    foreach (var tematica in tematicaList)
                    {
                        preguntaList = tematica.preguntas.ToList();
                        foreach (var pregunta in preguntaList)
                        {
                            incisoList = pregunta.incisos.ToList();
                            foreach (var inciso in incisoList)
                            {
                                if (inciso.tipo_inciso.tipo_inciso1 == "Escribir")
                                {
                                    resultadosList=aplicacion.resultados_encuesta.Where(item=>item.id_inciso==inciso.id_inciso).ToList();
                                    foreach (var resultado in resultadosList)
                                    {
                                        temporal_encuesta temporal = new temporal_encuesta();
                                        temporal.id = DataEmprendeconextion.temporal_encuesta.Count();
                                        temporal.AplicacionDescripcion = aplicacion.descripcion;
                                        temporal.AplicacionId = aplicacion.id;
                                        temporal.AplicacionFecha = aplicacion.fecha;

                                        //temporal.DetalleId
                                        //temporal.DetalleNombre = "";
                                        //temporal.DetalleNumeral
                                        //temporal.DetallePeso


                                        temporal.EncuestaEncuesta = aplicacion.encuesta.encuesta1;
                                        temporal.EncuestaId = aplicacion.encuesta.id;
                                        
                                        temporal.IncisoId = inciso.id_inciso;
                                        temporal.IncisoInciso = inciso.inciso.Trim();
                                        temporal.IncisoNumeral = inciso.numeral;

                                        temporal.PreguntaId = pregunta.id_pregunta;
                                        temporal.PreguntaPregunta = pregunta.pregunta;

                                        temporal.ResultadoIdSujeto = resultado.id_sujeto;
                                        temporal.ResultadoResultado = resultado.resultado;
                                        temporal.ResultadoResultadoTexto = resultado.resultado_texto;

                                        temporal.TematicaId = tematica.id;
                                        temporal.TematicaTematica = tematica.tematica1;

                                        temporal.TipoIncisoId = inciso.tipo_inciso.id_tipo_inciso;
                                        temporal.TipoIncisoTipoInciso = inciso.tipo_inciso.tipo_inciso1;
                                                                                
                                        DataEmprendeconextion.temporal_encuesta.AddObject(temporal);
                                        DataEmprendeconextion.SaveChanges();
                                    }
                                }
                                else
                                {                                    
                                    int total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Count();
                                    detallesList=inciso.catalogo.detalle_catalogo.ToList();
                                    foreach (var detalle in detallesList)
                                    {
                                        temporal_encuesta temporal = new temporal_encuesta();
                                        temporal.id = DataEmprendeconextion.temporal_encuesta.Count();

                                        temporal.AplicacionDescripcion = aplicacion.descripcion;
                                        temporal.AplicacionId = aplicacion.id;
                                        temporal.AplicacionFecha = aplicacion.fecha;

                                        temporal.DetalleId = detalle.id;
                                        temporal.DetalleNombre = detalle.nombre;
                                        temporal.DetalleNumeral = detalle.numeral;
                                        temporal.DetallePeso = detalle.peso;

                                        temporal.EncuestaEncuesta = aplicacion.encuesta.encuesta1;
                                        temporal.EncuestaId = aplicacion.encuesta.id;

                                        temporal.IncisoId = inciso.id_inciso;
                                        temporal.IncisoInciso = inciso.inciso.Trim();
                                        temporal.IncisoNumeral = inciso.numeral;

                                        temporal.PreguntaId = pregunta.id_pregunta;
                                        temporal.PreguntaPregunta = pregunta.pregunta;

                                        //temporal.ResultadoIdSujeto = resultado.id_sujeto;
                                        //temporal.ResultadoResultado = resultado.resultado;
                                        int cantidadDetalles = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && item.resultado==detalle.id).Count();
                                        double porCiento=(double)(cantidadDetalles*100)/((double)total);
                                        //temporal.ResultadoResultadoTexto = cantidadDetalles + " de " + total+" para "+porCiento+" %";
                                        temporal.ResultadoResultadoTexto = "(Evaluado como " +cantidadDetalles + " de " + total + " para un " + Redondeo(porCiento,1) + " %)";

                                        temporal.TematicaId = tematica.id;
                                        temporal.TematicaTematica = tematica.tematica1;

                                        temporal.TipoIncisoId = inciso.tipo_inciso.id_tipo_inciso;
                                        temporal.TipoIncisoTipoInciso = inciso.tipo_inciso.tipo_inciso1;

                                        DataEmprendeconextion.temporal_encuesta.AddObject(temporal);
                                        DataEmprendeconextion.SaveChanges();
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }
        */

        //no utilizado Reporte General este es el mas viejo
        /*
        private void LLenarDatosReporteGeneral()
        {
            try
            {
                IList<temporal_encuesta> temporalEncuestaList = DataEmprendeconextion.temporal_encuesta.ToList();
                foreach (var item in DataEmprendeconextion.temporal_encuesta)
                {
                    DataEmprendeconextion.temporal_encuesta.DeleteObject(item);
                }
                DataEmprendeconextion.SaveChanges();

                foreach (var aplicacion in DataEmprendeconextion.aplicacion_encuesta)
                {
                    foreach (var tematica in aplicacion.encuesta.tematica)
                    {
                        foreach (var pregunta in tematica.preguntas)
                        {
                            foreach (var inciso in pregunta.incisos)
                            {


                                if (inciso.tipo_inciso.tipo_inciso1 == "Escribir")
                                {
                                    foreach (var resultado in aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso))
                                    {
                                        temporal_encuesta temporal = new temporal_encuesta();
                                        temporal.AplicacionDescripcion = aplicacion.descripcion;
                                        temporal.AplicacionId = aplicacion.id;
                                        temporal.AplicacionFecha = aplicacion.fecha;

                                        temporal.IncisoId = inciso.id_inciso;
                                        temporal.IncisoInciso = inciso.inciso.Trim();
                                        temporal.IncisoNumeral = inciso.numeral;

                                        temporal.ResultadoResultadoTexto = resultado.resultado_texto;

                                        DataEmprendeconextion.temporal_encuesta.AddObject(temporal);
                                        DataEmprendeconextion.SaveChanges();
                                    }
                                }
                                else
                                {
                                    int total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Count();
                                    foreach (var detalle in inciso.catalogo.detalle_catalogo)
                                    {
                                        temporal_encuesta temporal = new temporal_encuesta();
                                        temporal.AplicacionDescripcion = aplicacion.descripcion;
                                        temporal.AplicacionId = aplicacion.id;
                                        temporal.AplicacionFecha = aplicacion.fecha;

                                        temporal.IncisoId = inciso.id_inciso;
                                        temporal.IncisoInciso = inciso.inciso.Trim();
                                        temporal.IncisoNumeral = inciso.numeral;

                                        int cantidadDetalles = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && item.resultado == detalle.id).Count();

                                        temporal.ResultadoResultadoTexto = cantidadDetalles + " - " + total;

                                        DataEmprendeconextion.temporal_encuesta.AddObject(temporal);
                                        DataEmprendeconextion.SaveChanges();
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }

        */


        #region LLenarDatosReporteEncuestaEntreFechasComentado
        //private static void LLenarDatosReporteEncuesta(DataEmprendeEntities2 conexion, encuesta encuestaAReporte, DateTime first, DateTime last)
        //{
        //    try
        //    {
        //        IList<temporal_encuesta> temporalEncuestaList = conexion.temporal_encuesta.ToList();
        //        foreach (var item in temporalEncuestaList)
        //        {
        //            conexion.temporal_encuesta.DeleteObject(item);
        //        }
        //        conexion.SaveChanges();

        //        IList<aplicacion_encuesta> aplicacionList = encuestaAReporte.aplicacion_encuesta.Where(item=>(item.fecha>=first && item.fecha<=last)).ToList();
        //        IList<tematica> tematicaList;
        //        IList<preguntas> preguntaList;
        //        IList<incisos> incisoList;
        //        IList<resultados_encuesta> resultadosList;
        //        IList<detalle_catalogo> detallesList;

        //        Queue<object> coleccionesValores = new Queue<object>();

        //        foreach (var aplicacion in aplicacionList)
        //        {
        //            Dictionary<tematica, ValoresManager> tematicasValores = new Dictionary<tematica, ValoresManager>();
        //            Dictionary<preguntas, ValoresManager> preguntasValores = new Dictionary<preguntas, ValoresManager>();
        //            Dictionary<incisos, ValoresManager> incisosValores = new Dictionary<incisos, ValoresManager>();

        //            tematicaList = aplicacion.encuesta.tematica.ToList();
        //            foreach (var tematica in tematicaList)
        //            {
        //                preguntaList = tematica.preguntas.ToList();
        //                ValoresManager tematicaValor = new ValoresManager();
        //                foreach (var pregunta in preguntaList)
        //                {
        //                    incisoList = pregunta.incisos.ToList();
        //                    ValoresManager preguntaValor = new ValoresManager();
        //                    foreach (var inciso in incisoList)
        //                    {
        //                        ValoresManager incisoValor = new ValoresManager();
        //                        int total = 0;
        //                        double average = 0.0;
        //                        int maximo = int.MinValue;

        //                        if (inciso.tipo_inciso.tipo_inciso1 != "Escribir")
        //                        {
        //                            total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Count();
        //                            //total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso&&conexion.detalle_catalogo.Where(det=>det.id==item.resultado).First().peso!=0).Count();
        //                            var auxiliarResultados = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso != 0);
        //                            if (auxiliarResultados.Count() > 0)
        //                            {
        //                                average = auxiliarResultados.Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
        //                            }
        //                            else
        //                            {
        //                                average = 0;
        //                            }
        //                            double averageAux = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
        //                            //if (average != averageAux)
        //                            //{
        //                            //    averageAux = 10;
        //                            //}
        //                            //average = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso != 0).Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
        //                            maximo = inciso.catalogo.detalle_catalogo.Max(item => item.peso);
        //                            //maximo = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Max(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);                                    
        //                        }
        //                        incisoValor.Add(average, total, maximo);
        //                        preguntaValor.Add(average, total, maximo);
        //                        tematicaValor.Add(average, total, maximo);
        //                        incisosValores.Add(inciso, incisoValor);
        //                    }
        //                    preguntasValores.Add(pregunta, preguntaValor);
        //                }
        //                tematicasValores.Add(tematica, tematicaValor);
        //            }
        //            //coleccionesValores.Add(incisosValores);
        //            //coleccionesValores.Add(preguntasValores);
        //            //coleccionesValores.Add(tematicasValores);
        //            coleccionesValores.Enqueue(incisosValores);
        //            coleccionesValores.Enqueue(preguntasValores);
        //            coleccionesValores.Enqueue(tematicasValores);
        //        }

        //        foreach (var aplicacion in aplicacionList)
        //        {
        //            Dictionary<incisos, ValoresManager> incisosValores = (Dictionary<incisos, ValoresManager>)coleccionesValores.Dequeue();
        //            Dictionary<preguntas, ValoresManager> preguntasValores = (Dictionary<preguntas, ValoresManager>)coleccionesValores.Dequeue();
        //            Dictionary<tematica, ValoresManager> tematicasValores = (Dictionary<tematica, ValoresManager>)coleccionesValores.Dequeue();


        //            tematicaList = aplicacion.encuesta.tematica.ToList();
        //            //double tematicaPromedio=conexion.resultados_encuesta.Where(item=> conexion.incisos.Where(inciso=>inciso.id_inciso==item.id_inciso).First().preguntas.id_tematica==
        //            //double tematicaPromedio=conexion.resultados_encuesta.Sum(item=>item.pe
        //            foreach (var tematica in tematicaList)
        //            {
        //                preguntaList = tematica.preguntas.ToList();
        //                foreach (var pregunta in preguntaList)
        //                {
        //                    incisoList = pregunta.incisos.ToList();
        //                    foreach (var inciso in incisoList)
        //                    {
        //                        if (inciso.tipo_inciso.tipo_inciso1 == "Escribir")
        //                        {
        //                            resultadosList = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).ToList();
        //                            foreach (var resultado in resultadosList)
        //                            {
        //                                temporal_encuesta temporal = new temporal_encuesta();
        //                                temporal.id = conexion.temporal_encuesta.Count();
        //                                temporal.AplicacionDescripcion = aplicacion.descripcion;
        //                                temporal.AplicacionId = aplicacion.id;
        //                                temporal.AplicacionFecha = aplicacion.fecha;

        //                                //temporal.DetalleId
        //                                //temporal.DetalleNombre = "";
        //                                //temporal.DetalleNumeral
        //                                //temporal.DetallePeso


        //                                temporal.EncuestaEncuesta = aplicacion.encuesta.encuesta1;
        //                                temporal.EncuestaId = aplicacion.encuesta.id;

        //                                temporal.IncisoId = inciso.id_inciso;
        //                                temporal.IncisoInciso = inciso.inciso.Trim();
        //                                temporal.IncisoNumeral = inciso.numeral;

        //                                temporal.PreguntaId = pregunta.id_pregunta;
        //                                temporal.PreguntaPregunta = pregunta.pregunta;

        //                                temporal.ResultadoIdSujeto = resultado.id_sujeto;
        //                                temporal.ResultadoResultado = resultado.resultado;
        //                                temporal.ResultadoResultadoTexto = resultado.resultado_texto;

        //                                temporal.TematicaId = tematica.id;
        //                                temporal.TematicaTematica = tematica.tematica1;

        //                                temporal.TipoIncisoId = inciso.tipo_inciso.id_tipo_inciso;
        //                                temporal.TipoIncisoTipoInciso = inciso.tipo_inciso.tipo_inciso1;

        //                                conexion.temporal_encuesta.AddObject(temporal);
        //                                conexion.SaveChanges();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            int total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Count();
        //                            detallesList = inciso.catalogo.detalle_catalogo.ToList();
        //                            foreach (var detalle in detallesList)
        //                            {
        //                                temporal_encuesta temporal = new temporal_encuesta();
        //                                temporal.id = conexion.temporal_encuesta.Count();

        //                                temporal.AplicacionDescripcion = aplicacion.descripcion;
        //                                temporal.AplicacionId = aplicacion.id;
        //                                temporal.AplicacionFecha = aplicacion.fecha;

        //                                temporal.DetalleId = detalle.id;
        //                                temporal.DetalleNombre = detalle.nombre;
        //                                temporal.DetalleNumeral = detalle.numeral;
        //                                temporal.DetallePeso = detalle.peso;

        //                                temporal.EncuestaEncuesta = aplicacion.encuesta.encuesta1;
        //                                temporal.EncuestaId = aplicacion.encuesta.id;

        //                                temporal.IncisoId = inciso.id_inciso;
        //                                ValoresManager temporalValor = incisosValores[inciso];
        //                                temporal.IncisoInciso = inciso.inciso.Trim() + " " + temporalValor.Promedio + " de " + temporalValor.Maximo;
        //                                temporal.IncisoNumeral = inciso.numeral;

        //                                temporalValor = preguntasValores[pregunta];
        //                                temporal.PreguntaId = pregunta.id_pregunta;
        //                                temporal.PreguntaPregunta = pregunta.pregunta + " " + temporalValor.Promedio + " de " + temporalValor.Maximo;

        //                                //temporal.ResultadoIdSujeto = resultado.id_sujeto;
        //                                //temporal.ResultadoResultado = resultado.resultado;
        //                                int cantidadDetalles = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && item.resultado == detalle.id).Count();
        //                                double porCiento = (double)(cantidadDetalles * 100) / ((double)total);
        //                                temporal.ResultadoResultadoTexto = "(Evaluado como " + cantidadDetalles + " de " + total + " para un " + Redondeo(porCiento,1) + " %)";
        //nuevo para los reportes
        //temporal.Calificacion = cantidadDetalles;
        //temporal.Maximo = total;
        //temporal.Porcentaje = double.Parse( Redondeo(porCiento, 1));
        //
        //                                temporalValor = tematicasValores[tematica];
        //                                temporal.TematicaId = tematica.id;
        //                                temporal.TematicaTematica = tematica.tematica1 + " " + temporalValor.Promedio + " de " + temporalValor.Maximo;

        //                                temporal.TipoIncisoId = inciso.tipo_inciso.id_tipo_inciso;
        //                                temporal.TipoIncisoTipoInciso = inciso.tipo_inciso.tipo_inciso1;

        //                                conexion.temporal_encuesta.AddObject(temporal);
        //                                conexion.SaveChanges();
        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    { }
        //}

        #endregion


        private static void LLenarDatosReporteAplicacion(DataEmprendeEntities2 conexion, aplicacion_encuesta aplicacionEncuesta)
        {
            try
            {
                IList<temporal_encuesta> temporalEncuestaList = conexion.temporal_encuesta.ToList();
                foreach (var item in temporalEncuestaList)
                {
                    conexion.temporal_encuesta.DeleteObject(item);
                }
                conexion.SaveChanges();



                IList<aplicacion_encuesta> aplicacionList = new List<aplicacion_encuesta>();
                aplicacionList.Add(aplicacionEncuesta);
                IList<tematica> tematicaList;
                IList<preguntas> preguntaList;
                IList<incisos> incisoList;
                IList<resultados_encuesta> resultadosList;
                IList<detalle_catalogo> detallesList;

               
                Dictionary<tematica, ValoresManager> tematicasValores = new Dictionary<tematica, ValoresManager>();
                Dictionary<preguntas, ValoresManager> preguntasValores = new Dictionary<preguntas, ValoresManager>();
                Dictionary<incisos, ValoresManager> incisosValores = new Dictionary<incisos, ValoresManager>();

                foreach (var aplicacion in aplicacionList)
                {
                    tematicaList = aplicacion.encuesta.tematica.ToList();
                    foreach (var tematica in tematicaList)
                    {
                        preguntaList = tematica.preguntas.ToList();
                        ValoresManager tematicaValor = new ValoresManager();
                        foreach (var pregunta in preguntaList)
                        {
                            incisoList = pregunta.incisos.ToList();
                            ValoresManager preguntaValor = new ValoresManager();
                            foreach (var inciso in incisoList)
                            {
                                ValoresManager incisoValor = new ValoresManager();
                                int total = 0;
                                double average = 0.0;
                                int maximo = int.MinValue;

                                if (inciso.tipo_inciso.tipo_inciso1 != "Escribir")
                                {
                                    total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Count();
                                    //double averageAux = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
                                    var auxiliarResultados = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso != 0);
                                    if (auxiliarResultados.Count() > 0)
                                    {
                                        average = auxiliarResultados.Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
                                    }
                                    else
                                    {
                                        average = 0;
                                    }
                                    //average = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso != 0).Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
                                    maximo = inciso.catalogo.detalle_catalogo.Max(item => item.peso);
                                    //maximo = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Max(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);                                    
                                }
                                incisoValor.Add(average, total, maximo);
                                preguntaValor.Add(average, total, maximo);
                                tematicaValor.Add(average, total, maximo);
                                incisosValores.Add(inciso, incisoValor);
                            }
                            preguntasValores.Add(pregunta, preguntaValor);
                        }
                        tematicasValores.Add(tematica, tematicaValor);
                    }
                }

                foreach (var aplicacion in aplicacionList)
                {
                    tematicaList = aplicacion.encuesta.tematica.ToList();
                    foreach (var tematica in tematicaList)
                    {
                        preguntaList = tematica.preguntas.ToList();
                        foreach (var pregunta in preguntaList)
                        {
                            incisoList = pregunta.incisos.ToList();
                            foreach (var inciso in incisoList)
                            {
                                if (inciso.tipo_inciso.tipo_inciso1 == "Escribir")
                                {
                                    resultadosList = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).ToList();
                                    foreach (var resultado in resultadosList)
                                    {
                                        temporal_encuesta temporal = new temporal_encuesta();
                                        temporal.id = conexion.temporal_encuesta.Count();
                                        temporal.AplicacionDescripcion = aplicacion.descripcion;
                                        temporal.AplicacionId = aplicacion.id;
                                        temporal.AplicacionFecha = aplicacion.fecha;

                                        //temporal.DetalleId
                                        //temporal.DetalleNombre = "";
                                        //temporal.DetalleNumeral
                                        //temporal.DetallePeso


                                        temporal.EncuestaEncuesta = aplicacion.encuesta.encuesta1;
                                        temporal.EncuestaId = aplicacion.encuesta.id;

                                        temporal.IncisoId = inciso.id_inciso;
                                        temporal.IncisoInciso = inciso.inciso.Trim();
                                        temporal.IncisoNumeral = inciso.numeral;

                                        temporal.PreguntaId = pregunta.id_pregunta;
                                        temporal.PreguntaPregunta = pregunta.pregunta;

                                        temporal.ResultadoIdSujeto = resultado.id_sujeto;
                                        temporal.ResultadoResultado = resultado.resultado;
                                        temporal.ResultadoResultadoTexto = resultado.resultado_texto;

                                        temporal.TematicaId = tematica.id;
                                        temporal.TematicaTematica = tematica.tematica1;

                                        temporal.TipoIncisoId = inciso.tipo_inciso.id_tipo_inciso;
                                        temporal.TipoIncisoTipoInciso = inciso.tipo_inciso.tipo_inciso1;

                                        conexion.temporal_encuesta.AddObject(temporal);
                                        conexion.SaveChanges();
                                    }
                                }
                                else
                                {
                                    int total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Count();
                                    detallesList = inciso.catalogo.detalle_catalogo.ToList();
                                    foreach (var detalle in detallesList)
                                    {
                                        temporal_encuesta temporal = new temporal_encuesta();
                                        temporal.id = conexion.temporal_encuesta.Count();

                                        temporal.AplicacionDescripcion = aplicacion.descripcion;
                                        temporal.AplicacionId = aplicacion.id;
                                        temporal.AplicacionFecha = aplicacion.fecha;

                                        temporal.DetalleId = detalle.id;
                                        temporal.DetalleNombre = detalle.nombre;
                                        temporal.DetalleNumeral = detalle.numeral;
                                        temporal.DetallePeso = detalle.peso;

                                        temporal.EncuestaEncuesta = aplicacion.encuesta.encuesta1;
                                        temporal.EncuestaId = aplicacion.encuesta.id;

                                        temporal.IncisoId = inciso.id_inciso;
                                        ValoresManager temporalValor = incisosValores[inciso];
                                        temporal.IncisoInciso = inciso.inciso.Trim() + " " + temporalValor.Promedio + " de " + temporalValor.Maximo;
                                        temporal.IncisoInciso = inciso.inciso.Trim() ;
                                        temporal.Auxiliar1 = "Calificación según peso: " + temporalValor.Promedio + " de un máximo de " + temporalValor.Maximo;
                                        temporal.IncisoNumeral = inciso.numeral;

                                        temporalValor = preguntasValores[pregunta];
                                        temporal.PreguntaId = pregunta.id_pregunta;
                                        temporal.PreguntaPregunta = pregunta.pregunta + " " + temporalValor.Promedio + " de " + temporalValor.Maximo;

                                        //temporal.ResultadoIdSujeto = resultado.id_sujeto;
                                        //temporal.ResultadoResultado = resultado.resultado;
                                        int cantidadDetalles = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && item.resultado == detalle.id).Count();
                                        double porCiento = (double)(cantidadDetalles * 100) / ((double)total);
                                        //temporal.ResultadoResultadoTexto = "(Evaluado como "+ cantidadDetalles + " de " + total + " para un " + Redondeo(porCiento,1) + "%)";
                              
                                        //todo esto es una solucion temporal para lo de los reportes 
                                        //temporal.ResultadoResultadoTexto =SalvacionReporte(cantidadDetalles,total,Redondeo(porCiento,1));
                                        temporal.ResultadoResultadoTexto = "";
                                            

                                        //nuevo para los reportes
                                        temporal.Calificacion = cantidadDetalles;
                                        temporal.Maximo = total;
                                        temporal.Porcentaje = double.Parse( Redondeo(porCiento, 1));
                                        //

                                        temporalValor = tematicasValores[tematica];
                                        temporal.TematicaId = tematica.id;
                                        temporal.TematicaTematica = tematica.tematica1 + " " + temporalValor.Promedio + " de " + temporalValor.Maximo;
                                        
                                        temporal.TipoIncisoId = inciso.tipo_inciso.id_tipo_inciso;
                                        temporal.TipoIncisoTipoInciso = inciso.tipo_inciso.tipo_inciso1;

                                        conexion.temporal_encuesta.AddObject(temporal);
                                       conexion.SaveChanges();
                                    }
                                }

                            }
                        }
                    }
                }
               // conexion.SaveChanges();
            }
            catch (Exception)
            { }
        }

        internal static void LLenarDatosReporteAplicacionUsuario(DataEmprendeEntities2 conexion, aplicacion_encuesta aplicacionEncuesta,int sujetoUsuario)
        {
            try
            {
                IList<temporal_encuesta> temporalEncuestaList = conexion.temporal_encuesta.ToList();
                foreach (var item in temporalEncuestaList)
                {
                    conexion.temporal_encuesta.DeleteObject(item);                    
                }
                if(temporalEncuestaList.Count>0)
                    conexion.SaveChanges();



                IList<aplicacion_encuesta> aplicacionList = new List<aplicacion_encuesta>();
                aplicacionList.Add(aplicacionEncuesta);
                IList<tematica> tematicaList;
                IList<preguntas> preguntaList;
                IList<incisos> incisoList;
                IList<resultados_encuesta> resultadosList;
                IList<detalle_catalogo> detallesList;


                Dictionary<tematica, ValoresManager> tematicasValores = new Dictionary<tematica, ValoresManager>();
                Dictionary<preguntas, ValoresManager> preguntasValores = new Dictionary<preguntas, ValoresManager>();
                Dictionary<incisos, ValoresManager> incisosValores = new Dictionary<incisos, ValoresManager>();

                foreach (var aplicacion in aplicacionList)
                {
                    tematicaList = aplicacion.encuesta.tematica.ToList();
                    foreach (var tematica in tematicaList)
                    {
                        preguntaList = tematica.preguntas.ToList();
                        ValoresManager tematicaValor = new ValoresManager();
                        foreach (var pregunta in preguntaList)
                        {
                            incisoList = pregunta.incisos.ToList();
                            ValoresManager preguntaValor = new ValoresManager();
                            foreach (var inciso in incisoList)
                            {
                                ValoresManager incisoValor = new ValoresManager();
                                int total = 0;
                                double average = 0.0;
                                int maximo = int.MinValue;

                                if (inciso.tipo_inciso.tipo_inciso1 != "Escribir")
                                {
                                    total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Count();
                                    //double averageAux = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
                                    var auxiliarResultados = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso != 0);
                                    if (auxiliarResultados.Count() > 0)
                                    {
                                        average = auxiliarResultados.Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
                                    }
                                    else
                                    {
                                        average = 0;
                                    }
                                    //average = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso != 0).Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
                                    maximo = inciso.catalogo.detalle_catalogo.Max(item => item.peso);
                                    //maximo = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Max(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);                                    
                                }
                                incisoValor.Add(average, total, maximo);
                                preguntaValor.Add(average, total, maximo);
                                tematicaValor.Add(average, total, maximo);
                                incisosValores.Add(inciso, incisoValor);
                            }
                            preguntasValores.Add(pregunta, preguntaValor);
                        }
                        tematicasValores.Add(tematica, tematicaValor);
                    }
                }

                foreach (var aplicacion in aplicacionList)
                {
                    tematicaList = aplicacion.encuesta.tematica.ToList();
                    foreach (var tematica in tematicaList)
                    {
                        preguntaList = tematica.preguntas.ToList();
                        foreach (var pregunta in preguntaList)
                        {
                            incisoList = pregunta.incisos.ToList();
                            foreach (var inciso in incisoList)
                            {
                                
                                if (inciso.tipo_inciso.tipo_inciso1 == "Escribir")
                                {
                                    //resultadosList = aplicacion.resultados_encuesta.Where(item => (item.id_inciso == inciso.id_inciso&&item.id_sujeto==sujetoUsuario) ).ToList();
                                    resultadosList = aplicacion.resultados_encuesta.Where(item => (item.id_inciso == inciso.id_inciso && item.id_sujeto == sujetoUsuario)).ToList();
                                    foreach (var resultado in resultadosList)
                                    {                                        
                                        temporal_encuesta temporal = new temporal_encuesta();
                                        temporal.id = conexion.temporal_encuesta.Count();
                                        temporal.AplicacionDescripcion = aplicacion.descripcion;
                                        temporal.AplicacionId = aplicacion.id;
                                        temporal.AplicacionFecha = aplicacion.fecha;

                                        //temporal.DetalleId
                                        //temporal.DetalleNombre = "";
                                        //temporal.DetalleNumeral
                                        //temporal.DetallePeso


                                        temporal.EncuestaEncuesta = aplicacion.encuesta.encuesta1;
                                        temporal.EncuestaId = aplicacion.encuesta.id;

                                        temporal.IncisoId = inciso.id_inciso;
                                        temporal.IncisoInciso = inciso.inciso.Trim();
                                        temporal.IncisoNumeral = inciso.numeral;

                                        temporal.PreguntaId = pregunta.id_pregunta;
                                        temporal.PreguntaPregunta = pregunta.pregunta;

                                        temporal.ResultadoIdSujeto = resultado.id_sujeto;
                                        temporal.ResultadoResultado = resultado.resultado;
                                        temporal.ResultadoResultadoTexto = resultado.resultado_texto;

                                        temporal.TematicaId = tematica.id;
                                        temporal.TematicaTematica = tematica.tematica1;

                                        temporal.TipoIncisoId = inciso.tipo_inciso.id_tipo_inciso;
                                        temporal.TipoIncisoTipoInciso = inciso.tipo_inciso.tipo_inciso1;

                                        if (aplicacion.curso != null)
                                        {
                                            temporal.CursoId = aplicacion.id_curso;
                                            temporal.CursoDescripcion = aplicacion.curso.descripcion;
                                            if(aplicacion.curso.id_turno!=null )
                                            {
                                                temporal.TurnoId=aplicacion.curso.id_turno;
                                                temporal.TurnoTurno=aplicacion.curso.turnos.turno;
                                            }
                                        }
                                        if (aplicacion.id_profesor != null)
                                        {
                                            temporal.ProfesorId = aplicacion.id_profesor;
                                            profesores profesor=conexion.profesores.Where(item=>item.id_persona==aplicacion.id_profesor).First();
                                            temporal.ProfesorNombreApellidos = Utiles.NombreProfesor(profesor);
                                        }

                                        if (aplicacion.id_centro != null)
                                        {
                                            temporal.CentroId = aplicacion.id_centro;
                                            temporal.CentroNombre = mia.centro;
                                        }
                                        if (resultado.id_persona != null)
                                        {
                                            temporal.AutorId = resultado.id_persona;
                                            persona person=conexion.persona.Where(item=>item.id==resultado.id_persona).First();
                                            temporal.AutorNombreApellidos = Utiles.NombrePersona(person);
                                        }
                                        

                                        conexion.temporal_encuesta.AddObject(temporal);
                                        conexion.SaveChanges();
                                    }
                                }
                                else
                                {
                                    int total = aplicacion.resultados_encuesta.Where(item => (item.id_inciso == inciso.id_inciso&&item.id_sujeto==sujetoUsuario)).Count();
                                    detallesList = inciso.catalogo.detalle_catalogo.ToList();
                                    detallesList.Clear();
                                    var resultado = conexion.resultados_encuesta.Where(res => res.id_sujeto == sujetoUsuario && res.id_inciso == inciso.id_inciso && res.id_aplicacion_encuesta == aplicacion.id).FirstOrDefault();
                                    if (resultado != null && resultado.resultado != null)
                                    {
                                        var detalle = conexion.detalle_catalogo.Where(det => det.id == resultado.resultado).FirstOrDefault();
                                        if (detalle != null)
                                        {
                                            detallesList.Add(detalle);
                                        }
                                    }
                                    foreach (var detalle in detallesList)
                                    {
                                        temporal_encuesta temporal = new temporal_encuesta();
                                        temporal.id = conexion.temporal_encuesta.Count();

                                        temporal.AplicacionDescripcion = aplicacion.descripcion;
                                        temporal.AplicacionId = aplicacion.id;
                                        temporal.AplicacionFecha = aplicacion.fecha;

                                        temporal.DetalleId = detalle.id;
                                        temporal.DetalleNombre = detalle.nombre;
                                        temporal.DetalleNumeral = detalle.numeral;
                                        temporal.DetallePeso = detalle.peso;

                                        temporal.EncuestaEncuesta = aplicacion.encuesta.encuesta1;
                                        temporal.EncuestaId = aplicacion.encuesta.id;

                                        temporal.IncisoId = inciso.id_inciso;
                                        //ValoresManager temporalValor = incisosValores[inciso];
                                    //    temporal.IncisoInciso = inciso.inciso.Trim() + " " + temporalValor.Promedio + " de " + temporalValor.Maximo;
                                        temporal.IncisoInciso = inciso.inciso.Trim();
                                        temporal.IncisoNumeral = inciso.numeral;

                                      //  temporalValor = preguntasValores[pregunta];
                                        temporal.PreguntaId = pregunta.id_pregunta;
                                        //temporal.PreguntaPregunta = pregunta.pregunta + " " + temporalValor.Promedio + " de " + temporalValor.Maximo;
                                        temporal.PreguntaPregunta = pregunta.pregunta;

                                        //temporal.ResultadoIdSujeto = resultado.id_sujeto;
                                        //temporal.ResultadoResultado = resultado.resultado;
                                        int cantidadDetalles = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && item.resultado == detalle.id).Count();
                                        double porCiento = (double)(cantidadDetalles * 100) / ((double)total);
                                        //temporal.ResultadoResultadoTexto = "(Evaluado como " + cantidadDetalles + " de " + total + " para un " + Redondeo(porCiento, 1) + "%)";
                                        temporal.ResultadoResultadoTexto = "";

                                        //todo esto es una solucion temporal para lo de los reportes 
                                        //temporal.ResultadoResultadoTexto = SalvacionReporte(cantidadDetalles, total, Redondeo(porCiento, 1));


                                        //nuevo para los reportes
                                        temporal.Calificacion = cantidadDetalles;
                                        temporal.Maximo = total;
                                        temporal.Porcentaje = double.Parse(Redondeo(porCiento, 1));
                                        //

                                     //   temporalValor = tematicasValores[tematica];
                                        temporal.TematicaId = tematica.id;
                                        //temporal.TematicaTematica = tematica.tematica1 + " " + temporalValor.Promedio + " de " + temporalValor.Maximo;
                                        temporal.TematicaTematica = tematica.tematica1;

                                        temporal.TipoIncisoId = inciso.tipo_inciso.id_tipo_inciso;
                                        temporal.TipoIncisoTipoInciso = inciso.tipo_inciso.tipo_inciso1;

                                        if (aplicacion.curso != null)
                                        {
                                            temporal.CursoId = aplicacion.id_curso;
                                            temporal.CursoDescripcion = aplicacion.curso.descripcion;
                                            if (aplicacion.curso.id_turno != null)
                                            {
                                                temporal.TurnoId = aplicacion.curso.id_turno;
                                                temporal.TurnoTurno = aplicacion.curso.turnos.turno;
                                            }
                                        }
                                        if (aplicacion.id_profesor != null)
                                        {
                                            temporal.ProfesorId = aplicacion.id_profesor;
                                            profesores profesor = conexion.profesores.Where(item => item.id_persona == aplicacion.id_profesor).First();
                                            temporal.ProfesorNombreApellidos = Utiles.NombreProfesor(profesor);
                                        }

                                        if (aplicacion.id_centro != null)
                                        {
                                            temporal.CentroId = aplicacion.id_centro;
                                            temporal.CentroNombre = mia.centro;
                                        }
                                        if (resultado.id_persona != null)
                                        {
                                            temporal.AutorId = resultado.id_persona;
                                            persona person = conexion.persona.Where(item => item.id == resultado.id_persona).First();
                                            temporal.AutorNombreApellidos = Utiles.NombrePersona(person);
                                        }
                                        

                                        conexion.temporal_encuesta.AddObject(temporal);
                                        conexion.SaveChanges();
                                    }
                                }

                            }
                        }
                    }
                }
                // conexion.SaveChanges();
            }
            catch (Exception)
            { }
        }

        
        private static void LLenarDatosReporteEncuesta(DataEmprendeEntities2 conexion, IList<aplicacion_encuesta> aplicaciones)
        {
            try
            {
                IList<temporal_encuesta> temporalEncuestaList = conexion.temporal_encuesta.ToList();
                foreach (var item in temporalEncuestaList)
                {
                    conexion.temporal_encuesta.DeleteObject(item);
                }
                conexion.SaveChanges();

                //IList<aplicacion_encuesta> aplicacionList = encuestaAReporte.aplicacion_encuesta.ToList();
                IList<aplicacion_encuesta> aplicacionList = aplicaciones;
                IList<tematica> tematicaList;
                IList<preguntas> preguntaList;
                IList<incisos> incisoList;
                IList<resultados_encuesta> resultadosList;
                IList<detalle_catalogo> detallesList;

                Queue<object> coleccionesValores = new Queue<object>();
                
                foreach (var aplicacion in aplicacionList)
                {
                    Dictionary<tematica, ValoresManager> tematicasValores = new Dictionary<tematica, ValoresManager>();
                    Dictionary<preguntas, ValoresManager> preguntasValores = new Dictionary<preguntas, ValoresManager>();
                    Dictionary<incisos, ValoresManager> incisosValores = new Dictionary<incisos, ValoresManager>();

                    tematicaList = aplicacion.encuesta.tematica.ToList();                    
                    foreach (var tematica in tematicaList)
                    {
                        preguntaList = tematica.preguntas.ToList();
                        ValoresManager tematicaValor = new ValoresManager();
                        foreach (var pregunta in preguntaList)
                        {
                            incisoList = pregunta.incisos.ToList();
                            ValoresManager preguntaValor = new ValoresManager();
                            foreach (var inciso in incisoList)
                            {
                                ValoresManager incisoValor = new ValoresManager();
                                int total = 0;
                                double average=0.0;
                                int maximo = int.MinValue;

                                if (inciso.tipo_inciso.tipo_inciso1 != "Escribir")
                                {
                                    total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Count();
                                    //total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso&&conexion.detalle_catalogo.Where(det=>det.id==item.resultado).First().peso!=0).Count();
                                    var auxiliarResultados = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso != 0);
                                    if (auxiliarResultados.Count() > 0)
                                    {
                                        average = auxiliarResultados.Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
                                    }
                                    else
                                    {
                                        average = 0;
                                    }
                                    //double averageAux = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Average(item=>conexion.detalle_catalogo.Where(det=>det.id==item.resultado).First().peso);
                                    //if (average != averageAux)
                                    //{
                                    //    averageAux = 10;
                                    //}
                                    //average = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso != 0).Average(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);
                                    maximo = inciso.catalogo.detalle_catalogo.Max(item => item.peso);
                                    //maximo = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Max(item => conexion.detalle_catalogo.Where(det => det.id == item.resultado).First().peso);                                    
                                }
                                incisoValor.Add(average, total, maximo);
                                preguntaValor.Add(average, total, maximo);
                                tematicaValor.Add(average, total, maximo);
                                incisosValores.Add(inciso, incisoValor);
                            }
                            preguntasValores.Add(pregunta, preguntaValor);
                        }
                        tematicasValores.Add(tematica, tematicaValor);
                    }
                    //coleccionesValores.Add(incisosValores);
                    //coleccionesValores.Add(preguntasValores);
                    //coleccionesValores.Add(tematicasValores);
                    coleccionesValores.Enqueue(incisosValores);
                    coleccionesValores.Enqueue(preguntasValores);
                    coleccionesValores.Enqueue(tematicasValores);
                }

                foreach (var aplicacion in aplicacionList)
                {
                    Dictionary<incisos, ValoresManager> incisosValores = (Dictionary<incisos, ValoresManager>)coleccionesValores.Dequeue();
                    Dictionary<preguntas, ValoresManager> preguntasValores = (Dictionary<preguntas, ValoresManager>)coleccionesValores.Dequeue();
                    Dictionary<tematica, ValoresManager> tematicasValores = (Dictionary<tematica, ValoresManager>)coleccionesValores.Dequeue();
                    

                    tematicaList = aplicacion.encuesta.tematica.ToList();
                    //double tematicaPromedio=conexion.resultados_encuesta.Where(item=> conexion.incisos.Where(inciso=>inciso.id_inciso==item.id_inciso).First().preguntas.id_tematica==
                    //double tematicaPromedio=conexion.resultados_encuesta.Sum(item=>item.pe
                    foreach (var tematica in tematicaList)
                    {
                        preguntaList = tematica.preguntas.ToList();
                        foreach (var pregunta in preguntaList)
                        {
                            incisoList = pregunta.incisos.ToList();
                            foreach (var inciso in incisoList)
                            {
                                if (inciso.tipo_inciso.tipo_inciso1 == "Escribir")
                                {
                                    resultadosList = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).ToList();
                                    foreach (var resultado in resultadosList)
                                    {
                                        temporal_encuesta temporal = new temporal_encuesta();
                                        temporal.id = conexion.temporal_encuesta.Count();
                                        temporal.AplicacionDescripcion = aplicacion.descripcion;
                                        temporal.AplicacionId = aplicacion.id;
                                        temporal.AplicacionFecha = aplicacion.fecha;

                                        //temporal.DetalleId
                                        //temporal.DetalleNombre = "";
                                        //temporal.DetalleNumeral
                                        //temporal.DetallePeso


                                        temporal.EncuestaEncuesta = aplicacion.encuesta.encuesta1;
                                        temporal.EncuestaId = aplicacion.encuesta.id;

                                        temporal.IncisoId = inciso.id_inciso;
                                        temporal.IncisoInciso = inciso.inciso.Trim();
                                        temporal.IncisoNumeral = inciso.numeral;

                                        temporal.PreguntaId = pregunta.id_pregunta;
                                        temporal.PreguntaPregunta = pregunta.pregunta;

                                        temporal.ResultadoIdSujeto = resultado.id_sujeto;
                                        temporal.ResultadoResultado = resultado.resultado;
                                        temporal.ResultadoResultadoTexto = resultado.resultado_texto;

                                        temporal.TematicaId = tematica.id;
                                        temporal.TematicaTematica = tematica.tematica1;

                                        temporal.TipoIncisoId = inciso.tipo_inciso.id_tipo_inciso;
                                        temporal.TipoIncisoTipoInciso = inciso.tipo_inciso.tipo_inciso1;

                                        conexion.temporal_encuesta.AddObject(temporal);
                                        conexion.SaveChanges();
                                    }
                                }
                                else
                                {
                                    int total = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso).Count();
                                    detallesList = inciso.catalogo.detalle_catalogo.ToList();
                                    foreach (var detalle in detallesList)
                                    {
                                        temporal_encuesta temporal = new temporal_encuesta();
                                        temporal.id = conexion.temporal_encuesta.Count();

                                        temporal.AplicacionDescripcion = aplicacion.descripcion;
                                        temporal.AplicacionId = aplicacion.id;
                                        temporal.AplicacionFecha = aplicacion.fecha;

                                        temporal.DetalleId = detalle.id;
                                        temporal.DetalleNombre = detalle.nombre;
                                        temporal.DetalleNumeral = detalle.numeral;
                                        temporal.DetallePeso = detalle.peso;

                                        temporal.EncuestaEncuesta = aplicacion.encuesta.encuesta1;
                                        temporal.EncuestaId = aplicacion.encuesta.id;

                                        temporal.IncisoId = inciso.id_inciso;
                                        ValoresManager temporalValor = incisosValores[inciso];
                                        temporal.IncisoInciso = inciso.inciso.Trim()+" "+temporalValor.Promedio+" de "+temporalValor.Maximo;
                                        temporal.IncisoInciso = inciso.inciso.Trim();

                                        temporal.Auxiliar1 = "Calificación según peso: " + temporalValor.Promedio + " de un máximo de " + temporalValor.Maximo;
                                        temporal.IncisoNumeral = inciso.numeral;

                                        temporalValor = preguntasValores[pregunta];
                                        temporal.PreguntaId = pregunta.id_pregunta;
                                        temporal.PreguntaPregunta = pregunta.pregunta + " " + temporalValor.Promedio + " de " + temporalValor.Maximo; 

                                        //temporal.ResultadoIdSujeto = resultado.id_sujeto;
                                        //temporal.ResultadoResultado = resultado.resultado;
                                        int cantidadDetalles = aplicacion.resultados_encuesta.Where(item => item.id_inciso == inciso.id_inciso && item.resultado == detalle.id).Count();
                                        double porCiento = (double)(cantidadDetalles * 100) / ((double)total);
                                        //temporal.ResultadoResultadoTexto = "(Evaluado como "+cantidadDetalles + " de " + total + " para un " + Redondeo(porCiento,1) + " %)";

                                        //todo esto es una solucion temporal para lo de los reportes 
                                        //temporal.ResultadoResultadoTexto = SalvacionReporte(cantidadDetalles, total, Redondeo(porCiento, 1));
                                        temporal.ResultadoResultadoTexto = "";

                                        temporal.Calificacion = cantidadDetalles;
                                        temporal.Maximo = total;
                                        temporal.Porcentaje = double.Parse( Redondeo(porCiento, 1));

                                        temporalValor = tematicasValores[tematica];
                                        temporal.TematicaId = tematica.id;
                                        temporal.TematicaTematica = tematica.tematica1 + " " + temporalValor.Promedio + " de " + temporalValor.Maximo;

                                        temporal.TipoIncisoId = inciso.tipo_inciso.id_tipo_inciso;
                                        temporal.TipoIncisoTipoInciso = inciso.tipo_inciso.tipo_inciso1;

                                        conexion.temporal_encuesta.AddObject(temporal);
                                        conexion.SaveChanges();
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }


        private static string Redondeo(double valor, int lugares)
        {
            if (lugares < 0)
                return valor.ToString();
            string cadenaValor=valor.ToString();
            int index = cadenaValor.IndexOf('.');
            if (index >= 0)
            {
                cadenaValor = cadenaValor.Substring(0, index + lugares+1);
            }
            return cadenaValor;
        }

        #region Salvacion Reporte
        /*
        private static string SalvacionReporte(int cantidadDetalles, int total, string porCiento)
        {
            return SalvacionReporte(cantidadDetalles, total, porCiento, 22, 110);
        }

        private static string SalvacionReporte(int cantidadDetalles, int total, string porCiento, int cantidadEspacios, int espacioInicial)
        {
            string result = "";
            result +=new string(' ', espacioInicial);
            result += cantidadDetalles;
            int aux = cantidadEspacios - cantidadDetalles.ToString().Length;
            if (aux > 0)
            {
                result += new string(' ', aux);
            }
            else
            {
                result += " ";
            }
            result += total;
            aux = cantidadEspacios - total.ToString().Length;
            if (aux > 0)
            {
                result += new string(' ', aux);
            }
            else
            {
                result += " ";
            }
            result += porCiento;
            return result;

        }
*/

        #endregion


        public static void MostrarReporte()
        {
            
            frmTipoReporte formulario = new frmTipoReporte();
            formulario.ShowDialog();
            if (formulario.Ok)
            {
                XtraReport reporte;
                switch (formulario.ReporteMostrar)
                {
                    case frmTipoReporte.TipoReporte.Promediado:
                        reporte = new XtraReportPromediado();
                        reporte.Name = "Reporte de Cuba Emprende Promediado";
                        //reporte = new XtraReportProbando();
                        //reporte.Name = "Reporte de Cuba Emprende de Prueba";
                        break;
                    case frmTipoReporte.TipoReporte.ElementoCatalogo:
                        reporte = new XtraReportElementoCatalogo();
                        reporte.Name = "Reporte de Cuba Emprende por Elemento de Catálogo";
                        break;
                    default:
                        return;
                        break;
                }
                reporte.CreateDocument();
                Form_reportes Form_reportes1 = new Form_reportes();
                Form_reportes1.printBarManager1.PrintControl.PrintingSystem = reporte.PrintingSystem;
                Form_reportes1.ShowDialog();

            }
        }

        #endregion 


    }

    public class ValoresManager
    {
        double sumaTotal;
        int maximo=int.MinValue;
        int cantidad;

        public void Add(int peso)
        {
            cantidad++;
            sumaTotal = sumaTotal + peso;
            maximo = Math.Max(maximo, peso);
        }

        public void Add(double average, int cantidad, int maximo)
        {
            this.cantidad += cantidad;
            sumaTotal += average * cantidad;
            this.maximo = Math.Max(this.maximo, maximo);
            
        }

        public string Promedio
        {
            get
            {
                string result=(sumaTotal / cantidad).ToString();
                int index = result.IndexOf('.');
                if (index >= 0)
                {
                    result = result.Substring(0, index + 2);
                }

                return result;
            }
        }

        public int Maximo
        {
            get
            {
                return maximo;
            }
        }
    }
}
