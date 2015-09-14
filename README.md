# c--code

Here you can find code from Outlook 2010 Add-In that manages an off-line Email Distribution Lists System. A cloud php app receives commands via Email from the add-in, thus the changes are done in the remote database. An Email with the acknowledgment is receive by the add-in and the change is confirmed in the local database.  To send an Email to an email-list, the proper list is chosen in an add-in form, then  an automatic Email is generated so the client can populate its content, attach files, etc. Finally the Email is send to the cloud app, where it is sent to each of the members of the list.
frmEntraTexto.cs: This is a “multipurpose ” form used to enter de subject of an Email that will be send to the cloud app, in order to be sent either an email list, or individual email addresses.
frmListas.cs: This form manages the local distribution lists representation of the add-in, allowing make changes and send Email commands.
ThisAddIn.cs: This is the principal behavior – management library of any OutLook Add-in. Here you can find the principal local data management and remote app confirmation command handling. 

Also from desktop application that manages institutional information stored in a SQL 2008 Server database. The institution has several offices that hasn´t direct connection with the headquarters, so the system also allows data import/export functionality in any direction. The DevExpress (TM) component libraries are used too. Both Entity Framework and Table Adapter data-layers are used.
Form functionality classes: Form_procesos_incubadora.cs, frmEditarEncuesta.cs, Form_schedul.cs, Principal.cs, frmAgregaDetalleCatalogo.cs, Form_respaldoBD_otro.cs.
mia.cs: An application global functionality class.
