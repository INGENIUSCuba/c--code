using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;
using FarsiLibrary.Win.Controls;

namespace sistema
{
    public partial class Form_schedul : Form
    {
        string creada;
        public Form_schedul()
        {
            InitializeComponent();
            schedulerControl1.DayView.TopRowTime = new TimeSpan(10, 0, 0);
        }

        private void schedulerControl1_CustomDrawTimeCell(object sender, DevExpress.XtraScheduler.CustomDrawObjectEventArgs e)
        {
            if (e.ObjectInfo is TimeCell)
            {
                TimeCell tc = e.ObjectInfo as TimeCell;
                if (tc.Interval.Start.Hour >= 12 && tc.Interval.Start.Hour < 14)
                {
                    tc.Appearance.BackColor = Color.LightBlue;
                }
            }
        }

        private void Form_squedul_Load(object sender, EventArgs e)
        {
            schedulerControl1.Start = DateTime.Now;
            // TODO: esta línea de código carga datos en la tabla 'dS_DataEmprende.tipo_evento' Puede moverla o quitarla según sea necesario.
            this.tipo_eventoTableAdapter.Fill(this.dS_DataEmprende.tipo_evento);
            schedulerStorage1.Appointments.Labels.Clear();

            int color;
            string tipo_evento;

            MyColour c = new MyColour();

            tipo_eventoBindingSource .MoveFirst();
            for (int i = 0; i < tipo_eventoBindingSource.Count; i++)
            {
                DataRowView view = (DataRowView)tipo_eventoBindingSource.Current;
                color = Convert.ToInt32(view["color"].ToString());
                tipo_evento = view["tipo_evento"].ToString();

                c.Colour = Color.FromKnownColor((KnownColor)color);
                AppointmentLabel lbl = new AppointmentLabel(Color.FromName(c.Colour.Name), tipo_evento,tipo_evento);
                schedulerStorage1.Appointments.Labels.Add(lbl);


                tipo_eventoBindingSource.MoveNext();

            }
            


            
            if (mia.nivel_acceso == 1)
               this.appointmentsTableAdapter.Fill(this.dS_DataEmprende.Appointments);
            if (mia.nivel_acceso == 3)
                this.appointmentsTableAdapter.filtra_tipo_cita(this.dS_DataEmprende.Appointments, mia.id_profesor);
            
            actualiza_apps_grupos();

        }

        private void schedulerControl1_EditAppointmentFormShowing(object sender, DevExpress.XtraScheduler.AppointmentFormEventArgs e)
        {
            DevExpress.XtraScheduler.SchedulerControl scheduler = ((DevExpress.XtraScheduler.SchedulerControl)(sender));
            sistema.CustomAppointmentForm form = new sistema.CustomAppointmentForm(scheduler, e.Appointment, e.OpenRecurrenceForm);
            try
            {
                e.DialogResult = form.ShowDialog();
//                if (!(e.DialogResult == System.Windows.Forms.DialogResult.Cancel || e.DialogResult == System.Windows.Forms.DialogResult.Abort)) schedulerControl1.Refresh();
                e.Handled = true;
            }
            finally
            {
                form.Dispose();
            }

        }

        private void schedulerStorage1_AppointmentsChanged(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e)
        {
            CommitTask();
        }

        private void schedulerStorage1_AppointmentsDeleted(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e)
        {
            CommitTask();
        }

        private void schedulerStorage1_AppointmentsInserted(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e)
        {
            CommitTask();
        //    schedulerStorage1.SetAppointmentId(((Appointment)e.Objects[0]), id);
        }

        void CommitTask()
        {
            appointmentsTableAdapter.Update(dS_DataEmprende);
            this.dS_DataEmprende.AcceptChanges();
        }

