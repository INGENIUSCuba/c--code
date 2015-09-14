using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;

namespace sistema
{
    public partial class Form_procesos_incubadora : Form
    {
        int id_doc;
        int maximo_id;

        public Form_procesos_incubadora()
        {
            InitializeComponent();
            cb_doc.SelectedIndex = 0;
        }

        private void Form_procesos_incubadora_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dS_DataEmprende.alumno' Puede moverla o quitarla según sea necesario.
            this.alumnoTableAdapter.Fill(this.dS_DataEmprende.alumno);

            this.doc_int_asesoria_incTableAdapter.Fill(this.dS_DataEmprende.doc_int_asesoria_inc);

            this.doc_int_asesoria_pre_incTableAdapter.Fill(this.dS_DataEmprende.doc_int_asesoria_pre_inc);

            this.detalle_expedienteTableAdapter.Fill(this.dS_DataEmprende.detalle_expediente);

            this.doc_int_check_detalleTableAdapter.Fill(this.dS_DataEmprende.doc_int_check_detalle);

            this.doc_int_detalle_tipo_checkTableAdapter.Fill(this.dS_DataEmprende.doc_int_detalle_tipo_check);

            this.doc_int_check_listTableAdapter.Fill(this.dS_DataEmprende.doc_int_check_list);

            this.estadoTableAdapter.filtra_mayor_6(this.dS_DataEmprende.estado);
            
            this.inc_vista_persona_proyectoTableAdapter.filtra_estado(this.dS_DataEmprende.inc_vista_persona_proyecto, 7);
            filtrado();
        }

