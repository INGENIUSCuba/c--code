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
    public partial class frmEntraTexto : Form
    {
        #region VARIABLES GLOBALES
            public frmEditaMiembrosLista FormEditaMiebrosLista;
            public int ParaQueMeUsan;
            public frmListas FormListas;
            public frmBadMail FormBadMail;
        #endregion VARIABLES GLOBALES

       #region COSTRUCTORES 
        public frmEntraTexto()
        {
            InitializeComponent();
        }

        //Cuando es llamado de frmBadMail
        public frmEntraTexto(frmBadMail f)
        {
            InitializeComponent();
            FormBadMail = f;
        }
        //Cuando es llamado de EditaMiembrosLista
        public frmEntraTexto(frmEditaMiembrosLista f)
        {
            InitializeComponent();
            FormEditaMiebrosLista = f;
        }

        //Cuando es llamado desde Listas
        public frmEntraTexto(frmListas f)
        {
            InitializeComponent();
            FormListas = f;
        }

       #endregion COSTRUCTORES

        #region EVENTOS

        //Clic en Aceptar
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Leyenda
            //1. Se usa para asunto de mensaje y guardarlo en Borrador cuando es llamado desde EditaMiembrosLista
            //2. Se usa para asunto de mensaje y guardarlo en Borrador cuando es llamado desde Listas
            
            //Gestiono segun para que me usan
            if (ctxAsunto.Text != "")
            {
                switch (ParaQueMeUsan)
                {
                    case 1: //Si se usa el form para un asunto de mensaje de Edita Miembros lista
                        string Asunto = ctxAsunto.Text;
                        //Obtengo la dirección de Email del buzón de comandos, si abrió una lista es porque la capa de permisos se lo permitió
                        Properties.Settings setting = new Properties.Settings(); //Creo variable para acceder a Settings y leer EmailAdmiin
                        string EmailDir = setting["Email_Admin"].ToString();
                        //Preparo y salvo el Email

                        //Preparo gestión de consecutivos y fabrico el subject
                        this.consecutivoTableAdapter.Fill(this.correosDataSet.consecutivo);
                        int? count = 0; //Para usar en el nuevo aleatorio
                        string consec = ""; //El consecutivo
                        do
                        {
                            consec = Globals.ThisAddIn.GeneraConsecutivo();
                            count = this.consecutivoTableAdapter.CuantosHay(consec);
                        } while (count > 0);
                        this.consecutivoTableAdapter.InserUnConsecutivo(consec);
                        this.consecutivoTableAdapter.Update(this.correosDataSet);
                        string EmailSubject = "enviar-correo" + "|" + consec + "|";

                        string preVacio = "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "____________________________________________" + "\n";
                        string EmailBody = preVacio + "|" + this.FormEditaMiebrosLista.Mi_IDLista.ToString() + "|" + Asunto + "|\n" + "____________________________________________" + "\n";
                        Globals.ThisAddIn.CreateEmailItemAndSave(EmailSubject, EmailDir, EmailBody);
                        MessageBox.Show("El mensaje ha sido guardado en la carpeta Borrador" + "\n" + "en espera de ser editado y enviado por usted", "¡Información!", MessageBoxButtons.OK);
                        this.Close();
                        break;

                    case 2: //Si se usa el form para un asunto de mensaje de lista
                        Asunto = ctxAsunto.Text;
                        //Tomo el id de lista
                        int LaLista = Convert.ToInt32(this.FormListas.dataGridViewListas.SelectedRows[0].Cells[0].Value); 
                        //Obtengo la dirección de Email del buzón de comandos, si abrió una lista es porque la capa de permisos se lo permitió
                        setting = new Properties.Settings(); //Creo variable para acceder a Settings y leer EmailAdmiin
                        EmailDir = setting["Email_Admin"].ToString();
                        //Preparo y salvo el Email

                        //Preparo gestión de consecutivos y fabrico el subject
                        this.consecutivoTableAdapter.Fill(this.correosDataSet.consecutivo);
                        count = 0; //Para usar en el nuevo aleatorio
                        consec = ""; //El consecutivo
                        do
                        {
                            consec = Globals.ThisAddIn.GeneraConsecutivo();
                            count = this.consecutivoTableAdapter.CuantosHay(consec);
                        } while (count > 0);
                        this.consecutivoTableAdapter.InserUnConsecutivo(consec);
                        this.consecutivoTableAdapter.Update(this.correosDataSet);
                        EmailSubject = "enviar-correo" + "|" + consec + "|";

                        preVacio = "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "____________________________________________" + "\n";
                        EmailBody = preVacio +  "|" + LaLista.ToString() + "|" + Asunto + "|\n" + "____________________________________________" + "\n";
                        Globals.ThisAddIn.CreateEmailItemAndSave(EmailSubject, EmailDir, EmailBody);
                        MessageBox.Show("El mensaje ha sido guardado en la carpeta Borrador" + "\n" + "en espera de ser editado y enviado por usted", "¡Información!", MessageBoxButtons.OK);
                        this.Close();
                        break;
                    case 3: //Si se usa desde BadMail
                        Asunto = ctxAsunto.Text;
                        FormBadMail.EmailSubject = Asunto;
                        this.Close();
                        break;
                }
            }
        }

        //clic en Enter
        private void ctxAsunto_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)  //Si es return
                {
                    this.btnAceptar_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Cancelar
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion EVENTOS

        

        
    }
}
