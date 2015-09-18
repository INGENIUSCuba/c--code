using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OutlookAddInEmprende
{
    public partial class frmListas : Form
    {
        #region VARIABLES GLOBALES
        public int id_centro; //Tiene el id de centro seleccionado en CbxCentros

        #endregion VARIABLES GLOBALES

        #region CONSTRUCTORES
        public frmListas()
        {
            InitializeComponent();
        }
        #endregion CONSTRUCTORES

        #region EVENTOS
        //Oppening del Menú contextual, permisos
        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            //Obtengo el looged
            Properties.Settings setting = new Properties.Settings();
            int logged = Convert.ToInt32(setting["logged"]);

            this.nuevaToolStripMenuItem.Visible = (logged < 3);
            this.editarMiembrosToolStripMenuItem.Visible = (logged < 3);
            this.editarNombreToolStripMenuItem.Visible = (logged < 3);
            this.borrarToolStripMenuItem.Visible = (logged < 3);
        }

        //Acciones al cargar el formulario
        private void frmListas_Load(object sender, EventArgs e)
        {
            //Configuro según logged
            //Leo el looged
            Properties.Settings setting = new Properties.Settings(); //Creo variable para acceder a Settings y leer EmailAdmiin
            int logged = Convert.ToInt32(setting["logged"]);
            
            //pueblo los table adapters
            this.centroTableAdapter.Fill(correosDataSet.centro);
            this.listaTableAdapter.Fill(correosDataSet.lista);

            switch (logged)
            {
                case 1:
                    //Pueblo el CbxCentros
                    string estado = "";
                    for (int i = 0; i < this.correosDataSet.centro.Rows.Count; i++)
                    {
                        estado = this.correosDataSet.centro.Rows[i]["estado"].ToString(); //Evito inclusion de no sincronizados
                        if (estado == "") //Si está sincronizado el estado
                        {
                            this.cbxCentros.Items.Add(this.correosDataSet.centro.Rows[i]["nombre"]);
                        }

                    }
                    if (this.correosDataSet.centro.Rows.Count > 0)
                    {
                        this.cbxCentros.SelectedItem = this.correosDataSet.centro.Rows[0]["nombre"];
                    }

                    break;

                case 2:
                case 3:
                    string elCorreo = setting["Logon_User"].ToString();
                    correosDataSet.CentroListaByLoggedCorreoDataTableDataTable dstDataLogged = this.centroListaByLoggedCorreoDataTableTableAdapter.GetCentroListaByLoggedCorreo(elCorreo);
                    
                        estado = dstDataLogged.Rows[0]["centro_estado"].ToString(); //Evito inclusion de no sincronizados
                        if (estado == "") //Si está sincronizado el estado
                        {
                            this.cbxCentros.Items.Add(dstDataLogged.Rows[0]["nombre_centro"].ToString()); //solo el primer centro que es el unico que tengo en el dst
                        }
                    this.cbxCentros.SelectedItem = dstDataLogged.Rows[0]["nombre_centro"].ToString();

                    //Oculto botones segun logged
                    this.btnBorrar.Visible = (logged < 3);
                    this.btnEditar.Visible = (logged < 3);
                    this.btnNueva.Visible = (logged < 3);

                    break;

            }   
            
            //Terminé de cargar
      }
     
        //Cuando cambia la seleccion en el CbxCentros
        public void cbxCentros_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string item = "";
                item = (cbxCentros.SelectedItem != null) ? cbxCentros.SelectedItem.ToString() : ""; //Tomo la selección si no es nula
                if (item != "") //Me cuido de una mala selección
                {
                    Properties.Settings setting = new Properties.Settings(); //Creo variable para acceder a Settings y leer EmailAdmiin
                    int logged = Convert.ToInt32(setting["logged"]);
                    string elCorreo = setting["Logon_User"].ToString();
                    
                    //Limpio  el grid
                    while (this.dataGridViewListas.Rows.Count > 0)
                    {
                        this.dataGridViewListas.Rows.RemoveAt(0);
                    }

                    //Configuro a código el DatataGridView
                            this.dataGridViewListas.Columns[0].Width = 50;
                            this.dataGridViewListas.Columns[0].HeaderText = "ID";
                            this.dataGridViewListas.Columns[1].Width = 300;
                            this.dataGridViewListas.Columns[1].HeaderText = "Nombre";
                            this.dataGridViewListas.Columns[2].Visible = false;
                            this.dataGridViewListas.Columns[3].Width = 100;
                            this.dataGridViewListas.Columns[3].HeaderText = "Estado";
                            this.dataGridViewListas.Columns[3].Name = "Estado";
                    
                    switch (logged)
                    {
                        case 1: //Admins
                        case 2:
                            id_centro = Convert.ToInt32(this.centroTableAdapter.GetCentroByNombre(item).Rows[0]["id_centro"].ToString()); //Tomo el id de centro
                            //Pueblo el DataGridView

                            if (this.listaTableAdapter.ListasDeUnCentro(id_centro).Count > 0)
                            {
                                correosDataSet.listaDataTable dstLista;
                                dstLista = this.listaTableAdapter.ListasDeUnCentro(id_centro);
                                this.etListas.Text = dstLista.Count.ToString();
                                for (int i = 0; i < dstLista.Count; i++)
                                {
                                    if (dataGridViewListas.Rows.Count <= i) dataGridViewListas.Rows.Add();
                                    dataGridViewListas.Rows[i].Cells["ID"].Value = dstLista[i].id_lista.ToString();
                                    dataGridViewListas.Rows[i].Cells["Nombre"].Value = dstLista[i].nombre.ToString();
                                    dataGridViewListas.Rows[i].Cells["Estado"].Value = (dstLista[i].estado == null) ? "" : dstLista[i].estado.ToString();
                                }
                            }
                            
                            
                            break;

                        case 3:
                            correosDataSet.CentroListaByLoggedCorreoDataTableDataTable dstDataLogged = this.centroListaByLoggedCorreoDataTableTableAdapter.GetCentroListaByLoggedCorreo(elCorreo);
                            this.dataGridViewListas.DataSource = null; //Reseteo el dataSource, voy a llenar el grid a mano
                            if (dstDataLogged.Count > 0)
                            {
                                this.etListas.Text = dstDataLogged.Count.ToString();
                                for (int i = 0; i < dstDataLogged.Count; i++)
                                {
                                    if (dataGridViewListas.Rows.Count <= i) dataGridViewListas.Rows.Add();
                                    dataGridViewListas.Rows[i].Cells["ID"].Value = dstDataLogged[i].id_lista.ToString();
                                    dataGridViewListas.Rows[i].Cells["Nombre"].Value = dstDataLogged[i].nombre_lista.ToString();
                                    dataGridViewListas.Rows[i].Cells["Estado"].Value = (dstDataLogged[i].lista_estado == null) ? "" : dstDataLogged[i].lista_estado.ToString();
                                }
                            }

                            break;
                    }
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //Nueva Lista
        private void btnNueva_Click(object sender, EventArgs e)
        {
            if (this.cbxCentros.SelectedItem != null)
            {
                frmNewLista f = new frmNewLista(this);
                f.ShowDialog();
            }
        }
        
        //Editar Nombre de la lista
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridViewListas.SelectedRows.Count == 1) //Se se marcó una sola lista
            { 
                //Valido sincronización de la lista
                DataGridViewRow fila = dataGridViewListas.SelectedRows[0];
                string estado = fila.Cells["Estado"].Value.ToString(); //tomo el estado de la lista
                if (estado == "")    //si está sincronizada
                {
                    frmEditaLista f = new frmEditaLista(this);
                    f.ctxNombreLista.Text = this.dataGridViewListas.SelectedRows[0].Cells[1].Value.ToString(); //Inicialza el texto con lo que hay
                    f.ShowDialog();
                }
            }
        }

        //Editar los miembros de una lista
        private void btnEditarMiebrosLista_Click(object sender, EventArgs e)
        {
            if (dataGridViewListas.SelectedRows.Count == 1) //Se se marcó una sola lista
            {
                //Valido sincronización de la lista
                DataGridViewRow fila = dataGridViewListas.SelectedRows[0];
                string estado = (fila.Cells["Estado"].Value == null) ? "" : fila.Cells["Estado"].Value.ToString(); //tomo el estado de la lista
                if (estado == "")    //si está sincronizada
                {
                    frmEditaMiembrosLista f = new frmEditaMiembrosLista(this);
                    //Esporto los nombres de la lista y el centro, para sendas etiquetas
                    f.etNombreLista.Text = this.dataGridViewListas.SelectedRows[0].Cells[1].Value.ToString(); //Inicialza el texto con lo que hay
                    f.etCentro.Text = this.cbxCentros.SelectedItem.ToString();
                    f.ShowDialog();
                }
            }
        }

        //Borrar una lista
        private void btnBorrar_Click(object sender, EventArgs e)
        {
            
            if (dataGridViewListas.SelectedRows.Count == 1) //Se se marcó una sola lista
            {
                //Valido sincronización de la lista
                DataGridViewRow fila = dataGridViewListas.SelectedRows[0];
                string estado = fila.Cells["Estado"].Value.ToString(); //tomo el estado de la lista
                if (estado == "")    //si está sincronizada
                {
                    DialogResult decide = MessageBox.Show("Se eliminarán la lista y sus usuarios, ¿está seguro?", "¡Cuidado!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (decide == System.Windows.Forms.DialogResult.Yes)
                    {
                        try
                        {
                            int mi_idlista = Convert.ToInt32(fila.Cells[0].Value); //obtengo el ID de la lista
                            string mi_nombrelista = fila.Cells[1].Value.ToString();
                            this.listaTableAdapter.UpdateUnaLista(mi_nombrelista, "Borrado. No Sinc.", mi_idlista); //Declaro para borrar

                            //Envio el correo
                            Properties.Settings setting = new Properties.Settings(); //Creo variable para acceder a Settings y leer EmailAdmiin
                            string EmailDir = setting["Email_Admin"].ToString();
                            string EmailSubject = "eliminar-lista";
                            string EmailBody = Convert.ToString(mi_idlista) + "|";
                            Globals.ThisAddIn.SendEmailAddressSubjectBody(EmailDir, EmailSubject, EmailBody);

                            //Actualizo Data Grid
                            this.listaTableAdapter.Fill(correosDataSet.lista);
                            this.cbxCentros_SelectedIndexChanged(this, new EventArgs());

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
                
            }
        }

        //Clie en enviar correo a la lista señalada
        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewListas.SelectedRows.Count > 0) //S hay fila señalada
            {
                frmEntraTexto f = new frmEntraTexto(this);
                f.ctxAsunto.Text = "Saludos desde CubaEmprende";
                f.etAsunto.Text = "Asunto:";
                f.Text = "Mensaje a la lista";
                f.ParaQueMeUsan = 2; //Declaro uso en Asunto de Mensaje desde Listas
                f.ShowDialog();
            }
        }

        //Clic Derecho para Menú Contextual
        private void dataGridViewListas_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > -1)
            {
                //Guardo la variable de evento del clic, par usarla desde el tratamiento del clic en los contextMenuStrip items
                DataGridViewCellMouseEventArgs ClicEnDataGrid = e;
                //Quito Selección a las Rows
                foreach (DataGridViewRow dr in this.dataGridViewListas.SelectedRows)
                {
                    dr.Selected = false;
                }
                //Selecciono la Row en la que estoy
                this.dataGridViewListas.Rows[e.RowIndex].Selected = true;
                //Muestro el menúStrip
                this.contextMenuStrip.Show(MousePosition.X + 5, MousePosition.Y + 5);
            }

            if (e.Button == MouseButtons.Left && e.RowIndex > -1)
            {
                foreach (DataGridViewRow dr in this.dataGridViewListas.SelectedRows)
                {
                    dr.Selected = false;
                }
                //Selecciono la Row en la que estoy
                this.dataGridViewListas.Rows[e.RowIndex].Selected = true;
            }
        }

        #region Eventos de Clic derecho en el menú contextual
        //Enviar
        private void enviarCorreoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnEnviarCorreo_Click(this, new EventArgs());
        }

        //Nueva Lista
        private void nuevaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnNueva_Click(this, new EventArgs());
        }

        //Editar Miembros Lista
        private void editarMiembrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnEditarMiebrosLista_Click(this, new EventArgs());
        }

        //Edita Nombre
        private void editarNombreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnEditar_Click(this, new EventArgs());
        }

        //Borrar
        private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.btnBorrar_Click(this, new EventArgs());
        }

        #endregion Eventos de clic derecho

        //Cerrar
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewListas_DoubleClick(object sender, EventArgs e)
        {
            this.dataGridViewListas.CurrentRow.Selected = true;
            this.btnEditarMiebrosLista_Click(this, new EventArgs());

            //Rows[e.RowIndex].Selected = !this.dataGridViewMiembrosLista.Rows[e.RowIndex].Selected;
        }

        private void declararSincronToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewListas.SelectedRows.Count == 1) //Se se marcó una sola lista
            {
                //Valido sincronización de la lista
                DataGridViewRow fila = dataGridViewListas.SelectedRows[0];
                string estado = fila.Cells["Estado"].Value.ToString(); //tomo el estado de la lista
                if (estado != "")    //si está sincronizada
                {
                    
                    int elid = Convert.ToInt32(this.dataGridViewListas.SelectedRows[0].Cells["ID"].Value); //Tengo el id
                    correosDataSet.listaDataTable dstlista = this.listaTableAdapter.GetListaByID(elid);
                    DialogResult result = MessageBox.Show("Borrar el Estado puede crear ambiguedades en el sitema." + "\n" + "Después de borrar el estado: " + "\n" + "Deberá corroborar el estado de los registros en la web." + "\n" + "O bien realizar nuevamente la operación pendiente de sincronización." + "\n" + "\n" + "¿Está segur@ que desea borrar el estado de estos registros?", "¡Cuidado!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); 
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (dstlista.Count > 0)
                         {
                             this.dataGridViewListas.SelectedRows[0].Cells["Estado"].Value = ""; //Limpio el estado
                             this.listaTableAdapter.UpdateUnaLista(dstlista[0].nombre.ToString(), "", elid);//Borro el estado
                         }
                         else
                         {
                             MessageBox.Show("No se encuentra la lista en la Base de datos." + "\n" + "Consulte al soporte del sistema.", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         }
                    }

                }
            }
        }     

        #endregion EVENTOS

        
        



        
        



        



















    }
}
