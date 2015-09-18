using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;


namespace OutlookAddInEmprende
{
    public partial class ThisAddIn
    {
        //protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject() { return new RibbonEmprende(); }

        #region VARIABLES GLOBALES
            public string Problemas = "Ejecución satisfactoria " + "\n" + "de comandos MailEmprende " + "\n" + "recibidos de la Web.";
            public bool FindComand; //Se usa para saber si se encontro un comando Mail Emprende
            public MailEmprende theRibbon;
            
            //Varables de Acceso a TableAdapters
            correosDataSetTableAdapters.centroTableAdapter taCentrosCl;
            correosDataSetTableAdapters.accesoTableAdapter taAccesoCl;
            correosDataSetTableAdapters.usuariosTableAdapter taUsuarios;
            correosDataSetTableAdapters.listaTableAdapter taListas;
            correosDataSetTableAdapters.usuario_listaTableAdapter taUsuarioListas;
        #endregion VARIABLES GLOBALES

        #region START STOP
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            try
            {
                //Establezco el Hadler para revisar el Email cuando entra
                //this.Application.NewMail += new Microsoft.Office.Interop.Outlook.ApplicationEvents_11_NewMailEventHandler(ThisApplication_NewMail);
                this.Application.NewMail += new Microsoft.Office.Interop.Outlook.ApplicationEvents_11_NewMailEventHandler(ThisApplication_NewMail);

                //Si no existe la carpeta MailEmprende, la creo
                if (!ExistsCurrentFolder("MailEmprende"))
                {
                    CreateCustomFolder("MailEmprende");
                }

                //Cargo los settings
                Properties.Settings setting;
                setting = new Properties.Settings(); //Creo variable para acceder a Settings 
                
                //Establezco la ruta a la db local correos.mdb
                string path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "correos.mdb");
                setting["DBpath"] = path;
                setting.Save();

                //Actualizo el setting de envío desde la BD
                OutlookAddInEmprende.correosDataSetTableAdapters.settingsTableAdapter taSettings = new OutlookAddInEmprende.correosDataSetTableAdapters.settingsTableAdapter();
                setting["Email_Admin"] = taSettings.GetEmailAdmin()[0].Email_Admin.ToString();
                setting.Save();

