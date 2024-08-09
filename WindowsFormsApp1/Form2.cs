using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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

        //private System.Windows.Forms.Timer timer;
        private Dictionary<int, PictureBox> pictureBoxMap;


        public Form2(int cantidadint)
        {
            InitializeComponent();
            this.cantidadint = cantidadint;

            // Configurar Form2_Load para manejar el evento de carga
            this.Load += new System.EventHandler(this.Form2_Load);

            // Iniciar la lógica de los filósofos en un hilo separado
            Task.Run(() => IniciarFilosofo());
        }

        private void IniciarFilosofo()
        {
            cts = new CancellationTokenSource();
            nombresFilosofos = ObtenerNombresFilosofos();
            tenedores = CrearTenedores(cantidadint);
            filosofos = CrearFilosofos(cantidadint, nombresFilosofos, tenedores);

            semaforo = new SemaphoreSlim(cantidadint / 2); // Permitir que la mitad de los filósofos coman al mismo tiempo

            // Crear hilos
            hilos = new List<Thread>();
            foreach (Filosofo filosofo in filosofos)
            {
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
            MessageBox.Show("Yepa ya estoy por aquí" + cantidadint, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Inicializar el diccionario de PictureBoxes
            pictureBoxMap = new Dictionary<int, PictureBox>
            {
                { 0, pictureBox12 },
                { 1, pictureBox13 },
                { 2, pictureBox14 },
                { 3, pictureBox15 },
                { 4, pictureBox16 },
                { 5, pictureBox17 },
                { 6, pictureBox18 },
                { 7, pictureBox19 },
                { 8, pictureBox20 },
                { 9, pictureBox21 }
            };

            // Mostrar solo los PictureBox necesarios
            for (int i = 0; i < tenedores.Count; i++)
            {
                ActualizarUI(pictureBoxMap[i]);
                Console.WriteLine("PictueBox puesto con éxito");
            }

            // Ocultar los PictureBox restantes si hay más PictureBox que tenedores
            for (int i = tenedores.Count; i < pictureBoxMap.Count; i++)
            {
                pictureBoxMap[i].Visible = false;
            }
        }

        private void ActualizarUI(PictureBox pictureBox)
        {
            if (!InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate {
                    pictureBox.Image = Image.FromFile("Resources\\tenedor.png");
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // Ajusta la imagen para que se estire y ocupe todo el PictureBox
                    pictureBox.Visible = true;
                    pictureBox.BackColor = Color.Beige;
                    pictureBox.BringToFront();
                });
            }
            else
            {
                pictureBox.BackColor = Color.Red;
                pictureBox.Visible = true;
                pictureBox.BringToFront();
            }
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {

        }
    }
}
