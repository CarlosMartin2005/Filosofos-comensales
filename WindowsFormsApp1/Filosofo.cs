using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Filosofo
    {
        // Propiedades
        public int Numero { get; set; }
        public string Nombre { get; set; }
        public string estado;
        public string Estado
        {
            get => estado;
            private set
            {
                estado = value;
                OnEstadoCambiado?.Invoke(this, EventArgs.Empty);
            }
        }

        public int CantidadComida { get; set; }
        public Tenedor TenedorIzquierdo { get; set; }
        public Tenedor TenedorDerecho { get; set; }

        private static Random random = new Random();

        // Evento que se dispara cuando el estado cambia
        public event EventHandler OnEstadoCambiado;

        // Constructor
        public Filosofo(int numero, string nombre, int cantidadComida, Tenedor tenedorIzquierdo, Tenedor tenedorDerecho)
        {
            // Inicializar propiedades
            Numero = numero;
            Nombre = nombre;
            Estado = "Pensando";
            CantidadComida = cantidadComida;
            TenedorIzquierdo = tenedorIzquierdo;
            TenedorDerecho = tenedorDerecho;
        }

        // Método para pensar
        public void Pensar()
        {
            Estado = "Pensando";
            Console.WriteLine($"{Nombre} está pensando.");
            Thread.Sleep(5000); // Pausar el hilo 5 segundos
            Console.WriteLine($"{Nombre} ha terminado de pensar.");
        }

        // Método para comer
        public void Comer(SemaphoreSlim semaforo)
        {
            while (CantidadComida > 0)
            {
                try
                {
                    semaforo.Wait(); // Esperar a que el semáforo esté disponible
                    Pensar(); // Filósofo piensa

                    Estado = "Hambriento";
                    Console.WriteLine($"Filosofo {Nombre} está hambriento");
                    TenedorIzquierdo.Tomar();
                    Console.WriteLine($"Filosofo {Nombre} ha tomado el tenedor izquierdo");
                    TenedorDerecho.Tomar();
                    Console.WriteLine($"Filosofo {Nombre} ha tomado el tenedor derecho");

                    Estado = "Comiendo";
                    Console.WriteLine($"Filosofo {Nombre} está comiendo un bocado");
                    Thread.Sleep(5000); // Pausar el hilo 5 segundos
                    CantidadComida--;

                    TenedorIzquierdo.Dejar();
                    Console.WriteLine($"Filosofo {Nombre} ha dejado el tenedor izquierdo");
                    TenedorDerecho.Dejar();
                    Console.WriteLine($"Filosofo {Nombre} ha dejado el tenedor derecho");
                }
                finally
                {
                    semaforo.Release(); // Cambiar el semáforo a verde
                }
            }
            Estado = "Pensando";
            Console.WriteLine($"{Nombre} ha terminado de comer  y está pensando.");
        }
    }
}