        private void schedulerControl1_Click(object sender, EventArgs e)
        {
            try
            {
                Appointment selectedApt;
                if (this.schedulerControl1.SelectedAppointments.Count == 1)
                {
                    selectedApt = this.schedulerControl1.SelectedAppointments[0];
                    DataRowView row = (DataRowView)selectedApt.GetSourceObject(this.schedulerStorage1);
                    mia.id_squedul = Convert.ToInt32(row["UniqueID"].ToString());
                    mia.ids_participan = row["ResourceIDs"].ToString();
                    creada = row["creada"].ToString();

                   
                }
                else
                {
                    mia.id_squedul = 0;
                    mia.ids_participan = "";
                }
            }
            catch { }

        }

      
        private void schedulerStorage1_AppointmentDeleting(object sender, PersistentObjectCancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("¿ Está seguro que desea borrar esta cita ?", "Por favor, Confirme ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                e.Cancel = true;
        }


        //Funcion que actualiza los appointmens a partir de los grupos
        public void actualiza_apps_grupos()
        {
            string cf; Appointment apt, unApp = null; int cantEncuetros; int miIDEvento, IdCentro;
            //OccurrenceCalculator calc;
            try
            {
                DS_DataEmprende.sl_grupo_cursoDataTable dstGruposMaterias = this.sl_grupo_cursoTableAdapter1.GetData(); //Colecto los grupos

                AppointmentCollection theApps = this.schedulerStorage1.Appointments.Items; //Colecto las citas
                //Preparo el splash
                /*
                frmTheSplash theSplash = new frmTheSplash();
                theSplash.pbProgress.Step = 1;
                theSplash.pbProgress.Maximum = dstGruposMaterias.Count;
                theSplash.pbProgress.Minimum = 1;
                theSplash.pbProgress.Value = 1;
                theSplash.Show();
                */
                foreach (DS_DataEmprende.sl_grupo_cursoRow elRow in dstGruposMaterias)
                {
                    cf = "<CitasEspeciales " + "Type = grupos_materias" + " id_grupo = " + elRow.id_grupo + " id_materia = " + elRow.id_materia + " id_centro = " + mia.id_centro.ToString() + ">"; //Identificador en el Custom Field
                    unApp = this.schedulerStorage1.CreateAppointment(AppointmentType.Pattern);
                    unApp = this.schedulerStorage1.Appointments.Items.Find(appointment => ((appointment.CustomFields["CustomField1"] == System.DBNull.Value) ? "" : (string)appointment.CustomFields["CustomField1"]) == cf);
                    if (unApp == null) //no está el App
                    {
                        //Creo el Appointment
                        apt = this.schedulerStorage1.CreateAppointment(AppointmentType.Pattern);

                        apt.Subject = ("Clases al Grupo: " + utiles.quitacomillas(elRow.nombre.ToString()) + " | Asig: " + utiles.quitacomillas(elRow.nombre_materia)).Trim();

                        try
                        {
                            apt.Description = ("Profesor: " + utiles.quitacomillas(elRow.nomb_profesor.ToString()));
                        }
                        catch { apt.Description = "No existe Profesor Asignado todavía !!"; }
                        
                        apt.RecurrenceInfo.AllDay = false;
                        apt.RecurrenceInfo.Periodicity = 1;

                  

                        apt.StatusId = 2;
                        apt.LabelId = 6;

                        apt.RecurrenceInfo.Range = RecurrenceRange.OccurrenceCount;
                        cantEncuetros = Convert.ToInt32(elRow.cant_encuentros);
                        apt.RecurrenceInfo.OccurrenceCount = cantEncuetros; //Un encuentro si no está especiicada la cantidad en el registro del curso

                        int hora_i=8, minutos_i=0,hora_f=12,minutos_f=0;
                        try
                        {
                            DateTime tt = Convert.ToDateTime(elRow.hora_inicio.ToString());
                            hora_i = Convert.ToInt32(tt.Hour.ToString());
                            minutos_i = Convert.ToInt32(tt.Minute.ToString());
                            tt = Convert.ToDateTime(elRow.hora_fin.ToString());
                            hora_f = Convert.ToInt32(tt.Hour.ToString());
                            minutos_f = Convert.ToInt32(tt.Minute.ToString());
                        }
                        catch { }

                        try
                        {
                                apt.RecurrenceInfo.Start = (DateTime)elRow.fecha_inicio.Date + new TimeSpan(hora_i, minutos_i, 0);
                                apt.RecurrenceInfo.End = (DateTime)elRow.fecha_inicio.Date + new TimeSpan(hora_f, minutos_f, 0); //Si depende de OccurrentceCount el fin es calculado
                                apt.Start = (DateTime)elRow.fecha_inicio.Date + new TimeSpan(hora_i, minutos_i,  0); //Si depende de OccurrentceCount el fin es calculado
                                apt.End = (DateTime)elRow.fecha_inicio.Date + new TimeSpan(hora_f, minutos_f,  0); //Si depende de OccurrentceCount el fin es calculado
                        }
                        catch { }
                        
                        
                        apt.RecurrenceInfo.WeekDays = WeekDays.WorkDays;
                        apt.RecurrenceInfo.Type = RecurrenceType.Daily;
                        apt.CustomFields["CustomField1"] = cf;
                        IdCentro = mia.id_centro;
                        apt.CustomFields["IdTipoEvento"] = -1 * IdCentro;
                        apt.Location = "Sin Especificar";
                        apt.CustomFields["IdLocation"] = -1;
                        apt.CustomFields["IdPrioridad"] = -1;
                        apt.CustomFields["IdCentro"] = IdCentro;
                        apt.CustomFields["Local"] = "Sin Especificar";
                        apt.CustomFields["TipoCita"] = "publica";
                        apt.CustomFields["Creada"] = "admin";
                        apt.CustomFields["ResourceIDs"] = "";

                        miIDEvento = (IdCentro == 1) ? (IdCentro * 100000000 + (new Random()).Next(1, 100000000)) : (IdCentro * 100000 + (new Random()).Next(1, 100000));
                        apt.CustomFields["IdEvento"] = miIDEvento;

                        schedulerControl1.Storage.Appointments.Items.Add(apt);
                        //calc = OccurrenceCalculator.CreateInstance(apt.RecurrenceInfo);
                        //AppointmentBaseCollection apts = calc.CalcOccurrences(new TimeInterval(apt.RecurrenceInfo.Start, apt.RecurrenceInfo.End + DateTimeHelper.DaySpan), apt);


                    }
                    else //Está, sólo actuaizo algunas cosas si se cambiaron en la info sobre grupos en Infoemprende.
                    {
                        unApp.Subject = ("Clases al Grupo: " + utiles.quitacomillas(elRow.nombre.ToString()) + " | Asig: " + utiles.quitacomillas(elRow.nombre_materia)).Trim();

                        int hora_i = 8, minutos_i = 0, hora_f = 12, minutos_f = 0;
                        try
                        {
                            DateTime tt = Convert.ToDateTime(elRow.hora_inicio.ToString());
                            hora_i = Convert.ToInt32(tt.Hour.ToString());
                            minutos_i = Convert.ToInt32(tt.Minute.ToString());
                            tt = Convert.ToDateTime(elRow.hora_fin.ToString());
                            hora_f = Convert.ToInt32(tt.Hour.ToString());
                            minutos_f = Convert.ToInt32(tt.Minute.ToString());
                        }
                        catch { }

                        try
                        {
                            unApp.RecurrenceInfo.Start = (DateTime)elRow.fecha_inicio.Date + new TimeSpan(hora_i, minutos_i, 0);
                            unApp.RecurrenceInfo.End = (DateTime)elRow.fecha_inicio.Date + new TimeSpan(hora_f, minutos_f, 0); //Si depende de OccurrentceCount el fin es calculado
                            unApp.Start = (DateTime)elRow.fecha_inicio.Date + new TimeSpan(hora_i, minutos_i, 0); //Si depende de OccurrentceCount el fin es calculado
                            unApp.End = (DateTime)elRow.fecha_inicio.Date + new TimeSpan(hora_f, minutos_f, 0); //Si depende de OccurrentceCount el fin es calculado
                        }
                        catch { }

                        unApp.RecurrenceInfo.Range = RecurrenceRange.OccurrenceCount;
                        cantEncuetros = Convert.ToInt32(elRow.cant_encuentros);
                        unApp.RecurrenceInfo.OccurrenceCount = cantEncuetros; //Un encuentro si no está especiicada la cantidad en el registro del curso
                        
                        unApp.RecurrenceInfo.Type = RecurrenceType.Daily;
                        unApp.RecurrenceInfo.WeekDays = WeekDays.WorkDays;

                    }
                    schedulerControl1.Update();
                   // theSplash.pbProgress.PerformStep();
                  //  Application.DoEvents();
                }
           //     theSplash.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Se intentaba leer la inormación de grupos para actualizar citas.", "¡Error de datos!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //this.ActualizandoDocencia = true;
            //this.Form1_Load(this, new EventArgs());
            this.schedulerStorage1.RefreshData();
            this.schedulerControl1.Update();


        }

        private void schedulerControl1_AllowAppointmentDelete(object sender, AppointmentOperationEventArgs e)
        {
            e.Allow = CanUserModifyThisAppointment(e.Appointment);
        }

        bool CanUserModifyThisAppointment(Appointment apt)
        {
            object obj = apt.CustomFields["Creada"];
            if (obj == null)
                return true;
            string appointmentOwner = obj.ToString();
            if ((appointmentOwner == "admin") && (mia.nivel_acceso != 1))
                return false;
            return true;
        }

  

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.dateNavigator.Visible = this.barCheckItem1.Checked;
        }

        private void schedulerControl1_AllowAppointmentConflicts(object sender, AppointmentConflictEventArgs e)
        {
            e.Conflicts.Clear();
            FillConflictedAppointmentsCollection(e.Conflicts, e.Interval, ((SchedulerControl)sender).Storage.Appointments.Items, e.Appointment);
        }



        private void FillConflictedAppointmentsCollection(AppointmentBaseCollection conflicts, TimeInterval interval,
    AppointmentBaseCollection collection, Appointment currApt)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                Appointment apt = collection[i];
                if (new TimeInterval(apt.Start, apt.End).IntersectsWith(interval) & !(apt.Start == interval.End || apt.End == interval.Start))
                {
                    string local_usado = apt.CustomFields["Local"].ToString();

                    try
                     {
                        mia.local_a_usar =  currApt.CustomFields["Local"].ToString();
                     }
                    catch {  }


                    if ((apt != currApt) && (local_usado == mia.local_a_usar))
                    {
                        conflicts.Add(apt); 
                        toolTipController1.ShowHint("Horario y Lugar Ocupado.", Form_squedul.MousePosition);
                    }
                }
                if (apt.Type == AppointmentType.Pattern)
                {
                    FillConflictedAppointmentsCollection(conflicts, interval, apt.GetExceptions(), currApt);
                }
            }
        }





        SolidBrush msb = new SolidBrush(Color.FromArgb(90, 50, 50, 50));

        private void schedulerControl1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = Rectangle.Empty;
            if (schedulerControl1.ActiveView is DayView)
            {
                foreach (DayViewColumn column in ((DayViewInfo)schedulerControl1.ActiveView.ViewInfo).Columns)
                {
                    for (int i = 0; i < column.Cells.Count; i++)
                    {
                        TimeCell tc = column.Cells[i] as TimeCell;
                        if (tc.Interval.Start.Hour >= 12 && tc.Interval.Start.Hour <= 13)
                        {
                            if (rect == Rectangle.Empty)
                                rect = tc.Bounds;
                            else
                                rect = Rectangle.Union(rect, tc.Bounds);
                        }
                    }
                }
                if (rect != Rectangle.Empty)
                    using (Font f = new Font("Arial", rect.Height - 4, GraphicsUnit.Pixel))
                        e.Graphics.DrawString("Horario de Almuerzo", f, msb, new PointF(rect.X + rect.Width / 2 - f.Size * 3, rect.Y + rect.Height / 2 - f.Size / 2));
            }
        }

    
       
    }
}
