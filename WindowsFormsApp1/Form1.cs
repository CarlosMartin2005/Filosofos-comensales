using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Establecer la imagen de fondo
            this.BackgroundImage = Image.FromFile("Resources\\filosofos comensales.jpg");
            // Ajustar la imagen al tamaño del formulario
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Problema de los filósofos comensales";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            cantidadFilosofos.Font = new Font("Arial", 20, FontStyle.Bold);
        }

        private void btn_Aceptar_Click(object sender, EventArgs e)
        {
            string cantidad = textBox1.Text;
            try
            {
                if (int.TryParse(cantidad, out int cantidadint) && cantidadint >= 2 && cantidadint <= 10)
                {
                    Console.WriteLine("Cantidad de filosofos ingresados: " + cantidadint);
                    MessageBox.Show("Cantidad de filosofos ingresados: " + cantidadint, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form2 form2 = new Form2(cantidadint);

                    form2.Show();

                    this.Hide();
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, ingrese un número entre 2 y 10.");
                    MessageBox.Show("Entrada inválida. Por favor, ingrese un número entre 2 y 10.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR "+ex, "Se produjo un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
