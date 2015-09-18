using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Data.SqlClient;


 
namespace sistema
{
     class utiles
    {
        public static string quitacomillas(string a)
        {
            string b;

            b = a.Replace('"', ' ');
            b = b.Replace('“', ' ');
            b = b.Replace('”', ' ');

            return b;
        }

        public bool contiene(string str1, string str2)
        {
            bool esta = false;
            esta = str1.Contains(str2);

            return esta;
        }
    }
 
    public static class mia
    {
        public static Boolean fecha_ok;
        public static Boolean email_ok;
        public static int id_centro;
        public static Boolean cerrar = true;
        public static int nivel_acceso;
        public static string nivel_acceso_texto = "";
        public static string email_usuario;
        public static string id_profesor = "";
        public static string nombre = "";
        public static string camino = "";
        public static string cadena ="";
        public static string cadena_conexion = "";
        public static string usuariocorreo;
        public static string clavecorreo;
        public static string servidorcorreo;
        public static string probando_si_puedo_enviar="No";
        // del squedul
        public static int  id_squedul;
        public static string ids_participan;
        public static string local_a_usar;




        public static string centro = "";

        public static string mensaje_error = "Introduzca un valor para este campo, si ha cometido un error al insertar el elemento, Por favor elimínelo"; 
        public static string mensaje_error_superior = "Campo vacio o posible error cometido.";
        public static string mensaje_eliminando = "¿Esta seguro que desea eliminar este registro? (Esto puede provocar un BORRADO en CADENA de VARIOS REGISTROS)";
        public static string mensaje_eliminando_superior = "Confirmación de Borrado";

   
       
