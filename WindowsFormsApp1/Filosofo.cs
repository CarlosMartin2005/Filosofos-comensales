using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Filosofo
    {
        // Propiedades
        public int Numero { get; set; }
        public string Nombre { get; set; }
        public int CantidadComida { get; set; }
        public Tenedor TenedorIzquierdo { get; set; }
        public Tenedor TenedorDerecho { get; set; }

        private static Random random = new Random();

        // Constructor
        public Filosofo(int numero, string nombre, int cantidadComida, Tenedor tenedorIzquierdo, Tenedor tenedorDerecho)
        {
            Numero = numero;
            Nombre = nombre;
            CantidadComida = cantidadComida;
            TenedorIzquierdo = tenedorIzquierdo;
            TenedorDerecho = tenedorDerecho;
        }

        // Método para pensar
        public void Pensar()
        {
            Console.WriteLine($"{Nombre} está pensando.");
            Thread.Sleep(5000); // Pausar el hilo 5 segundos
            Console.WriteLine($"{Nombre} ha terminado de pensar.");
        }

        // Método para comer
        public void Comer(SemaphoreSlim semaforo)
        {
            while (CantidadComida > 0)
            {
                semaforo.Wait(); // Esperar a que el semáforo esté disponible
                try
                {
                    Pensar(); // Filósofo piensa

                    Console.WriteLine($"Filosofo {Nombre} está hambriento");
                    TenedorIzquierdo.Tomar();
                    Console.WriteLine($"Filosofo {Nombre} ha tomado el tenedor izquierdo");
                    TenedorDerecho.Tomar();
                    Console.WriteLine($"Filosofo {Nombre} ha tomado el tenedor derecho");

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

            Console.WriteLine($"{Nombre} ha terminado de comer.");
        }
    }
}
