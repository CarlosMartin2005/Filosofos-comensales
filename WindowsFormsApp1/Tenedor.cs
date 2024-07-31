using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Tenedor
    {
        private readonly SemaphoreSlim _semaforo1 = new SemaphoreSlim(1, 1); // Crear semáforos

        public int disponible { get; private set; } = 0; // 0 = disponible, 1 = en uso

        public void Tomar()
        {
            _semaforo1.Wait(); // Esperar a que el semáforo 1 esté en verde
            disponible = 1; // Cambiar el estado del tenedor a no disponible
        }
        public void Dejar()
        {
            disponible = 0; // Cambiar el estado del tenedor a disponible
            _semaforo1.Release(); // Cambiar el semáforo 1 a verde
        }

    }
}
