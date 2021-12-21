using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TPSimFinal
{
    public partial class Form1 : Form
    {
        public Calculo calculo;
        public int desde;
        public int hasta;
        public int cantSimulaciones;
        public int cantPerritos;
        public int media;
        public int a;
        public int b;

        public Form2_enunciado pantallaEnunciado;

        public Form1()
        {
            InitializeComponent();
            txtSimulacion.Text = "100";
            txtCantPerritos.Text = "7";
            txtMedia.Text = "15";
            txtA.Text = "10";
            txtB.Text = "20";
        }


        private void btnSimular_Click(object sender, EventArgs e)
        {
            cantSimulaciones = Int32.Parse(txtSimulacion.Text);
            desde = txtDesde.Text != "" ? Int32.Parse(txtDesde.Text) : 0;
            hasta = txtHasta.Text != "" ? Int32.Parse(txtHasta.Text) : cantSimulaciones;
            cantPerritos = txtCantPerritos.Text != "" ? Int32.Parse(txtCantPerritos.Text) : 7;
            media =txtMedia.Text != "" ? Int32.Parse(txtMedia.Text) : 15;
            a = txtA.Text != "" ? Int32.Parse(txtA.Text) : 10;
            b = txtB.Text != "" ? Int32.Parse(txtB.Text) : 20;
            calculo = new Calculo(this);


        }

        public void cargarFila(Simulacion s)
        {
            dgvInicio.Rows.Add(
                s.numeroSimulacion,
                Math.Round(s.reloj,4),
                s.evento,
                Math.Round(s.proxFinAfuera, 4),
                Math.Round(s.proxFinDentro,4),
                s.cantidadAfuera,
                s.cantidadDentro,
                s.exitos,
                s.acExitos,
                Math.Round(s.probabilidadAfuera,4),
                s.acumulador,
                Math.Round(s.promedioDentro,4)
                ) ;
        }

        public void limpiarPantalla(object sender, EventArgs e)
        {
            txtSimulacion.Text = "100";
            txtDesde.Text = "";
            txtHasta.Text = "";
            txtCantPerritos.Text = "7";
            txtMedia.Text = "15";
            txtA.Text = "10";
            txtB.Text = "20";

            dgvInicio.Rows.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pantallaEnunciado = new Form2_enunciado();
            pantallaEnunciado.ShowDialog();
        }
    }
}