        public static void mensaje_outlook(string para, string asunto, string cuerpo)
        {
            try
            {
            Outlook.Application oApp = new Outlook.Application();
            Outlook.MailItem mailItem = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            Outlook.Inspector oInspector = mailItem.GetInspector;
            mailItem.Subject = asunto;
            try
            {
                mailItem.To = para;
            }
            catch { mailItem.To = mia.email_usuario; }
            mailItem.Body = cuerpo;
           //mailItem.Display(false);
            mailItem.Send();
            
            MessageBox.Show("Correo electrónico depositado en la Bandeja de Salida de su Microsoft Outlook.", "Envio de Correo", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
            catch (Exception exp)
            {
                   MessageBox.Show(exp.Message); 
            }
        }
         

        public static void sql(string comando)
        {
            SqlConnection connect;
            string con = mia.cadena_conexion;
            connect = new SqlConnection(con);
            connect.Open();
            SqlCommand command;
            command = new SqlCommand(comando, connect);
            command.ExecuteNonQuery();
            connect.Close();
        }

        public static void confecciona_mensaje(string de, string para, string asunto, string cuerpo)
        {
                try
                {
                        MailMessage email = new MailMessage();
                        email.To.Add(new MailAddress(para));
                        email.From = new MailAddress(de);

                        email.Subject = asunto;
                        email.Body = cuerpo;
                        email.IsBodyHtml = false;
                        email.Priority = MailPriority.Normal;

                        SmtpClient smtp = new SmtpClient();
                      
                            smtp.Host = mia.servidorcorreo;
                            smtp.Port = 25;
                            smtp.EnableSsl = false;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(mia.usuariocorreo, mia.clavecorreo);
                            smtp.Send(email);
                            email.Dispose();
                            string output = null;
                            output = "Correo electrónico fue enviado satisfactoriamente por via (SMTP) de forma Directa.";
                            MessageBox.Show(output, "Envio de Correo", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                }
                catch 
                {
                    
                      if (mia.probando_si_puedo_enviar=="No")
                    MessageBox.Show("No se pudo enviar de forma directa (vía SMTP) su mensaje. \r\n \r\n Causas: \r\n"+
                     "   No tiene declarado servidor de Correo.\r\n"+
                     "   No existe un Origen de correo. \r\n"+
                     "   No existe un Destino de correo. \r\n \r\n"+
                   "Usted puede configurar estos Datos en Ventana de Caracterización de la Entidad. \r\n"+
                   "Se tratará de enviar por Microsoft Outlook. ","Error al enviar correo por via SMTP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mia.probando_si_puedo_enviar = "Si";
                    try
                      {
                           mia.mensaje_outlook(para, asunto, cuerpo);
                      }
                      catch (Exception exp)
                       { MessageBox.Show("Usted NO tiene configurado su Microsoft Outlook con alguna cuenta de correo.\r\n Error:   " + exp.Message, "Error al colocar correo en la Bandeja de Salida", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    
                }
        }


        public static void poner_combobox(BindingSource bs, DataGridView dgv, string celda)
        {
            DataRowView view = (DataRowView)bs.Current;
            dgv.Rows[dgv.Rows.Count - 1].Cells[celda].Value = view["id"].ToString();
            dgv.Focus();
        }
        public static void inserta_persona(BindingSource persona, BindingSource bs, DataGridView dgv, int max, string nombre, string id, string id_detalle)
            // el Bs de persona, el bs del elemento a tratar, el dgv del elemento a tratar
            // el maximo del id de el elemento a tratar
            // el elemnto donde quiro pararme al adicionar un elemento(nombre)
            // el nombre del id que tiene el dgv a tratar(id)
            // el nombre q tiene id id perosna en la tabla especifica
            
        {

            if (dgv.Rows.Count > 0) dgv.CurrentCell = dgv.CurrentRow.Cells[nombre];
            
            
            if (max >= 10000)  max = max + 1; 
            else  max = Convert.ToInt32(Convert.ToString(mia.id_centro) + "0001"); 
            
            bs.AddNew();
            dgv.Rows[dgv.Rows.Count - 1].Cells[id].Value = max;
            dgv.CurrentCell = dgv.CurrentRow.Cells[nombre];

            DataRowView view = (DataRowView)persona.Current;
            dgv.Rows[dgv.Rows.Count - 1].Cells[id_detalle].Value = view["id"].ToString();
            dgv.Focus();

        }

        public static void inserta(BindingSource bs, DataGridView dgv, ToolStripButton bn, int max, string nombre)
        {
            if (dgv.Rows.Count > 0)
            {
                bn.Enabled = true;
                dgv.CurrentCell = dgv.CurrentRow.Cells[nombre];
            }

            if (max >= 10000)  max = max + 1; 
            else  max = Convert.ToInt32(Convert.ToString(mia.id_centro) + "0001"); 


            bs.AddNew();
            dgv.Rows[dgv.Rows.Count - 1].Cells["id"].Value = max;
            dgv.CurrentCell = dgv.CurrentRow.Cells[nombre];
        }

        public static void inserta_asesoria(BindingSource bs, DataGridView dgv, ToolStripButton bn, int max, string nombre, string id)
        {
            if (dgv.Rows.Count > 0)
            {
                bn.Enabled = true;
                dgv.CurrentCell = dgv.CurrentRow.Cells[nombre];
            }

            if (max >= 10000) max = max + 1;
            else max = Convert.ToInt32(Convert.ToString(mia.id_centro) + "0001");


            bs.AddNew();
            dgv.Rows[dgv.Rows.Count - 1].Cells[id].Value = max;
            dgv.CurrentCell = dgv.CurrentRow.Cells[nombre];
        }

        public static void elimina(DataGridView dgv, ToolStripButton bn)
        {
            if (dgv.Rows.Count > 0)
            {
                DialogResult Resp;
                Resp = MessageBox.Show(mia.mensaje_eliminando, mia.mensaje_eliminando_superior, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (Resp == DialogResult.Yes)
                {

                    if ((Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value.ToString()) > 10000) || (nivel_acceso == 1)) 
                    {
                        if (Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value.ToString()) < 10000)
                        {
                            string texto = "Usted está eliminando un REGISTRO PRIMARIO. Esto puede afectar el funcionamiento del sistema. ¿Está Seguro?";
                            Resp = MessageBox.Show(texto, mia.mensaje_eliminando_superior, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (Resp == DialogResult.Yes) dgv.Rows.RemoveAt(dgv.CurrentRow.Index);
                        }
                        if (Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value.ToString()) >= 10000)
                        {
                            dgv.Rows.RemoveAt(dgv.CurrentRow.Index);
                        }
                        
                    }
                    else MessageBox.Show("Este registro es de tipo primario y NO puede ser borrado, solamente lo puede borrar un Super Administrador", "Error al borrar un registro", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    
                }
            }
       
        }

        public static void elimina_persona(DataGridView dgv, ToolStripButton bn, string id)
        {
            if (dgv.Rows.Count > 0)
            {
                DialogResult Resp;
                Resp = MessageBox.Show(mia.mensaje_eliminando, mia.mensaje_eliminando_superior, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (Resp == DialogResult.Yes)
                {
                    if ((Convert.ToInt32(dgv.CurrentRow.Cells[id].Value.ToString()) > 10000) || (nivel_acceso ==1))
                        dgv.Rows.RemoveAt(dgv.CurrentRow.Index);
                    else MessageBox.Show("Este registro es de tipo primario y NO puede ser borrado, solamente lo puede borrar un Super Administrador", "Error al borrar un registro", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }

        }
     


        public static Boolean valida_vacios(DataGridView dgv, String texto)
        {
            if (dgv.Rows.Count > 0)
                {
                    DataGridViewCell dgc;
                    dgv.EndEdit();
                    dgc = dgv.Rows[dgv.Rows.Count - 1].Cells[texto];
                    if ((String)dgc.Value.ToString() == string.Empty)
                    {
                        MessageBox.Show("Debe tener un valor en: " + texto + ", por favor verifíquelo", "Error al llenar celdas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else return false;
                }
            else return false;
        }
    
        
        public static void EsFecha(String fecha)
        {
            try
            {
                DateTime.Parse(fecha);
                fecha_ok = true;
            }
            catch
            {
                fecha_ok = false;
            }
        }

        public static void valida_fecha(DataGridViewCellValidatingEventArgs e, DataGridView dgv, int cel)
        {
            if (e.ColumnIndex == cel)
            {
                EsFecha(e.FormattedValue.ToString());
                if (fecha_ok == false)
                {
                    MessageBox.Show("El dato introducido no es de tipo fecha, debe ser con este formato 13/05/2012", "Error de validación",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgv.Rows[e.RowIndex].ErrorText = "El dato introducido no es de tipo fecha, debe ser con este formato 13/05/2012";
                    e.Cancel = true;
                }
            }    
        }



        public static void numero(KeyPressEventArgs e)
        {

            Boolean nonNumberEntered;
            nonNumberEntered = true;

            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 45 || e.KeyChar == 59 || e.KeyChar == 8 || e.KeyChar == 44 || e.KeyChar == 46 || e.KeyChar == 47)
            {
                nonNumberEntered = false;
            }

            if (nonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }

            /*        if (Char.IsDigit(e.KeyChar))           e.Handled = false;
                    else if (Char.IsControl(e.KeyChar))    e.Handled = false;
                    else if (Char.IsSeparator(e.KeyChar))  e.Handled = false;
                    else                                   e.Handled = true;
                    */
        }

        
        public static void valida(DataGridViewCellValidatingEventArgs e, DataGridView dgv, int cel)
        {
            
            if (e.ColumnIndex == cel)
            {
                if (e.FormattedValue.ToString().Length == 0)
                {

                    dgv.Rows[e.RowIndex].ErrorText = mia.mensaje_error;
                    MessageBox.Show(mia.mensaje_error, mia.mensaje_error_superior, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }
                else
                {
                    dgv.Rows[e.RowIndex].ErrorText = string.Empty;
                    e.Cancel = false;
                }

            }
        }

        public static void valida_numero_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e,  int cel)
        {
            if ((int)(((System.Windows.Forms.DataGridView)(sender)).CurrentCell.ColumnIndex) == cel)
            {
                e.Control.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TextboxNumeric_KeyPress);
            }
            else
            { 
                e.Control.KeyPress +=new KeyPressEventHandler(cualquiera);
            }
        }

        public static void cualquiera(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 0 && e.KeyChar <= 127)) { e.Handled = false; }
        }
        
        public static void TextboxNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            Boolean nonNumberEntered;
            nonNumberEntered = true;

            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 8 || e.KeyChar == 44 || e.KeyChar == 46 || e.KeyChar == 47)
            {
                nonNumberEntered = false;
            }

            if (nonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        
        public static void email_bien_escrito(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    email_ok = true;
                }
                else
                {
                    email_ok = false;
                }
            }
            else
            {
                email_ok = false;
            }
        }

        public static void valida_email(DataGridViewCellValidatingEventArgs e, DataGridView dgv, int cel)
        {

            if (dgv.CurrentCell.ColumnIndex == cel)
            {
                email_bien_escrito(e.FormattedValue.ToString());
                if (email_ok == false)
                {
                    MessageBox.Show("La dirección de correo introducida no es válida, por favor veríquelo", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgv.Rows[e.RowIndex].ErrorText = "La dirección de correo introducida no es válida, por favor veríquelo";
                    e.Cancel = true;
                }
            }
        }

    }


    
}
