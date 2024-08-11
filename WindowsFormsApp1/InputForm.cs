using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class InputForm : Form
    {
        public int CantidadComida { get; private set; }
        public InputForm(string nombreFilosofo)
        {
            InitializeComponent();
            this.Load += new EventHandler(InputForm_Load); // Asignar el evento Load al formulario

            labelMensaje.Text = $"Ingrese la cantidad de comida para {nombreFilosofo}: ";
        }

        private void btn_Aceptar_Click(object sender, EventArgs e)
        {
            // Verificar si la entrada es un número válido
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
            textBoxCantidad.SelectAll();
            textBoxCantidad.Focus();
            // Forzar que el formulario obtenga el foco
            this.ActiveControl = textBoxCantidad;

        }

        private void textBoxCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números y la tecla de retroceso
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Anula la pulsación de la tecla
            }
        }
    }
}
