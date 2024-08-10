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

        private Dictionary<int, PictureBox> pictureBoxTenedoresMap;
        private Dictionary<int, PictureBox> pictureBoxFilosofosMap;
        private Dictionary<int, PictureBox> pictureBoxPlatosMap;
        private Dictionary<int, Label> labelFilosofosMap;


        public Form2(int cantidadint)
        {
            InitializeComponent();
            this.cantidadint = cantidadint;

            // Iniciar la lógica de los filósofos en un hilo separado
            Task.Run(() => IniciarFilosofo());
        }

        private void IniciarFilosofo()
        {
            cts = new CancellationTokenSource(); // Crear token de cancelación
            nombresFilosofos = ObtenerNombresFilosofos(); // Obtener nombres de los filósofos
            tenedores = CrearTenedores(cantidadint, this); // Crear tenedores
            filosofos = CrearFilosofos(cantidadint, nombresFilosofos, tenedores, pictureBoxFilosofosMap, labelFilosofosMap, this); // Crear filósofos

            semaforo = new SemaphoreSlim(cantidadint); // Crear semáforo

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
            MessageBox.Show("Todos los filósofos han terminado de comer.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Inicializar el diccionario de Labels de estados
            labelFilosofosMap = new Dictionary<int, Label>
            {
                { 0, label1 },
                { 1, label2 },
                { 2, label3 },
                { 3, label4 },
                { 4, label5 },
                { 5, label6 },
                { 6, label7 },
                { 7, label8 },
                { 8, label9 },
                { 9, label10 }
            };

            // Inicializar el diccionario de PictureBoxes de filósofos
            pictureBoxFilosofosMap = new Dictionary<int, PictureBox>
            {
                { 0, pictureBox1 },
                { 1, pictureBox2 },
                { 2, pictureBox3 },
                { 3, pictureBox4 },
                { 4, pictureBox5 },
                { 5, pictureBox6 },
                { 6, pictureBox7 },
                { 7, pictureBox8 },
                { 8, pictureBox9 },
                { 9, pictureBox10 }
            };

            // Inicializar el diccionario de PictureBoxes de platos
            pictureBoxPlatosMap = new Dictionary<int, PictureBox>
            {
                { 0, pictureBox22 },
                { 1, pictureBox23 },
                { 2, pictureBox24 },
                { 3, pictureBox25 },
                { 4, pictureBox26 },
                { 5, pictureBox27 },
                { 6, pictureBox28 },
                { 7, pictureBox29 },
                { 8, pictureBox30 },
                { 9, pictureBox31 }
            };

            // Inicializar el diccionario de PictureBoxes de tenedores
            pictureBoxTenedoresMap = new Dictionary<int, PictureBox>
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

            // var res = MessageBox.Show("Mostrar los filósofos comensales", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (tenedores == null)
            {
                MessageBox.Show("La lista de tenedores no se ha inicializado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ocultar todos los labels al inicio
            foreach (var label in labelFilosofosMap.Values)
            {
                label.Visible = false;
            }

            // Mostrar solo los labels necesarios
            for (int i = 0; i < tenedores.Count; i++)
            {
                labelFilosofosMap[i].Visible = true;
            }
            
            // Mostrar solo los PictureBox de platos necesarios
            for (int i = 0; i < tenedores.Count; i++)
            {
                pictureBoxPlatosMap[i].Visible = true;
            }

            // Ocultar los PictureBox restantes si hay más PictureBox que filósofos
            for (int i = tenedores.Count; i < pictureBoxPlatosMap.Count; i++)
            {
                pictureBoxPlatosMap[i].Visible = false;
            }

            // Mostrar solo los PictureBox necesarios para los tenedores
            for (int i = 0; i < tenedores.Count; i++)
            {
                ActualizarUITenedor(tenedores[i],pictureBoxTenedoresMap[i]);
            }

            // Ocultar los PictureBox restantes si hay más PictureBox que tenedores
            for (int i = tenedores.Count; i < pictureBoxTenedoresMap.Count; i++)
            {
                pictureBoxTenedoresMap[i].Visible = false;
            }
        }

        private void ActualizarUITenedor(Tenedor tenedor, PictureBox pictureBox)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { ActualizarUITenedor(tenedor, pictureBox); });
            }
            else
            {
                if (tenedor.disponible == 0)
                {
                    pictureBox.Image = Image.FromFile("Resources\\tenedor.png");
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Visible = true;
                    pictureBox.BackColor = Color.White;
                    pictureBox.BringToFront();
                }
                else
                {
                    pictureBox.Visible = true;
                    pictureBox.BackColor = Color.Red;
                    pictureBox.BringToFront();
                }
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
                    MessageBox.Show("El usuario canceló la entrada de cantidad de comida, .", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        static List<Tenedor> CrearTenedores(int cantidadFilosofos, Form2 form2)
        {
            List<Tenedor> tenedores = new List<Tenedor>();
            for (int i = 0; i < cantidadFilosofos; i++)
            {
                Tenedor tenedor = new Tenedor();
                int index = i; // Capturar la variable i
                tenedor.EstadoCambiado += (sender, e) => form2.ActualizarUITenedor(tenedor, form2.pictureBoxTenedoresMap[index]);
                tenedores.Add(tenedor);
            }
            return tenedores;
        }

        static List<Filosofo> CrearFilosofos(int cantidadFilosofos, List<string> nombresFilosofos, List<Tenedor> tenedores, Dictionary<int, PictureBox> pictureBoxFilosofosMap, Dictionary<int, Label> labelFilosofosMap, Form2 form2)
        {
            List<Filosofo> filosofos = new List<Filosofo>();
            for (int i = 0; i < cantidadFilosofos; i++)
            {
                Tenedor tenedorIzquierdo = tenedores[i];
                Tenedor tenedorDerecho = tenedores[(i + 1) % cantidadFilosofos];
                int cantidadComida = SolicitarCantidadComida(nombresFilosofos[i]);
                Filosofo filosofo = new Filosofo(i + 1, nombresFilosofos[i], cantidadComida, tenedorIzquierdo, tenedorDerecho);

                // Actualizar el estado del filósofo junto con el PictureBox correspondiente
                int index = i; // Capturar la variable i
                filosofo.OnEstadoCambiado += (sender, e) => form2.ActualizarUIFilosofo(filosofo, form2.pictureBoxFilosofosMap[index], form2.labelFilosofosMap[index]);

                filosofos.Add(filosofo);
            }

            return filosofos;
        }

        private void ActualizarUIFilosofo(Filosofo filosofo, PictureBox pictureBox, Label label)
        {
            if (InvokeRequired)
            {
                // Invocar el método de forma segura
                Invoke((MethodInvoker)(() => ActualizarUIFilosofo(filosofo, pictureBox, label)));
                return;
            }

            switch (filosofo.Estado)
            {
                case "Pensando":
                    pictureBox.Image = Image.FromFile("Resources\\pensando.gif");
                    label.Text = $"{filosofo.Nombre} está pensando...";
                    break;
                case "Hambriento":
                    pictureBox.Image = Image.FromFile("Resources\\conhambre.gif");
                    label.Text = $"{filosofo.Nombre} está hambriento...";

                    break;
                case "Comiendo":
                    pictureBox.Image = Image.FromFile("Resources\\comiendo.gif");
                    label.Text = $"{filosofo.Nombre} está comiendo un bocado...";

                    break;
                case "Terminado":
                    pictureBox.Image = Image.FromFile("Resources\\terminado.gif");
                    label.Text = $"{filosofo.Nombre} terminó de comer.";
                    break;
            }

            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // Ajustar la imagen al PictureBox
            pictureBox.BringToFront(); // Traer el PictureBox al frente

            // Actualizar el plato correspondiente
            if (pictureBoxPlatosMap.ContainsKey(filosofo.Numero - 1))
            {
                PictureBox pictureBoxPlato = pictureBoxPlatosMap[filosofo.Numero - 1];

                // Determinar la imagen del plato según la cantidad de comida restante
                if (filosofo.CantidadComida > 5)
                {
                    pictureBoxPlato.Image = Image.FromFile("Resources\\plato_lleno.png");
                }
                else if (filosofo.CantidadComida > 0)
                {
                    pictureBoxPlato.Image = Image.FromFile("Resources\\plato_medio.png");
                }
                else
                {
                    pictureBoxPlato.Image = Image.FromFile("Resources\\plato_vacio.png");
                }

                pictureBoxPlato.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxPlato.BringToFront();
            }
        }

        private void EjecutarFilosofo(Filosofo filosofo, CancellationToken token, SemaphoreSlim semaforo)
        {
            // Filósofo piensa y come mientras haya comida y no se haya cancelado el token
            while (filosofo.CantidadComida > 0 && !token.IsCancellationRequested)
            {
                // Esperar a que termine de comer un filósofo
                filosofo.Comer(semaforo);
            }
        }
    }
}
