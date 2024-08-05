using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {

        private int cantidadint;
        private List<string> nombresFilosofos;
        private List<Filosofo> filosofos;
        private List<Tenedor> tenedores;
        private List<Thread> hilos;
        private CancellationTokenSource cts;
        private SemaphoreSlim semaforo;

        private List<PictureBox> pictureBoxesFilosofos;
        private List<Label> labelsEstadosFilosofos;
        private List<PictureBox> pictureBoxesTenedores;

        private System.Windows.Forms.Timer timer;

        public Form2(int cantidadint)
        {
            this.Show();
            InitializeComponent();

            this.cantidadint = cantidadint;
            cts = new CancellationTokenSource();
            nombresFilosofos = ObtenerNombresFilosofos();
            tenedores = CrearTenedores(cantidadint);
            filosofos = CrearFilosofos(cantidadint, nombresFilosofos, tenedores);

            semaforo = new SemaphoreSlim(cantidadint / 2); // Permitir que la mitad de los filósofos coman al mismo tiempo

            // Inicializar componentes gráficos
            pictureBoxesFilosofos = new List<PictureBox>();
            labelsEstadosFilosofos = new List<Label>();
            pictureBoxesTenedores = new List<PictureBox>();

            // Crear hilos
            hilos = new List<Thread>();
            foreach (Filosofo filosofo in filosofos)
            {
                filosofo.Pensar();
                var hilo = new Thread(() => EjecutarFilosofo(filosofo, cts.Token, semaforo));
                hilos.Add(hilo);
                hilo.Start();
            }

            // Esperar a que todos los hilos terminen
            foreach (Thread hilo in hilos)
            {
                hilo.Join();
            }
            Console.WriteLine("Todos los filósofos han terminado de comer.");
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

        private void EjecutarFilosofo(Filosofo filosofo, CancellationToken token, SemaphoreSlim semaforo)
        {
            while (filosofo.CantidadComida > 0 && !token.IsCancellationRequested)
            {
                // Esperar a que termine de comer un filósofo
                filosofo.Comer(semaforo);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
