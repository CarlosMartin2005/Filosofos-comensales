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

        private void btn_Aceptar_Click(object sender, EventArgs e)
        {
            // Obtener la cantidad de filósofos ingresada por el usuario
            string cantidad = textBox1.Text;
            try
            {
                // Verificar si la entrada es un número válido
                if (int.TryParse(cantidad, out int cantidadint) && cantidadint >= 2 && cantidadint <= 10)
                {
                    Console.WriteLine("Cantidad de filosofos ingresados: " + cantidadint);
                    MessageBox.Show("Cantidad de filosofos ingresados: " + cantidadint, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide(); // Ocultar Form1
                    Form2 form2 = new Form2(cantidadint); // Crear una instancia de Form2
                    form2.FormClosed += (s, args) => this.Close(); // Cerrar Form1 cuando Form2 se cierra

                    form2.Show(); // Mostrar Form2
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