                //Seteo los valores de login por defecto si existían
                setting["Logon_User"] = setting["Default_User"]; //Declaro usuario por defecto
                setting["logged"] = setting["Default_Logged"]; //Declaro tipo de usuario logueado por defecto 
                setting.Save();
    

            }
            catch 
            {
                MessageBox.Show(ex.Message + " Se intentaba abrir el Componente OutlookAddin Emprende", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #endregion STAR STOP

        #region FUNCIONES GLOBALES
       
        //Valida un dirección de Email *******INCOMPLETA****************
        public string LimpiaMail(string elCorreo)
        {
            string elMail = "";
            Regex rx = new Regex(
        @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
            string[] elements = elCorreo.Split(';'); //Separo las cadenas según ;
            foreach (string element in elements)
            {
                
               // rx.Replace(
               
            }
            return elMail;
        }

        public string GeneraConsecutivo()
        {
            string a = DateTime.Now.Year.ToString() + 
                       DateTime.Now.Month.ToString() + 
                       DateTime.Now.Day.ToString() + 
                       DateTime.Now.Hour.ToString() + 
                       DateTime.Now.Minute.ToString() + 
                       DateTime.Now.Second.ToString();
            Random b = new Random((int)DateTime.Now.Ticks & 0x000FFFFF);
            byte[] values = new byte[6];
            b.NextBytes(values);
            string c = "";
            foreach (var value in values) c += value.ToString();
            a = a + c;
            return (a);
        }

        #endregion FUNCIONES GLOBALES

        #region UTILES DE EMAIL
        //Envia un Email a una adirección con un subject con un body
        public void SendEmailAddressSubjectBody(string addressEmail, string subjectEmail, string bodyEmail)
        {
            this.CreateEmailItem(subjectEmail, addressEmail, bodyEmail);
        }

        //Crea un Email y lo envía pasando la priolidad como parámetro
        public void CreateEmailItemPrio(string subjectEmail, string toEmail, string bodyEmail,string prio)
        {

            try
            {
                Outlook.MailItem eMail = (Outlook.MailItem)this.Application.CreateItem(Outlook.OlItemType.olMailItem);
                eMail.InternetCodepage = 65001; //UTF-8
                eMail.Subject = subjectEmail;
                eMail.To = toEmail;
                eMail.BodyFormat = Outlook.OlBodyFormat.olFormatPlain;
                eMail.Body = "";
                eMail.Body = bodyEmail;
                switch (prio)
                {
                    case "Hight":
                        eMail.Importance = Outlook.OlImportance.olImportanceHigh;
                        break;
                    case "Low":
                        eMail.Importance = Outlook.OlImportance.olImportanceLow;
                        break;
                    case "Normal":
                        eMail.Importance = Outlook.OlImportance.olImportanceNormal;
                        break;
                }
                

                ((Outlook._MailItem)eMail).Send();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Se intentaba mostrar el mensaje de Email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        
        //Crea un Email y lo envía
        private void CreateEmailItem(string subjectEmail, string toEmail, string bodyEmail)
        {

            try
            {
                
                Outlook.MailItem eMail = (Outlook.MailItem)this.Application.CreateItem(Outlook.OlItemType.olMailItem);
                eMail.InternetCodepage = 65001; //UTF-8
                eMail.BodyFormat = Outlook.OlBodyFormat.olFormatPlain;
                eMail.Subject = subjectEmail;
                eMail.To = toEmail;
                eMail.Body = "";
                eMail.Body = bodyEmail;
                eMail.Importance = Outlook.OlImportance.olImportanceHigh;

                ((Outlook._MailItem)eMail).Send();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Se intentaba mostrar el mensaje de Email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Crea un Email y lo Muestra
        public void CreateEmailItemAndDisplay(string subjectEmail, string toEmail, string bodyEmail)
        {

            try
            {
                Outlook.MailItem eMail = (Outlook.MailItem)this.Application.CreateItem(Outlook.OlItemType.olMailItem);
                eMail.InternetCodepage = 65001; //UTF-8
                eMail.Subject = subjectEmail;
                eMail.To = toEmail;
                eMail.BodyFormat = Outlook.OlBodyFormat.olFormatPlain;
                eMail.Body = bodyEmail;
                eMail.Importance = Outlook.OlImportance.olImportanceHigh;
                ((Outlook._MailItem)eMail).Display();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Se intentaba mostrar el mensaje de Email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Crea un Email y lo Salva
        public void CreateEmailItemAndSave(string subjectEmail, string toEmail, string bodyEmail)
        {

            try
            {
                Outlook.MailItem eMail = (Outlook.MailItem)this.Application.CreateItem(Outlook.OlItemType.olMailItem);
                eMail.InternetCodepage = 65001; //UTF-8
                eMail.Subject = subjectEmail;
                eMail.To = toEmail;
                eMail.Body = bodyEmail;
                eMail.Importance = Outlook.OlImportance.olImportanceHigh;
                ((Outlook._MailItem)eMail).Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Se intentaba mostrar el mensaje de Email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion UTILES DE EMAIL

        #region UTILES DE CARPETAS

        //Establece como carpeta por defecto a foldername
        //Con cartel si hay errror
        public void SetCurrentFolder(string folderName)
        {
            Outlook.MAPIFolder inBox = (Outlook.MAPIFolder)
                this.Application.ActiveExplorer().Session.GetDefaultFolder
                (Outlook.OlDefaultFolders.olFolderInbox);
            try
            {
                this.Application.ActiveExplorer().CurrentFolder = inBox.
                    Folders[folderName];
                this.Application.ActiveExplorer().CurrentFolder.Display();
            }
            catch
            {
                MessageBox.Show("No existe la carpeta con el nombre " + folderName +
                    ".", "Carpeta no encontrada");
            }
        }

        //Averigua si existe una carpeta   
        //La deja seteada por defecto
        public bool ExistsCurrentFolder(string folderName)
        {
            Outlook.MAPIFolder inBox = (Outlook.MAPIFolder)
                this.Application.ActiveExplorer().Session.GetDefaultFolder
                (Outlook.OlDefaultFolders.olFolderInbox);
            try
            {
                this.Application.ActiveExplorer().CurrentFolder = inBox.Folders[folderName];
                this.Application.ActiveExplorer().CurrentFolder = inBox; //vuelvo a poner al Inbox por defecto
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Crea una carpeta
        private void CreateCustomFolder(string userName)
        {
            Outlook.MAPIFolder inBox = (Outlook.MAPIFolder)
                this.Application.ActiveExplorer().Session.GetDefaultFolder
                (Outlook.OlDefaultFolders.olFolderInbox);
            Outlook.MAPIFolder customFolder = null;
            try
            {
                customFolder = (Outlook.MAPIFolder)inBox.Folders.Add(userName,
                    Outlook.OlDefaultFolders.olFolderInbox);
                //inBox.Folders[userName].Display();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Se intentaba crear la carpeteta de correo " + userName);
            }
        }





        #endregion UTILES DE CARPETAS

        #region HANDLERS

        //Analza un mail recibido
        public void AnalizaMail(Outlook.MailItem mail)
        {
            int pos;
            string elSubject = mail.Subject.ToString();
            string[] elBody;

            //Sincronización
            if ((pos = Strings.InStr(mail.Subject.ToString(), "Sincronizar:")) > 0)
            {
                //Por ahora no hago nada pues el handler del Evento lo copia para la carpeta MailEprende de todos modos...
                //Si la sincronización es manual no se necesita hacer nada aquí para sincronizar. 19-09-2013
            }

            //Direcciones erróneas
            if ((pos = Strings.InStr(mail.Subject.ToString(), "Direcciones Erroneas")) > 0)
            {
                try
                {
                    elBody = mail.Body.ToString().Trim().Split('|');
                    DateTime fecha = mail.ReceivedTime;
                    correosDataSetTableAdapters.bad_mailTableAdapter taBadMail = new correosDataSetTableAdapters.bad_mailTableAdapter();
                    correosDataSetTableAdapters.usuariosTableAdapter taUsers = new correosDataSetTableAdapters.usuariosTableAdapter();
                    correosDataSetTableAdapters.accesoTableAdapter taAccess = new correosDataSetTableAdapters.accesoTableAdapter();
                    
                    int NumBad = Convert.ToInt32(elBody[0]);
                    int NumBorraBad = 0; 
                    string elBodyBad = ""; string elBodyBadAccess = ""; string unaDir = ""; string unaDir1 = ""; string unaDir2 = "";
                    //Consulto si está activo el borrado automático en los settings...
                    Properties.Settings setting = new Properties.Settings(); //Creo variable para acceder a Settings y leer EmailAdmiin

                    //Declaro borrado y preparo el correo para borrar arriba si está establecido el Check de borrado automático
                    for (int i = 0; i < NumBad; i++)
                    {
                        //Tomo la dirección i-esima
                        unaDir = elBody[i + 1]; //Para chequear los usuarios

                        //La arreglo si está en una tupla de direcciones 01-10-2013 tanto de usuarios como de Accesos 08-05-2014
                        unaDir1 = (taUsers.GetCorreoLikeUnAddress(unaDir).Count > 0)? taUsers.GetCorreoLikeUnAddress(unaDir)[0].correo.ToString() : "";
                        unaDir2 = (taAccess.GetAccesoSegunCorreo(unaDir).Count > 0)? taAccess.GetAccesoSegunCorreo(unaDir)[0].correo.ToString():"";

                        if (unaDir1 != "" || unaDir2 != "") //Si encontró una dirección
                        {
                            if ((bool)setting["Borrado_Auto_BadMail"])//Si está seteado borrar automático las dir erroneas
                            {
                                //Preparo para mandar a borrar direcciones y declaro borrado en USUARIO
                                if (unaDir1 != "") //Si encontró una dirección en usuarios
                                {
                                    taUsers.UpdateEstadoSegunCorreo("Borrado. Dirección rebotada", unaDir);
                                    NumBorraBad++;
                                    elBodyBad += unaDir + "|";
                                }

                                //Preparo para mandar a borrar direcciones y declaro borrado en ACCESOS
                                if (unaDir2 != "") //Si encontró una dirección en usuarios
                                {
                                    setting = new Properties.Settings(); //Creo variable para acceder a Settings y leer EmailAdmiin
                                    string EmailDir = setting["Email_Admin"].ToString();
                                    string EmailSubject = "eliminar-accesos";
                                    taAccess.UpdateEstadoSegunCorreo("Borrado. Dirección rebotada", unaDir);
                                    elBodyBadAccess = taAccess.GetAccesoSegunCorreo(unaDir)[0].id_acceso.ToString() + "|";
                                    Globals.ThisAddIn.SendEmailAddressSubjectBody(EmailDir, EmailSubject, elBodyBadAccess);
                                    //Si hay más de un acceso con la misma dir de Email se borra solo uno, el otro será en el próximo rebote
                                }
                            }

                            //Escribo en la tabla bad_mail para que sea aconsultada cuando se quiera
                            //Desde aquí se podrá mandar a borrar si se quiere. 07-04-2014
                            if (taBadMail.GetByCorreo(unaDir).Count == 0) //Si no está ya el correo
                            {
                                //inserto en la tabla bad_mail si el mail está en la tabla USERS
                                if (taUsers.GetDataByUnCorreo(unaDir1).Count > 0 )
                                {
                                    taBadMail.Insert(unaDir, taUsers.GetDataByUnCorreo(unaDir)[0].nombre.ToString(), taUsers.GetDataByUnCorreo(unaDir)[0].apellido1.ToString(), fecha, 1);
                                }
                                //inserto en la tabla bad_mail si el mail está en la tabla ACCESOS
                                if (taAccess.GetAccesoSegunCorreo(unaDir2).Count > 0)
                                {
                                    try
                                    {
                                        taBadMail.Insert(unaDir, taAccess.GetAccesoSegunCorreo(unaDir)[0].nombre.ToString(), taAccess.GetAccesoSegunCorreo(unaDir)[0].apellido1.ToString(), fecha, 1);
                                    }
                                    catch 
                                    { 
                                        //Se captura la excepción de insersión duplicada de registro
                                        //Pero no se hace nada con ello, no es necesario
                                    }
                                }

                            }
                            else //Si ya está en la tabla BadMail
                            {
                                taBadMail.UpdateNumRebotesSegunCorreo(Convert.ToInt32(taBadMail.GetByCorreo(unaDir)[0]["numrebotes"]) + 1, unaDir); //Sumo uno al número de rebotes
                                taBadMail.UpdateFechaSegunCorreo(fecha, unaDir); //Actualizo la fecha del último rebote
                            }
                        }//Si consiguio la dirección de tupla o simple OK
                    } //For que recorre los NumBad
                    //El Correo
                    
                    if (NumBorraBad > 0 && (bool)setting["Borrado_Auto_BadMail"]) //Si encontré qué borrar y si está activo el borrado automático
                    {
                        setting = new Properties.Settings(); //Creo variable para acceder a Settings y leer EmailAdmiin
                        string EmailDir = setting["Email_Admin"].ToString();
                        //Preparo y envío
                        string EmailSubject = "eliminar-usuario";
                        string BodyBorrar = NumBorraBad.ToString() + "|" + elBodyBad;
                        Globals.ThisAddIn.SendEmailAddressSubjectBody(EmailDir, EmailSubject, BodyBorrar);
                    }
                    /* La actual rutina de borrar accesos es borra uno a uno, así que los Emails se generan arriba
                      if (NumBorraBadAccess > 0 && (bool)setting["Borrado_Auto_BadMail"]) //Si encontré qué borrar y si está activo el borrado automático*/

                }
                catch (Exception ex)
                {
                    if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                    Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                }
            }

            //Respuesta de comandos
            if ((pos = Strings.InStr(mail.Subject.ToString(), "Correcto")) > 0)
            {
                elSubject = mail.Subject.ToString().Split(' ')[1].Trim();
                switch (elSubject)
                {
                    //Limpiar el Estado en Accesos y Centros porque fue correcta la reconstrucción
                    case "rehacer-bd":
                        try
                        {
                            //Preparo variables de acceso a los TableAdapters
                            correosDataSetTableAdapters.centroTableAdapter taCentrosCl = new correosDataSetTableAdapters.centroTableAdapter();
                            correosDataSetTableAdapters.accesoTableAdapter taAccesoCl = new correosDataSetTableAdapters.accesoTableAdapter();
                            correosDataSetTableAdapters.usuariosTableAdapter taUsuarios = new correosDataSetTableAdapters.usuariosTableAdapter();
                            correosDataSetTableAdapters.listaTableAdapter taListas = new correosDataSetTableAdapters.listaTableAdapter();
                            correosDataSetTableAdapters.usuario_listaTableAdapter taUsuarioListas = new correosDataSetTableAdapters.usuario_listaTableAdapter();

                            int ra = Strings.InStr(taCentrosCl.GetData()[0].estado.ToString(), "Rehacer");
                            if (ra > 0)  //Si esta es la BD que mandó a Reahacer
                            {
                                taAccesoCl.LimpiarEstados();
                                taCentrosCl.LimpiarEstados();
                            }
                            else //Si no estoy en la BD que mandó a Rehacer, entonces a serruchar
                            {
                                //Preparo separadores y variables numéricas
                                string[] separators = new string[5];
                                int NumCentros, NumListas, NumAccesos, NumUsuarios, NumUsuarios_listas;
                                string[] BodyCentros, BodyListas, BodyAccesos, BodyUsuarios, BodyUsuarios_listas;
                                separators[0] = "Centros:"; separators[1] = "Listas:"; separators[2] = "Accesos:";
                                separators[3] = "Usuarios:"; separators[4] = "Usuarios_listas:";

                                //Obtengo los trozos del Body del mensaje que tienen los comando de creación
                                elBody = mail.Body.ToString().Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                BodyCentros = elBody[0].Split('|'); BodyListas = elBody[1].Split('|'); BodyAccesos = elBody[2].Split('|');
                                BodyUsuarios = elBody[3].Split('|'); BodyUsuarios_listas = elBody[4].Split('|');
                                NumCentros = Convert.ToInt32(BodyCentros[0]); NumListas = Convert.ToInt32(BodyListas[0]); NumAccesos = Convert.ToInt32(BodyAccesos[0]);
                                NumUsuarios = Convert.ToInt32(BodyUsuarios[0]); NumUsuarios_listas = Convert.ToInt32(BodyUsuarios_listas[0]);

                                //Elimino las tablas Usuarios y Centros, con esto limpio todo
                                taAccesoCl.DeleteAll();
                                taCentrosCl.DeleteAll();
                                taUsuarios.DeleteAll();

                                //Muestro el Splash (Nueva Instancia)
                                OutlookAddInEmprende.frmSplash theSplashR = new frmSplash();
                                theSplashR.pbProgress.Step = 1;
                                theSplashR.pbProgress.Value = 1;
                                theSplashR.pbProgress.Maximum = NumAccesos+NumCentros+NumListas+NumUsuarios+NumUsuarios_listas;
                                theSplashR.etAccion.Text = "Reconstruyendo la Base de Datos Local...";
                                theSplashR.pbProgress.ForeColor = System.Drawing.Color.Brown;
                                theSplashR.Show();
                                System.Windows.Forms.Application.DoEvents();

                                //Ejecuto comandos de creación uno por uno
                                //***************************************************

                                //Crear Centros: id centro|nombre del centro|correo|
                                for (int i = 0; i < NumCentros; i++)
                                {
                                    taCentrosCl.InsertUnCentro(Convert.ToInt32(Strings.Replace(BodyCentros[3 * i + 1], " ", "")), Strings.Replace(BodyCentros[3 * i + 2], " ", ""), Strings.Replace(BodyCentros[3 * i + 3], " ", ""), "");
                                    theSplashR.pbProgress.PerformStep();
                                    System.Windows.Forms.Application.DoEvents();
                                }

                                //Crear Listas: id lista|nombre de la lista|ID del centro|
                                for (int i = 0; i < NumListas; i++)
                                {
                                    taListas.Insert(Convert.ToInt32(Strings.Replace(BodyListas[3 * i + 1], " ", "")), Strings.Replace(BodyListas[3 * i + 2], " ", ""), Convert.ToInt32(Strings.Replace(BodyListas[3 * i + 3], " ", "")), "");
                                    theSplashR.pbProgress.PerformStep();
                                    System.Windows.Forms.Application.DoEvents();
                                }

                                //Crear Accesos: id acceso|correo|clave|nombre|1er apellido|2do apellido|id de la lista|id de tipo de usuario|
                                for (int i = 0; i < NumAccesos; i++)
                                {
                                    taAccesoCl.InsertUnAcceso(Convert.ToInt32(Strings.Replace(BodyAccesos[8 * i + 1], " ", "")), Strings.Replace(BodyAccesos[8 * i + 2], " ", ""), Strings.Replace(BodyAccesos[8 * i + 3], " ", ""), BodyAccesos[8 * i + 4], BodyAccesos[8 * i + 5], BodyAccesos[8 * i + 6], Convert.ToInt32(Strings.Replace(BodyAccesos[8 * i + 7], " ", "")), Convert.ToInt32(Strings.Replace(BodyAccesos[8 * i + 8], " ", "")), "");
                                    theSplashR.pbProgress.PerformStep();
                                    System.Windows.Forms.Application.DoEvents();
                                }

                                //Crear Usuarios
                                for (int i = 0; i < NumUsuarios; i++)
                                {
                                    taUsuarios.InsertUnUsuario(Strings.Replace(BodyUsuarios[4 * i + 1]," ",""), BodyUsuarios[4 * i + 2], BodyUsuarios[4 * i + 3], BodyUsuarios[4 * i + 4], "");
                                    theSplashR.pbProgress.PerformStep();
                                    System.Windows.Forms.Application.DoEvents();
                                }

                                //Crear Usuario-Lista
                                for (int i = 0; i < NumUsuarios_listas ; i++)
                                {
                                    try
                                    {
                                        taUsuarioListas.InsertUsuario_Lista(Convert.ToInt32(Strings.Replace(BodyUsuarios_listas[2 * i + 2]," ","")), Strings.Replace(BodyUsuarios_listas[2 * i + 1]," ",""), "");
                                        theSplashR.pbProgress.PerformStep();
                                        System.Windows.Forms.Application.DoEvents();
                                    }
                                    catch
                                    {
                                        //Evita que explote por un error de no coinsidencia de clave, no se hace nada
                                        //Los usuarios quedarían entonces "sueltos"
                                    }
                                }
                                theSplashR.Close();

                            }
                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": "  + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;
                    case "crear-accesos":
                    case "editar-accesos":
                        //id acceso|correo|clave|nombre|1er apellido|2do apellido|id de la lista|id de tipo de usuario| ** 
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            correosDataSetTableAdapters.accesoTableAdapter taAcceso = new correosDataSetTableAdapters.accesoTableAdapter();

                            if (taAcceso.GetAccesoSegunCorreo(elBody[1]).Count > 0) //Si ya está lo actualizo
                            {
                                taAcceso.UpdateUnAcceso(elBody[1],elBody[2],elBody[3],elBody[4],elBody[5],Convert.ToInt32(elBody[6]), Convert.ToInt32(elBody[7]),"",elBody[1]);
                            }
                            else //Si no lo inserto
                            {
                                taAcceso.InsertUnAcceso(Convert.ToInt32(elBody[0]), elBody[1], elBody[2], elBody[3], elBody[4], elBody[5], Convert.ToInt32(elBody[6]), Convert.ToInt32(elBody[7]), "");
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;

                    case "eliminar-accesos":
                        //Cuerpo: id acceso| **
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            correosDataSetTableAdapters.accesoTableAdapter taAcceso = new correosDataSetTableAdapters.accesoTableAdapter();
                            taAcceso.DeleteUnAccesoByID(Convert.ToInt32(elBody[0]));
                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;

                    case "enviar-correo":
                        string[] sep = new string[1];
                        sep[0] = "ID:";
                        //Extraigo el ID y si no está lo escribo en la tabla
                        string consec = mail.Subject.ToString().Split( sep ,StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                        correosDataSetTableAdapters.consecutivoTableAdapter taConsecutivo = new correosDataSetTableAdapters.consecutivoTableAdapter();
                        if (taConsecutivo.CuantosHay(consec) == 0)
                            {
                                taConsecutivo.InserUnConsecutivo(consec);
                            }

                        break;

                    case "crear-centro":
                    case "editar-centro":
                    //cuerpo: id centro|nombre del centro|correo| **
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            correosDataSetTableAdapters.centroTableAdapter taCentros = new correosDataSetTableAdapters.centroTableAdapter();
                            if (taCentros.GetCentroBySoloID(Convert.ToInt32(elBody[0])).Count > 0) //si ya está el centro
                            {
                                correosDataSetTableAdapters.centroTableAdapter taCentro = new correosDataSetTableAdapters.centroTableAdapter();
                                taCentro.SetUnCentro(elBody[1], "", elBody[2], Convert.ToInt32(elBody[0]));
                            }
                            else //si no esta el centro
                            {
                                taCentros.InsertUnCentro(Convert.ToInt32(elBody[0]), elBody[1], elBody[2], "");
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;

                    case "eliminar-centro":
                        //cuerpo: ID del centro|**
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            correosDataSetTableAdapters.centroTableAdapter taCentro = new correosDataSetTableAdapters.centroTableAdapter();
                            taCentro.DeleteUnCentro(Convert.ToInt32(elBody[0]));
                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;

                    case "crear-lista":
                    case "editar-lista":
                    //cuerpo: id lista|nombre de la lista|ID del centro| **
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            correosDataSetTableAdapters.listaTableAdapter taLista = new correosDataSetTableAdapters.listaTableAdapter();
                            if (taLista.GetListaByID(Convert.ToInt32(elBody[0])).Count > 0) //Si está la lista
                            {
                                taLista.UpdateUnaLista(elBody[1], "", Convert.ToInt32(elBody[0]));
                            }
                            else //Si no está la lista
                            {
                                taLista.Insert(Convert.ToInt32(elBody[0]), elBody[1], Convert.ToInt32(elBody[2]),"");
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;

                    case "eliminar-lista":
                        //Cuerpo: Id lista| **
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            correosDataSetTableAdapters.listaTableAdapter taLista = new correosDataSetTableAdapters.listaTableAdapter();
                            taLista.DeleteUnaLista(Convert.ToInt32(elBody[0]));
                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;

                    case "crear-usuario":
                        //cant_usuarios|correo|nombre|1er apellido|2do apellido| **
                        //ejem: 2|mario@casa.com|mario|gles|mtinez|pepe@casa.com|jose|ramirez|prieto|
                        try
                        {
                            string elCorreo = "";
                            elBody = mail.Body.ToString().Trim().Split('|');
                            taUsuarios = new correosDataSetTableAdapters.usuariosTableAdapter();
                            int Num = Convert.ToInt32(elBody[0]);
                            for (int i = 0; i<Num; i++)
                            {
                                elCorreo = elBody[4*i + 1];
                                if (taUsuarios.GetDataByUnCorreo(elCorreo).Count > 0) //Si está el correo
                                {
                                    taUsuarios.UpdateEstadoSegunCorreo("", elCorreo); //Reseteo su estado
                                }
                                else //Si no está lo inserto
                                {
                                    taUsuarios.InsertUnUsuario(elCorreo, elBody[4 * i + 2], elBody[4 * i + 3], elBody[4 * i + 4], "");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }

                        break;

                    case "editar-usuario":
                    //correo_old|correo_new|nombre|1er apellido|2do apellido| **
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            taUsuarios = new correosDataSetTableAdapters.usuariosTableAdapter();
                            if (taUsuarios.GetDataByUnCorreo(elBody[1]).Count > 0) //Si el correo nuevo está
                            {
                                taUsuarios.UpdateUsuarioSegunCorreo(elBody[2],elBody[3],elBody[4],"",elBody[1]); //Actualizo todo menos el correo
                            }
                            //Si el correo viejo es el que está, y no está el nuevo, actualizo cambiando el correo.
                            if (taUsuarios.GetDataByUnCorreo(elBody[0]).Count > 0 && taUsuarios.GetDataByUnCorreo(elBody[1]).Count ==0) 
                            {
                                taUsuarios.UpdateUsuarioSegunCorreoIncCorreo(elBody[1],elBody[2], elBody[3], elBody[4], "", elBody[0]); //Actualizo todo
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;

                    case "editar-usuario-listas":
                    //cuerpo: cant_listas|correo_old|correo_new|nombre|1er apellido|2do apellido|id_lista|id_lista| --**
                    //ejem: 2|pedro@gmail.com|pedro1@gmail.com|Pedro|Pérez|García|7|8|
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            taUsuarios = new correosDataSetTableAdapters.usuariosTableAdapter();
                            correosDataSetTableAdapters.usuario_listaTableAdapter taUsuarioLista = new correosDataSetTableAdapters.usuario_listaTableAdapter();
                            if(taUsuarios.GetDataByUnCorreo(elBody[1]).Count > 0) //Si el correo_old estaba, lo borro
                                {
                                    taUsuarios.DeleteSegunCorreo(elBody[1]); //Borro el correoOld no importa si no está
                                }
                            if (taUsuarios.GetDataByUnCorreo(elBody[2]).Count > 0) //Si ya esta creado el correo_new
                            {
                                taUsuarios.UpdateUsuarioSegunCorreo(elBody[3],elBody[4],elBody[5],"", elBody[2]); //Actualizo los parámetros del usuaro
                                //Ahora actualizo Usuario Lista, actualizando el Estado en este caso
                                int Num = Convert.ToInt32(elBody[0]); //Tomo el número de listas
                                for (int i = 0; i < Num; i++)
                                {
                                    if (taUsuarioLista.GetDataByListaYMail(Convert.ToInt32(elBody[i + 6]),elBody[2]).Count > 0 ) //Si el usuario-lista ya está creado
                                    {
                                        taUsuarioLista.UpdateEstadoSegunIDListaYCorreo("", Convert.ToInt32(elBody[i + 6]), elBody[2]);
                                    }
                                    else //si el usuario-lista no estaba, lo creo en estado sincronizado
                                    {
                                        taUsuarioLista.InsertUsuario_Lista(Convert.ToInt32(elBody[i + 6]), elBody[2], "");
                                    }
                                }
                            }
                            else //si no estaba creado el correo_new
                            {
                                taUsuarios.InsertUnUsuario(elBody[2], elBody[3], elBody[4], elBody[5], "");
                                //Ahora actualizo Usuario Lista, creando en este caso
                                int Num = Convert.ToInt32(elBody[0]); //Tomo el número de listas
                                for (int i = 0; i < Num; i++)
                                {
                                    taUsuarioLista.InsertUsuario_Lista(Convert.ToInt32(elBody[i + 6]), elBody[2],"");
                                }
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;

                    case "eliminar-usuario":
                        //Cuerpo: cant_usuarios|correo|   y tantos usuarios como cant_usuarios de forma seguida **
                        //ejem: 2|pepe@casa.cu|marios@casa.cu|
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            taUsuarios = new correosDataSetTableAdapters.usuariosTableAdapter();
                            for(int i = 0; i < Convert.ToInt32(elBody[0]); i++)
                            {
                                taUsuarios.DeleteSegunCorreo(elBody[i+1]);
                            }

                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }

                        break;

                    case "crear-usuario-lista":
                    //Cuerpo: cant_usuarios|correo|id de la lista| y tantos usuarios como cant_usuarios de forma seguida    **    
                    //Implica que los usuarios ya están creados en la BD, sólo se asocian con listas
                    //ejem: 2|mario@casa.com|1|pepe@casa.com|1|
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            correosDataSetTableAdapters.usuario_listaTableAdapter taUsuariosLista = new correosDataSetTableAdapters.usuario_listaTableAdapter();
                            for (int i = 0; i < Convert.ToInt32(elBody[0]); i++)
                            {
                                if (taUsuariosLista.GetDataByListaYMail(Convert.ToInt32(elBody[2 * i + 2]),elBody[2 * i + 1]).Count > 0) //Si está el usuario lista actualizo el estado
                                {
                                    taUsuariosLista.UpdateEstadoSegunIDListaYCorreo("", Convert.ToInt32(elBody[2 * i + 2]), elBody[2 * i + 1]);
                                }
                                else //Si no está lo creo
                                {
                                    taUsuariosLista.InsertUsuario_Lista(Convert.ToInt32(elBody[2 * i + 2]), elBody[2 * i + 1], "");
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }


                        break;

                    case "crear-usuario-usuario-lista": 
                        //cuerpo: cant_usuarios|correo|nombre|apellido1|apellido2|id lista| y tantos usuarios como cant_usuarios de forma seguida **
                        //ejem: 2|mario@casa.com|mario|gles|mtinez|2|pepe@casa.com|jose|ramirez|prieto|1|

                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            taUsuarios = new correosDataSetTableAdapters.usuariosTableAdapter();
                            correosDataSetTableAdapters.usuario_listaTableAdapter taUsuariosLista = new correosDataSetTableAdapters.usuario_listaTableAdapter();
                            for (int i = 0; i < Convert.ToInt32(elBody[0]); i++)
                            {
                                //El usuario
                                if (taUsuarios.GetDataByUnCorreo(elBody[5 * i + 1]).Count > 0) //si está el usuario actualizo el estado
                                {
                                    taUsuarios.UpdateEstadoSegunCorreo("", elBody[5 * i + 1]);
                                }
                                else //No está, lo creo
                                {
                                    taUsuarios.InsertUnUsuario(elBody[5 * i + 1], elBody[5 * i + 2], elBody[5 * i + 3], elBody[5 * i + 4], "");
                                }
                                
                                //El usuario-lista
                                if (taUsuariosLista.GetDataByListaYMail(Convert.ToInt32(elBody[5 * i + 5]), elBody[5 * i + 1]).Count > 0) //si está el usuario-lista, actualizo el estado
                                {
                                    taUsuariosLista.UpdateEstadoSegunIDListaYCorreo("", Convert.ToInt32(elBody[5 * i + 5]), elBody[5 * i + 1]);
                                }
                                else //Si no está lo creo
                                {
                                    taUsuariosLista.InsertUsuario_Lista(Convert.ToInt32(elBody[5 * i + 5]), elBody[5 * i + 1], "");
                                }

                            } //For que recorre la lista de comandos del body

                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }

                        break;

                    case "eliminar-usuario-lista":
                        //Cuerpo: cant_usuarios|correo|id de la lista|  y tantos usuarios como cant_usuarios de forma seguida **
                        //ejem: 2|mario@casa.com|1|pepe@casa.com|1|
                        try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            correosDataSetTableAdapters.usuario_listaTableAdapter taUsuariosLista = new correosDataSetTableAdapters.usuario_listaTableAdapter();
                            for (int i = 0; i < Convert.ToInt32(elBody[0]); i++)
                            {
                                taUsuariosLista.DeleteUnUsuarioLista(Convert.ToInt32(elBody[2 * i + 2]), elBody[2 * i + 1]);
                            }

                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;

                        case "mover-usuario-lista":
                        //Cuerpo: cant_usuarios|correo|id de la lista vieja|id lista nueva| y tantos usuarios como cant_usuarios de forma seguida
                        //ejem: 2|mario@casa.com|1|3|pepe@casa.com|1|4|
                            try
                        {
                            elBody = mail.Body.ToString().Trim().Split('|');
                            correosDataSetTableAdapters.usuario_listaTableAdapter taUsuariosLista = new correosDataSetTableAdapters.usuario_listaTableAdapter();
                            for (int i = 0; i < Convert.ToInt32(elBody[0]); i++)
                            {
                                if (taUsuariosLista.EstaEnLista(Convert.ToInt32(elBody[3 * i + 3]), elBody[3 * i + 1]) == 0)
                                {
                                    taUsuariosLista.MuevePertenenciaALista(Convert.ToInt32(elBody[3 * i + 3]), "", Convert.ToInt32(elBody[3 * i + 2]), elBody[3 * i + 1]);
                                }
                                else 
                                {
                                    //Si ya estaba en la otra lista reseteo el estado
                                    taUsuariosLista.UpdateEstadoSegunIDListaYCorreo("", Convert.ToInt32(elBody[3 * i + 3]), elBody[3 * i + 1]);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            if (Strings.InStr(Problemas, "satisfactoria") > 0) Problemas = "";
                            Problemas += elSubject + ": " + mail.Body.ToString() + ". Registró: " + "\n" + ex.Message.ToString();
                        }
                        break;
                }//Switch elSubjec
                
            }
            
        } //Analiza Mail

        public void BuscaMailsDeComandos(int dias)
        {
            try
            {
                Cursor MyCursor = Cursors.WaitCursor;

                FindComand = false;
                Outlook.MailItem mail; //Ahí pongo cada Mail
                Outlook.NameSpace outlookNameSpace = this.Application.GetNamespace("MAPI");
                Outlook.MAPIFolder inbox = outlookNameSpace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);

                Outlook.Items items = inbox.Items.Restrict("[Unread]=true");
                DateTime hacedias = DateTime.Today - new TimeSpan(dias, 0, 0, 0); //Fecha de hace "dias" días

                //Muestro el splash
                OutlookAddInEmprende.frmSplash theSplash = new frmSplash();
                theSplash.Show();
                theSplash.pbProgress.Step = 1;
                theSplash.pbProgress.Value = 1;
                theSplash.pbProgress.Maximum = items.Count;
                System.Windows.Forms.Application.DoEvents();


                //Obtengo la dirección de Email del buzón de comandos
                Properties.Settings setting = new Properties.Settings(); //Creo variable para acceder a Settings y leer EmailAdmiin
                string EmailDir = setting["Email_Admin"].ToString();

                ////Reviso los Unread de la carpeta MailEmprende, y los analizo
                foreach (Object m in items)
                {
                    theSplash.pbProgress.PerformStep();
                    if ((mail = m as Outlook.MailItem) != null) //si el item es un mail
                    {
                        if (mail.MessageClass == "IPM.Note" && mail.SenderEmailAddress.ToUpper().Contains(EmailDir.ToUpper()) && mail.ReceivedTime > hacedias)
                        {
                            FindComand = true;
                            AnalizaMail(mail);
                            mail.UnRead = false;
                            mail.Move(inbox.Folders["MailEmprende"]);
                        }
                    }
                }

                if (FindComand) //Si encontré algo
                {
                    /*  Fue comentada porque ya no tiene sentido al hacerse más robusta la rutina precedente
                     * //09-04-2014
                    //Muevo los mails encontrados repito la rutina para que no quede nada
                    foreach (Object m in items)
                    {
                        if ((mail = m as Outlook.MailItem) != null) //si el item es un mail
                        {
                            if (mail.MessageClass == "IPM.Note" && mail.SenderEmailAddress.ToUpper().Contains(EmailDir.ToUpper()) && mail.ReceivedTime > hacedias)
                            {
                                mail.Move(inbox.Folders["MailEmprende"]);
                            }
                        }
                    }*/

                    //Pongo cartel de resultados
                    MessageBox.Show(Problemas, "¡Resultados!", MessageBoxButtons.OK);
                    Problemas = "Ejecución satisfactoria " + "\n" + "de comandos MailEmprende " + "\n" + "recibidos de la Web."; //Limpio la varible que guardaba los mensajes de error.
                }
                MyCursor = Cursors.Default;

                theSplash.Close(); //Cierro el splash
            }


            catch (Exception ex)
            {
                Cursor MyCursor = Cursors.Default;
                MessageBox.Show(ex.Message + " Se intentaba revisar los mensajes recibidos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        //Chequea el correo nuevo segun un filtro
        public void ThisApplication_NewMail()
        {
            //Cargo los settings
            Properties.Settings setting;
            setting = new Properties.Settings(); //Creo variable para acceder a Settings 
            BuscaMailsDeComandos((int)setting["Dias_Revisar_Corto"]); //Uso Settings de los días de la revisión corta
        }

        #endregion HANDLERS


        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }  
}
