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
        public event EventHandler EstadoCambiado;

        private readonly SemaphoreSlim _semaforo1 = new SemaphoreSlim(1, 1); // Crear semáforo

        public int disponible { get; private set; } = 0; // 0 = disponible, 1 = en uso

        public bool Tomar()
        {
            _semaforo1.Wait(); // Esperar a que el semáforo 1 esté en verde
            if (disponible == 0)
            {
                disponible = 1; // Cambiar el estado del tenedor a no disponible
                EstadoCambiado?.Invoke(this, EventArgs.Empty); // Disparar el evento
                return true;
            }
            else 
            {
                _semaforo1.Release(); // Cambiar el semáforo 1 a verde
                return false;
            
            }
        }
        public void Dejar()
        {
            if (disponible == 1)
            {
                disponible = 0; // Cambiar el estado del tenedor a disponible
                EstadoCambiado?.Invoke(this, EventArgs.Empty); // Disparar el evento
            }
            _semaforo1.Release(); // Cambiar el semáforo 1 a verde
        }
    }
}