        private void filtrado()
        {
            try
            {

                int id = Convert.ToInt32(lbestados.SelectedValue.ToString());
                inc_vista_persona_proyectoTableAdapter.filtra_estado(dS_DataEmprende.inc_vista_persona_proyecto, id);

                DataRowView view1 = (DataRowView)incvistapersonaproyectoBindingSource.Current;
                int id_alumno = Convert.ToInt32(view1["id_alumno"].ToString());
                string nombre = view1["nombre_solo"].ToString();
                inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, id_alumno);
                groupControl3.Text = "Documentos Internos de:  " + nombre;
            }
            catch
            {
                inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, 0);
                groupControl3.Text = "Documentos Internos de: (no existes alumnos en este estado) ";
            }
        }

        private void lbestados_MouseClick(object sender, MouseEventArgs e)
        {
            filtrado();
            
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lbestados.SelectedValue.ToString());
            string estado = lbestados.Text.ToString();
            Form_cambiar_estado Form_cambiar_estado1 = new Form_cambiar_estado(id, estado);
            Form_cambiar_estado1.ShowDialog();

            try
            {
                inc_vista_persona_proyectoTableAdapter.filtra_estado(dS_DataEmprende.inc_vista_persona_proyecto, id);

                int id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());
                inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, id_alumno);
                string nombre = gridView1.GetFocusedRowCellValue("nombre_solo").ToString();
                groupControl3.Text = "Documentos Internos de:  " + nombre;
            }
            catch { };
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            try
            {
                int id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());
                string nombre = gridView1.GetFocusedRowCellValue("nombre_solo").ToString();
                groupControl3.Text = "Documentos Internos de:  " + nombre;
                inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, id_alumno);
            }
            catch { }
        }

        private void dime_maximo(int max)
        {
            if (max >= 10000) max = max + 1;
            else max = Convert.ToInt32(Convert.ToString(mia.id_centro) + "0001");
            maximo_id = max;
        }

     

        private void insertar_chek(int id_alumno,int id_proyecto)
        {
            DateTime Hoy = DateTime.Today;
            string fecha_actual = Hoy.ToString("dd-MM-yyyy");

            dime_maximo(Convert.ToInt32(this.detalle_expedienteTableAdapter.maximo()));
            int id_detalle = maximo_id;

            dime_maximo(Convert.ToInt32(this.doc_int_check_listTableAdapter.maximo()));
            doc_int_check_listTableAdapter.inserta(id_alumno, fecha_actual, id_proyecto, id_detalle,maximo_id);
            id_doc = Convert.ToInt32(this.doc_int_check_listTableAdapter.maximo());


            int cant = Convert.ToInt32(doc_int_detalle_tipo_checkBindingSource.Count.ToString());
            doc_int_detalle_tipo_checkBindingSource.MoveFirst();
            
                for (int i = 1; i <=  cant; i++ )
                {
                    DataRowView view1 = (DataRowView)doc_int_detalle_tipo_checkBindingSource.Current;
                    //MessageBox.Show(view1["detalle_tipo_chek"].ToString());
                    int id_detalle_tipo = Convert.ToInt32(view1["id"].ToString());
                    int id_tipo = Convert.ToInt32(view1["id_tipo_chek"].ToString());

                    dime_maximo(Convert.ToInt32(this.doc_int_check_detalleTableAdapter.maximo()));
                    doc_int_check_detalleTableAdapter.inserta(id_doc,id_detalle_tipo,id_tipo, maximo_id);
                    doc_int_detalle_tipo_checkBindingSource.MoveNext();

                }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());
                string alumno = gridView1.GetFocusedRowCellValue("nombre_solo").ToString();
                string id_persona = gridView1.GetFocusedRowCellValue("id_persona").ToString();

                if (cb_doc.Text == "Acta de Confidencialidad")
                {
                    Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_persona, "Acta de Confidencialidad");
                    Form_doc_interno1.Size = new Size(970, 310);
                    Form_doc_interno1.ShowDialog();
                }

                if (cb_doc.Text == "Acta de Entrega")
                {
                    int id_proyecto = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_proyecto").ToString());
                    string proyecto = gridView1.GetFocusedRowCellValue("proyecto").ToString();
                    Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_proyecto, proyecto, id_persona, "Acta de Entrega");
                    Form_doc_interno1.Size = new Size(970, 333);
                    Form_doc_interno1.ShowDialog();
                }

                if (cb_doc.Text == "Check List")
                {
                    int id_proyecto = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_proyecto").ToString());
                    string proyecto = gridView1.GetFocusedRowCellValue("proyecto").ToString();
                    insertar_chek(id_alumno,id_proyecto);
                    Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_proyecto, proyecto, id_doc, id_persona, "Check List");
                    Form_doc_interno1.Size = new Size(995, 580);
                    Form_doc_interno1.ShowDialog();
                    
                }

                if (cb_doc.Text == "Resumen Ejecutivo")
                {
                    int id_proyecto = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_proyecto").ToString());
                    string proyecto = gridView1.GetFocusedRowCellValue("proyecto").ToString();
                    Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_proyecto, proyecto, id_persona, "Resumen Ejecutivo");
                    Form_doc_interno1.Size = new Size(870, 490);
                    Form_doc_interno1.ShowDialog();
                }

                if (cb_doc.Text == "Carta de Finiquito")
                {
                    int id_proyecto = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_proyecto").ToString());
                    string proyecto = gridView1.GetFocusedRowCellValue("proyecto").ToString();
                    Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_proyecto, proyecto, id_persona, "Carta de Finiquito");
                    Form_doc_interno1.Size = new Size(870, 490);
                    Form_doc_interno1.ShowDialog();
                }

                if (cb_doc.Text == "Dictamen")
                {
                    int id_proyecto = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_proyecto").ToString());
                    string proyecto = gridView1.GetFocusedRowCellValue("proyecto").ToString();
                    Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_proyecto, proyecto, id_persona, "Dictamen");
                    Form_doc_interno1.Size = new Size(870, 420);
                    Form_doc_interno1.ShowDialog();
                }

                if (cb_doc.Text == "Acuerdo")
                {
                    int id_proyecto = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_proyecto").ToString());
                    string proyecto = gridView1.GetFocusedRowCellValue("proyecto").ToString();
                    Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_proyecto, proyecto, id_persona, "Acuerdo");
                    Form_doc_interno1.Size = new Size(870, 330);
                    Form_doc_interno1.ShowDialog();
                }
                
                 if (cb_doc.Text == "Criterio de Evaluación")
                {
                    int id_proyecto = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_proyecto").ToString());
                    string proyecto = gridView1.GetFocusedRowCellValue("proyecto").ToString();
                    Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_proyecto, proyecto, id_persona, "Criterio de Evaluación");
                    Form_doc_interno1.Size = new Size(870, 710);
                    Form_doc_interno1.ShowDialog();
                }

                 if (cb_doc.Text == "Plan de Incubación")
                {
                    int id_proyecto = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_proyecto").ToString());
                    string proyecto = gridView1.GetFocusedRowCellValue("proyecto").ToString();
                    Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_proyecto, proyecto, id_persona, "Plan de Incubación");
                    Form_doc_interno1.Size = new Size(870, 530);
                    Form_doc_interno1.ShowDialog();
                }

                 if (cb_doc.Text == "Informe de Asesoría Pre Inc.")
                {
                    Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_persona, "Informe de Asesoría Pre Inc.");
                    Form_doc_interno1.Size = new Size(960, 500);
                    Form_doc_interno1.ShowDialog();
                }

                 if (cb_doc.Text == "Informe de Asesoría Inc.")
                 {
                     Form_doc_interno Form_doc_interno1 = new Form_doc_interno(cb_doc.SelectedIndex, id_alumno, alumno, id_persona, "Informe de Asesoría Inc.");
                     Form_doc_interno1.Size = new Size(960, 500);
                     Form_doc_interno1.ShowDialog();
                 }

                inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, id_alumno);
                

                
            } 
            catch { MessageBox.Show("No existen Alumnos en este estado, por favor verifique.", "Error al crear nuevo documento", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void acta_conf(reporte_doc_int_acta_confid frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }

        private void acta_entrega(reporte_doc_int_acta_entrega frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }

        private void check_list(reporte_doc_int_check_list frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }

        private void acuerdo(reporte_doc_int_acuerdo frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }
        private void dictamen(reporte_doc_int_dictamen frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }

        private void criterio_evaluacion(reporte_doc_int_criterio_evaluacion  frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }

        private void plan_incubacion(reporte_doc_int_plan_incubacion  frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }


        private void resumen(reporte_doc_int_resumen_ejecutivo frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }

        private void carta(reporte_doc_int_carta_finiquito frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }

        private void asesoria_pre_inc(reporte_doc_int_asesoria_pre_inc  frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }

        private void asesoria_inc(reporte_doc_int_asesoria_inc frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {

                int id_doc = Convert.ToInt32(gridView4.GetFocusedRowCellValue("id").ToString());
                string anulado =gridView4.GetFocusedRowCellValue("anulado").ToString();
                string nombre = gridView4.GetFocusedRowCellValue("nombre").ToString();

                if (anulado == "No")
                {
                    if (nombre == "Acta de Confidencialidad")
                    {
                        reporte_doc_int_acta_confid reporte1 = new reporte_doc_int_acta_confid(id_doc);
                        acta_conf(reporte1);
                    }

                    if (nombre == "Acta de Entrega")
                    {
                        reporte_doc_int_acta_entrega reporte1 = new reporte_doc_int_acta_entrega(id_doc);
                        acta_entrega(reporte1);
                    }

                    if (nombre == "Check List")
                    {
                        reporte_doc_int_check_list reporte1 = new reporte_doc_int_check_list(id_doc);
                        check_list(reporte1);
                    }
                    if (nombre == "Dictamen")
                    {
                        reporte_doc_int_dictamen reporte1 = new reporte_doc_int_dictamen(id_doc);
                        dictamen(reporte1);
                    }
                    if (nombre == "Acuerdo")
                    {
                        reporte_doc_int_acuerdo reporte1 = new reporte_doc_int_acuerdo(id_doc);
                        acuerdo(reporte1);
                    }


                    if (nombre == "Resumen Ejecutivo")
                    {
                        reporte_doc_int_resumen_ejecutivo reporte1 = new reporte_doc_int_resumen_ejecutivo(id_doc);
                        resumen(reporte1);
                    }

                    if (nombre == "Carta de Finiquito")
                    {
                        reporte_doc_int_carta_finiquito reporte1 = new reporte_doc_int_carta_finiquito(id_doc);
                        carta(reporte1);
                    }


                    if (nombre == "Criterio de Evaluación")
                    {
                        reporte_doc_int_criterio_evaluacion reporte1 = new reporte_doc_int_criterio_evaluacion(id_doc);
                        criterio_evaluacion(reporte1);
                    }

                    if (nombre == "Plan de Incubación")
                    {
                        reporte_doc_int_plan_incubacion reporte1 = new reporte_doc_int_plan_incubacion(id_doc);
                        plan_incubacion(reporte1);
                    }
                    
                    if (nombre == "Informe de Asesoría Pre Inc.")
                    {
                        reporte_doc_int_asesoria_pre_inc reporte1 = new reporte_doc_int_asesoria_pre_inc(id_doc);
                        asesoria_pre_inc(reporte1);
                    }

                    if (nombre == "Informe de Asesoría Inc.")
                    {
                        reporte_doc_int_asesoria_inc reporte1 = new reporte_doc_int_asesoria_inc(id_doc);
                        asesoria_inc(reporte1);
                    }

                }
                else { MessageBox.Show("Este Documento no se puede imprimir porque fue ANULADO.", "Error al imprimir reporte", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch { MessageBox.Show("Este Alumno no posee Documentos internos o no ha seleccionado aún un Alumno.", "Error al imprimir reporte", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }


        

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {

                string firmado = gridView4.GetFocusedRowCellValue("firmado").ToString();
                if (firmado == "No")
                {


                    DialogResult Resp;
                    Resp = MessageBox.Show("Se eliminará un documento interno ¿Está seguro?", mia.mensaje_eliminando_superior, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (Resp == DialogResult.Yes)
                    {

                        try
                        {
                            string documento = gridView4.GetFocusedRowCellValue("nombre").ToString();
                            int id = Convert.ToInt32(gridView4.GetFocusedRowCellValue("id").ToString());
                            int id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());
                            int id_expediente = Convert.ToInt32(gridView4.GetFocusedRowCellValue("id_expediente").ToString());


                            if (documento == "Acta de Confidencialidad") { doc_int_acta_confidencialidadTableAdapter1.borrar(id); }
                            if (documento == "Acta de Entrega") { doc_int_acta_entregaTableAdapter1.borrar(id); }
                            if (documento == "Check List") { doc_int_check_listTableAdapter1.borrar(id); }
                            if (documento == "Resumen Ejecutivo") { doc_int_resumen_ejecutivoTableAdapter1.borrar(id); }
                            if (documento == "Carta de Finiquito") { doc_int_carta_finiquitoTableAdapter1.borrar(id); }
                            if (documento == "Dictamen") { doc_int_dictamenTableAdapter1.borrar(id); }
                            if (documento == "Acuerdo") { doc_int_acuerdoTableAdapter1.borrar(id); }
                            if (documento == "Criterio de Evaluación") { doc_int_criterio_evaluacionTableAdapter1.borrar(id); }
                            if (documento == "Plan de Incubación") { doc_int_plan_encubacionTableAdapter1.borrar(id); }
                            if (documento == "Informe de Asesoría Pre Inc.") { doc_int_asesoria_pre_incTableAdapter.borrar(id); }
                            if (documento == "Informe de Asesoría Inc.") { doc_int_asesoria_incTableAdapter.borrar(id); }
                            
                            ///////////////////////// para borrar del disco duro ///////////
                            string id_persona = gridView4.GetFocusedRowCellValue("id_persona").ToString();
                            string rutaDestino = @mia.camino + "\\Alumnos\\" + Convert.ToString(id_persona) + "\\";
                            string archivoDestino = System.IO.Path.Combine(rutaDestino, gridView4.GetFocusedRowCellValue("url").ToString());
                            System.IO.File.Delete(archivoDestino);

                            detalle_expedienteTableAdapter.borrar(id_expediente);


                            inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, id_alumno);

                        }
                        catch { MessageBox.Show("Este Alumno no posee Documentos internos a borrar.", "Error al borrar documento interno", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    }
                }
                else { MessageBox.Show("Este Documento no puede ser Borrado porque está Firmado, solo se puede ANULAR.", "Error al borrar documento interno", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch { MessageBox.Show("Este Alumno no posee Documentos internos a borrar.", "Error al borrar documento interno", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }


        private void reporte_listado_persona_estado(reporte_doc_int_listado_persona_estado frm)
        {

            frm.CreateDocument();
            Form_reportes Form_reportes1 = new Form_reportes();
            Form_reportes1.printBarManager1.PrintControl.PrintingSystem = frm.PrintingSystem;
            Form_reportes1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
         try
          {
            int id_estado = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_estado").ToString());
            reporte_doc_int_listado_persona_estado reporte1 = new reporte_doc_int_listado_persona_estado(id_estado);
            reporte_listado_persona_estado(reporte1);
          }
         catch { MessageBox.Show("No existen Alumnos en este Estado.", "Error al generar reporte", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                string idalumno = gridView1.GetFocusedRowCellValue("id_persona").ToString();
                string nombre = gridView1.GetFocusedRowCellValue("nombre_solo").ToString();

                Form_detalle_expediente Form_detalle_expediente1 = new Form_detalle_expediente(idalumno, nombre);
                Form_detalle_expediente1.ShowDialog();

            }
            catch { MessageBox.Show("No existen Alumnos en este Estado.", "Error al ver Expediente", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void button9_Click(object sender, EventArgs e)
        {
         DialogResult Resp;
         Resp = MessageBox.Show("Este documento pasará a ser un DOCUMENTO FIRMADO ¿Está seguro?", "Documento firmado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
         if (Resp == DialogResult.Yes)
         {
             try
             {
                 int id_doc = Convert.ToInt32(gridView4.GetFocusedRowCellValue("id").ToString());
                 string tipo_doc = gridView4.GetFocusedRowCellValue("nombre").ToString();
                 int id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());

                 if (tipo_doc == "Acta de Confidencialidad") { doc_int_acta_confidencialidadTableAdapter1.firma("Si", id_doc); }
                 if (tipo_doc == "Acta de Entrega") { doc_int_acta_entregaTableAdapter1.firma("Si", id_doc); }
                 if (tipo_doc == "Check List") { doc_int_check_listTableAdapter1.firma("Si", id_doc); }
                 if (tipo_doc == "Resumen Ejecutivo") { doc_int_resumen_ejecutivoTableAdapter1.firma("Si", id_doc); }
                 if (tipo_doc == "Carta de Finiquito") { doc_int_carta_finiquitoTableAdapter1.firma("Si", id_doc); }
                 if (tipo_doc == "Dictamen") { doc_int_dictamenTableAdapter1.firma("Si", id_doc); }
                 if (tipo_doc == "Acuerdo") { doc_int_acuerdoTableAdapter1.firma("Si", id_doc); }
                 if (tipo_doc == "Criterio de Evaluación") { doc_int_criterio_evaluacionTableAdapter1.firma("Si", id_doc); }
                 if (tipo_doc == "Plan de Incubación") { doc_int_plan_encubacionTableAdapter1.firma("Si", id_doc); }
                 if (tipo_doc == "Informe de Asesoría Pre Inc.") { doc_int_asesoria_pre_incTableAdapter.firma("Si", id_doc); }
                 if (tipo_doc == "Informe de Asesoría Inc.") { doc_int_asesoria_incTableAdapter.firma("Si", id_doc); }

                 inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, id_alumno);
             }
             catch { MessageBox.Show("No existen Alumnos en este Estado.", "Error al firmar documento", MessageBoxButtons.OK, MessageBoxIcon.Information); }
         }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string firmado = gridView4.GetFocusedRowCellValue("firmado").ToString();
                if (firmado == "Si")
                {
                    DialogResult Resp;
                    Resp = MessageBox.Show("¿Está seguro que desea ANULAR este DOCUMENTO FIRMADO ?", "Anulando Documento firmado.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (Resp == DialogResult.Yes)
                    {
                        try
                        {
                            int id_doc = Convert.ToInt32(gridView4.GetFocusedRowCellValue("id").ToString());
                            string tipo_doc = gridView4.GetFocusedRowCellValue("nombre").ToString();
                            int id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());
                            int id_expediente = Convert.ToInt32(gridView4.GetFocusedRowCellValue("id_expediente").ToString());


                            if (tipo_doc == "Acta de Confidencialidad") { doc_int_acta_confidencialidadTableAdapter1.anular("Si", id_doc); }
                            if (tipo_doc == "Acta de Entrega") { doc_int_acta_entregaTableAdapter1.anular("Si", id_doc); }
                            if (tipo_doc == "Check List") { doc_int_check_listTableAdapter1.anular("Si", id_doc); }
                            if (tipo_doc == "Resumen Ejecutivo") { doc_int_resumen_ejecutivoTableAdapter1.anular("Si", id_doc); }
                            if (tipo_doc == "Carta de Finiquito") { doc_int_carta_finiquitoTableAdapter1.anular("Si", id_doc); }
                            if (tipo_doc == "Dictamen") { doc_int_dictamenTableAdapter1.anular("Si", id_doc); }
                            if (tipo_doc == "Acuerdo") { doc_int_acuerdoTableAdapter1.anular("Si", id_doc); }
                            if (tipo_doc == "Criterio de Evaluación") { doc_int_criterio_evaluacionTableAdapter1.anular("Si", id_doc); }
                            if (tipo_doc == "Plan de Incubación") { doc_int_plan_encubacionTableAdapter1.anular("Si", id_doc); }
                            if (tipo_doc == "Informe de Asesoría Pre Inc.") { doc_int_asesoria_pre_incTableAdapter.anular("Si", id_doc); }
                            if (tipo_doc == "Informe de Asesoría Inc.") { doc_int_asesoria_incTableAdapter.anular("Si", id_doc); }
                            

                            detalle_expedienteTableAdapter.anular("Si", "",id_expediente);

                            string id_persona = gridView4.GetFocusedRowCellValue("id_persona").ToString();
                            string rutaDestino = @mia.camino + "\\Alumnos\\" + Convert.ToString(id_persona) + "\\";
                            string archivoDestino = System.IO.Path.Combine(rutaDestino, gridView4.GetFocusedRowCellValue("url").ToString());
                            System.IO.File.Delete(archivoDestino);

                            inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, id_alumno);
                        }
                        catch { MessageBox.Show("No existen Alumnos en este Estado.", "Error al anular documento", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    }
                }
                else { MessageBox.Show("Este Documento no puede ser Anulado porque NO está Firmado, Se puede Borrar sin problemas.", "Error al Anular documento interno", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch { MessageBox.Show("Este Alumno no posee Documentos internos a Anular.", "Error al Anular documento interno", MessageBoxButtons.OK, MessageBoxIcon.Information); }

        }

        private void gridView4_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string anulado = View.GetRowCellDisplayText(e.RowHandle, View.Columns["anulado"]);
                string firmado = View.GetRowCellDisplayText(e.RowHandle, View.Columns["firmado"]);
               
                if (firmado == "Si")
                {
                    e.Appearance.BackColor = Color.DeepSkyBlue;
                    e.Appearance.BackColor2 = Color.Azure;
                    //e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }

                if (anulado == "Si")
                {
                    e.Appearance.BackColor = Color.Salmon;
                    e.Appearance.BackColor2 = Color.SeaShell;
                    //e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Italic);
                }
            }
        }

     

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {

                int id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());
                string nombre = gridView1.GetFocusedRowCellValue("nombres_completo").ToString();
                Form_asesoria Form_asesoria1 = new Form_asesoria(id_alumno, nombre, "1");
                Form_asesoria1.ShowDialog();
            }
            catch { MessageBox.Show("No existen Alumnos en este Estado.", "Error al crear una asesoría", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string idpersona = gridView1.GetFocusedRowCellValue("id_persona").ToString();
                Form_persona Form_persona1 = new Form_persona(idpersona, 1);
                Form_persona1.Size = new Size(1020, 330);
                Form_persona1.ShowDialog();
            }
            catch { MessageBox.Show("No existen Alumnos en este Estado.", "Error al editar alumno", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void limpiar_doc_interno()
           {
               try
               {
                   string documento = gridView4.GetFocusedRowCellValue("nombre").ToString();
                   int id = Convert.ToInt32(gridView4.GetFocusedRowCellValue("id").ToString());
                   int id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());
                   int id_expediente = Convert.ToInt32(gridView4.GetFocusedRowCellValue("id_expediente").ToString());


                   if (documento == "Acta de Confidencialidad") { doc_int_acta_confidencialidadTableAdapter1.borrar(id); }
                   if (documento == "Acta de Entrega") { doc_int_acta_entregaTableAdapter1.borrar(id); }
                   if (documento == "Check List") { doc_int_check_listTableAdapter1.borrar(id); }
                   if (documento == "Resumen Ejecutivo") { doc_int_resumen_ejecutivoTableAdapter1.borrar(id); }
                   if (documento == "Carta de Finiquito") { doc_int_carta_finiquitoTableAdapter1.borrar(id); }
                   if (documento == "Dictamen") { doc_int_dictamenTableAdapter1.borrar(id); }
                   if (documento == "Acuerdo") { doc_int_acuerdoTableAdapter1.borrar(id); }
                   if (documento == "Criterio de Evaluación") { doc_int_criterio_evaluacionTableAdapter1.borrar(id); }
                   if (documento == "Plan de Incubación") { doc_int_plan_encubacionTableAdapter1.borrar(id); }
                   if (documento == "Informe de Asesoría Pre Inc.") { doc_int_asesoria_pre_incTableAdapter.borrar(id); }
                   if (documento == "Informe de Asesoría Inc.") { doc_int_asesoria_incTableAdapter.borrar(id); }

                   ///////////////////////// para borrar del disco duro ///////////
                   string id_persona = gridView4.GetFocusedRowCellValue("id_persona").ToString();
                   string rutaDestino = @mia.camino + "\\Alumnos\\" + Convert.ToString(id_persona) + "\\";
                   string archivoDestino = System.IO.Path.Combine(rutaDestino, gridView4.GetFocusedRowCellValue("url").ToString());
                   System.IO.File.Delete(archivoDestino);

                   detalle_expedienteTableAdapter.borrar(id_expediente);


                 //  inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, id_alumno);

               }
               catch 
               {
               //    MessageBox.Show("Este Alumno no posee Documentos internos a borrar.", "Error al borrar documento interno", MessageBoxButtons.OK, MessageBoxIcon.Information); 
               }
           }


        
        private void button12_Click(object sender, EventArgs e)
        {
            if (incvistapersonaproyectoBindingSource.Count != 0)
            {
                DialogResult Resp;
                Resp = MessageBox.Show("¿Está seguro que desea ELIMINAR este Alumno de este ESTADO y colocarlo en el estado anterior ?", "Eliminado alumno de estado.", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (Resp == DialogResult.Yes)
                {
                    int id_estado_CB = Convert.ToInt32(lbestados.SelectedValue.ToString());
                    int id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());
                    int id_estado = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_estado").ToString());

                    /* falta hacer lo de eleiminar otods los doc internos */
                    if (id_estado == 7)
                    {
                        if (inc_documentos_internosBindingSource.Count != 0)
                        {
                            inc_documentos_internosBindingSource.MoveFirst();
                            for (int i = 1; i <= inc_documentos_internosBindingSource.Count; i++)
                            {
                                limpiar_doc_interno();
                                inc_documentos_internosBindingSource.MoveNext();
                            }
                        }
                        
                    }


                  
                    alumnoTableAdapter.actualiza_estado(id_estado - 1, id_alumno);
                    inc_vista_persona_proyectoTableAdapter.filtra_estado(dS_DataEmprende.inc_vista_persona_proyecto, id_estado_CB);
                    try
                    {
                        id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());
                        inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, id_alumno);
                        string nombre = gridView1.GetFocusedRowCellValue("nombre_solo").ToString();
                        groupControl3.Text = "Documentos Internos de:  " + nombre;
                    }
                    catch { groupControl3.Text = "Documentos Internos   "; inc_documentos_internosTableAdapter.filtra_alumno(dS_DataEmprende.inc_documentos_internos, 99999999); }

                
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                int id_alumno = Convert.ToInt32(gridView1.GetFocusedRowCellValue("id_alumno").ToString());
                string nombre = gridView1.GetFocusedRowCellValue("nombres_completo").ToString();
                Form_asesoria Form_asesoria1 = new Form_asesoria(id_alumno, nombre, "1");
                Form_asesoria1.ShowDialog();
            }
            catch { MessageBox.Show("No existen Alumnos en este Estado.", "Error al generar cita", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

     
    }
}
