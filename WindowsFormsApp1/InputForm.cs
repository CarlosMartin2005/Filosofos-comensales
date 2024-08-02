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
    public partial class InputForm : Form
    {
        public int CantidadComida { get; private set; }
        public InputForm(string nombreFilosofo)
        {
            InitializeComponent();
            labelMensaje.Text = $"Ingrese la cantidad de comida para {nombreFilosofo}: ";
        }

        private void btn_Aceptar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxCantidad.Text, out int comida) && comida > 0 && comida <= 10)
            {
                CantidadComida = comida;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Entrada inválida. Por favor, ingrese un número mayor a 0 y menor a 11.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InputForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
