using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Crear una instancia de Form1 y mostrarla
            Form1 form1 = new Form1();
            // Manejar el evento FormClosed para cerrar la aplicación cuando se cierren todos los formularios
            form1.FormClosed += (s, args) =>
            {
                if (Application.OpenForms.Count == 0) // Cuando se cierran todos los formularios
                {
                    Application.ExitThread(); // Cerrar la aplicación
                }
            };
            Application.Run(form1); // Ejecutar la aplicación
        }
    }
}
