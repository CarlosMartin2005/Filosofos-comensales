using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private int cantidadint;
        private List<string> nombresFilosofos;
        private List<Filosofo> filosofos;
        private List<Tenedor> tenedores;
        private List<Task> tareas;
        private CancellationTokenSource cts;
        private SemaphoreSlim semaforo;

        public Form2(int cantidadint)
        {
            InitializeComponent();

            this.cantidadint = cantidadint;
            cts = new CancellationTokenSource();
            nombresFilosofos = ObtenerNombresFilosofos();
            tenedores = CrearTenedores(cantidadint);
            filosofos = CrearFilosofos(cantidadint, nombresFilosofos, tenedores);

            semaforo = new SemaphoreSlim(cantidadint / 2); // Permitir que la mitad de los filósofos coman al mismo tiempo

            // Crear tareas
            tareas = new List<Task>();
            foreach (Filosofo filosofo in filosofos)
            {
                var tarea = Task.Run(() => EjecutarFilosofo(filosofo, cts.Token, semaforo));
                tareas.Add(tarea);
            }

            // Esperar a que todas las tareas terminen
            Task.WhenAll(tareas).ContinueWith(t =>
            {
                Console.WriteLine("Todos los filósofos han terminado de comer.");
            });
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public static int SolicitarCantidadComida(string nombreFilosofo)
        {
            using (InputForm inputForm = new InputForm(nombreFilosofo))
            {
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    return inputForm.CantidadComida;
                }
                else
                {
                    throw new OperationCanceledException("El usuario canceló la entrada de cantidad de comida.");
                }
            }
        }

        static List<string> ObtenerNombresFilosofos()
        {
            return new List<string>
            {
                "Sócrates", "Platón", "Aristóteles", "Descartes", "Kant",
                "Nietzsche", "Hegel", "Spinoza", "Locke", "Hume"
            };
        }

        static List<Tenedor> CrearTenedores(int cantidadFilosofos)
        {
            List<Tenedor> tenedores = new List<Tenedor>();
            for (int i = 0; i < cantidadFilosofos; i++)
            {
                tenedores.Add(new Tenedor());
            }
            return tenedores;
        }

        static List<Filosofo> CrearFilosofos(int cantidadFilosofos, List<string> nombresFilosofos, List<Tenedor> tenedores)
        {
            List<Filosofo> filosofos = new List<Filosofo>();
            for (int i = 0; i < cantidadFilosofos; i++)
            {
                Tenedor tenedorIzquierdo = tenedores[i];
                Tenedor tenedorDerecho = tenedores[(i + 1) % cantidadFilosofos];
                int cantidadComida = SolicitarCantidadComida(nombresFilosofos[i]);
                Filosofo filosofo = new Filosofo(i + 1, nombresFilosofos[i], cantidadComida, tenedorIzquierdo, tenedorDerecho);
                filosofos.Add(filosofo);
            }
            return filosofos;
        }

        private async Task EjecutarFilosofo(Filosofo filosofo, CancellationToken token, SemaphoreSlim semaforo)
        {
            while (filosofo.CantidadComida > 0 && !token.IsCancellationRequested)
            {
                // Esperar a que termine de comer un filósofo
                await Task.Run(() => filosofo.Comer(semaforo), token);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Cancelar todas las tareas cuando el formulario se esté cerrando
            cts.Cancel();
            base.OnFormClosing(e);
        }
    }
}
