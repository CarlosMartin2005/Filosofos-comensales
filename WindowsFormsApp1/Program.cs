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

        // Crear semáforos
        private static SemaphoreSlim semaforo1 = new SemaphoreSlim(1, 1); // Inicialmente en verde
        private static SemaphoreSlim semaforo2 = new SemaphoreSlim(0, 1); // Inicialmente en rojo
        
        [STAThread]
        static void Main()
        {   
            // Crear dos hilos
            Thread hilo1 = new Thread(new ThreadStart(ejec1));
            Thread hilo2 = new Thread(new ThreadStart(ejec2));

            // Iniciar los hilos
            hilo1.Start();
            hilo2.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }

        static void ejec1()
        {
            // Código que ejecutará el hilo 1
            for (int i = 0; i < 5; i++)
            {
                semaforo1.Wait(); // Esperar a que el semáforo 1 esté en verde
                Console.WriteLine("Hilo 1 ejecutándose...");
                Thread.Sleep(1000); // Pausar el hilo por 1 segundo
                semaforo2.Release(); // Cambiar el semáforo 2 a verde
            }
        }

        static void ejec2()
        {
            // Código que ejecutará el hilo 2
            for (int i = 0; i < 5; i++)
            {
                semaforo2.Wait(); // Esperar a que el semáforo 2 esté en verde
                Console.WriteLine("Hilo 2 ejecutándose...");
                Thread.Sleep(1000); // Pausar el hilo por 1 segundo
                semaforo1.Release(); // Cambiar el semáforo 1 a verde
            }
        }
    }
}
