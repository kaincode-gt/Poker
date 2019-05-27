using Poker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPoker
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPantalla
    {

        int OPPONENT_CARD_SIZE =110;
        private AutoResetEvent _event = new AutoResetEvent(false);
        private Mensajeador Mensajeador = new Mensajeador();
        public Jugador jugador1 = new JugadorWPF("Luis");
        public DatosPartidaWPF datos;

        public DatosPartida DatosDePartida
        {
            get { return juego.Data; }
        }
        public Jugador JugadorUsuario
        {
            get {
                foreach (Jugador j in juego.Jugadores)
                    if (j is JugadorWPF)
                        return j;
                return new JugadorWPF("Batman") { Nombre="No.", Saldo=99 };
            }
        }
        Juego juego;

        public MainWindow()
        {

            InitializeComponent();
            DataContext = this;

            juego = new Juego(this);
            datos = new DatosPartidaWPF();
            juego.Data = datos;

            //LetreroBote.DataContextChanged;


            Jugador jugador1 = new JugadorWPF("Dama", this);
            JugadorIA jugador2 = new JugadorWPF_IA("Fantasma");
            JugadorIA jugador3 = new JugadorWPF_IA("Nimeria");
            juego.AñadirJugador(jugador1);
            juego.AñadirJugador(jugador2);
            juego.AñadirJugador(jugador3);

            //  Thread HiloDeJuego = new Thread(new ThreadStart(juego.NuevoJuego));
            //    AutoResetEvent _event = new AutoResetEvent(false);
            //HiloDeJuego.Start();
            GenerarBotonNuevoTurno();
        //    HiloDeJuego.Start();
           // Thread.Sleep(1000);
           
        }

        public void PintarPanelesJuego()
        {
            PanelOponentes.Children.Clear();
            foreach (Jugador j in juego.Jugadores)
            {
                //Button boton = new Button();
                if (j is JugadorIA)
                {
                    DibujarPanelOponente(j, false);
                }
                else
                {
                    DrawCard(j.Mano, true);
                }
            }
        }

        public void IniciarNuevoTurno(object sender, RoutedEventArgs e)
        {
            new Thread(new ThreadStart(()=> {
                juego.NuevoJuego();
            })).Start();

            //Si fue llamado por un botón, elimina el botón
            if (sender != null)
                PanelBotonesJugador.Children.Clear();

           Thread.Sleep(200);
            PintarPanelesJuego();
        }



        public void IniciarJuego()
        {
            throw new NotImplementedException();
        }

        public void SeleccionarJugadorActivo(Jugador jugador)
        {
            if (!(jugador is JugadorIA))
            {
                //PanelJugador.Background = Brushes.Coral;
            }
        }

        #region Dibujar Botones

        public void GenerarBotonNuevoTurno()
        {
            Button botonTurno = new Button();
            botonTurno.Content = "Nuevo";
            botonTurno.Click += IniciarNuevoTurno;
            PanelBotonesJugador.Children.Clear();
            PanelBotonesJugador.Children.Add(botonTurno);
        }

        public void BotonRespuestaUsuario(object sender, RoutedEventArgs e)
        {
            var accion = (Accion)Enum.Parse(typeof(Accion), ((Button)sender).Name);

            //Si es un jugador usuario, configuro la acción del botón pulsado
            if (juego.JugadorActual is JugadorWPF)
                ((JugadorWPF)juego.JugadorActual).AccionActual = accion;
            _event.Set();

            //Borrar los botones
            PanelBotonesJugador.Children.Clear();
        }

        public Accion MostrarBotonesJugador()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                PanelBotonesJugador.Children.Clear();
                foreach (Accion accion in (Accion[])Enum.GetValues(typeof(Accion)))
                {
                    Button boton = new Button() { Content = accion.ToString(), Name = accion.ToString() };
                    boton.Padding = new Thickness(5);
                    boton.Click += BotonRespuestaUsuario;
                    PanelBotonesJugador.Children.Add(boton);
                }
            }));

            _event.WaitOne();
            return Accion.ABANDONAR;
        }
        #endregion

        #region Render Cards Methods

        //public void PintarPanelJugador()
        //{
        //    StackPanel panel = new StackPanel();

        //    panel.SetValue(Grid.RowProperty, 4);
        //    panel.SetValue(Grid.ColumnProperty, 4);
        //}

        public void DrawCard(Mano mano, bool showCards)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                PanelCartas.Children.Clear();
                if (mano != null)
                {
                    foreach (Carta carta in mano.Cartas)
                    {
                        string id = carta.Id.ToString().Count() == 2 ? carta.Id.ToString() : "0" + carta.Id.ToString();
                        //Default card
                        //Image img = new Image() { Height = 140, Source = new BitmapImage(new Uri($"pack://application:,,,/Imagen/Cartas/card_back.png")) };
                        Image img = showCards? ObtenerImagenCarta(carta, 140) : ObtenerImagenCarta(null, 140);
                        //if (showCards)
                        //    img = ObtenerImagenCarta(carta, 140);
                        //   // img = new Image() { Height = 140, Source = new BitmapImage(new Uri($"pack://application:,,,/Imagen/Cartas/{id}.png")) };
                        //else
                        //    img = ObtenerImagenCarta(null, 140);

                        // Image img = new Image() { Height = 140, Source = new BitmapImage(new Uri($"pack://application:,,,/Imagen/Cartas/{id}.png")) };
                        img.Margin = new Thickness(10, 0, 0, 0);
                        PanelCartas.Children.Add(img);
                    }
                }
            }));
        }

        public void DibujarPanelOponente(Jugador jugador, bool mostrarCartas)
        {
            StackPanel panel = new StackPanel();
            
            //DataContext = jugador;
            panel.Orientation = Orientation.Horizontal;
            panel.Margin = new Thickness() { Left = 10, Top = 10, Bottom = 10, Right = 10 };

            StackPanel subPanel = new StackPanel() { Orientation = Orientation.Vertical, VerticalAlignment = VerticalAlignment.Center, Width = 125 };
            
            //TextBlock Nombre con enlace
            Binding enlace = new Binding("Nombre");
            enlace.Source = jugador;
            TextBlock nombre = new TextBlock() { FontSize = 24,  Foreground=Brushes.White, FontWeight = FontWeights.ExtraLight};
            // nombre.Foreground = Brushes.White;

            nombre.SetBinding(TextBlock.TextProperty, enlace);
            subPanel.Children.Add( nombre);

            Binding enlaceSaldo = new Binding("Saldo");
            enlaceSaldo.Source = jugador;
            TextBlock textSaldo = new TextBlock() { FontSize=18, Foreground = Brushes.White };
            textSaldo.SetBinding(TextBlock.TextProperty, enlaceSaldo);
            subPanel.Children.Add(textSaldo);

            panel.Children.Add(subPanel);
            if (jugador.Mano != null)
            {
                foreach (Carta c in jugador.Mano.Cartas)
                {
                    Image img = mostrarCartas ? ObtenerImagenCarta(c, OPPONENT_CARD_SIZE) : ObtenerImagenCarta(null, OPPONENT_CARD_SIZE);
                    //Image img = new Image() { Height = 90, Source = new BitmapImage(new Uri("pack://application:,,,/Imagen/Cartas/28.png")) };
                    //Image img = ObtenerImagenCarta(c, 90);
                    img.Margin = new Thickness(6,0,0,0);
                    panel.Children.Add(img);
                }
            }
            PanelOponentes.Children.Add(panel);
        }
        #endregion

        public Image ObtenerImagenCarta(Carta carta, int tamaño)
        {
            if (carta!=null)
            {
                string id = carta.Id.ToString().Count() == 2 ? carta.Id.ToString() : "0" + carta.Id.ToString();
                return new Image() { Height = tamaño, Source = new BitmapImage(new Uri($"pack://application:,,,/Imagen/Cartas/{id}.png")) };
            }
            return new Image() { Height = tamaño, Source = new BitmapImage(new Uri($"pack://application:,,,/Imagen/Cartas/card_back.png")) }; ;
        }

        public void MostrarMensaje(object mensaje)
        {
            Mensajeador.AñadirMensaje(mensaje.ToString());

            Dispatcher.BeginInvoke(new Action(() => {
                CajaInformacion.Text = Mensajeador.ListaEnCadena;
            }));
        }

        public void Actualizar()
        {
            throw new NotImplementedException();
        }

        public void TurnoTerminado()
        {
            Dispatcher.BeginInvoke(new Action(() => {

                PanelOponentes.Children.Clear();
                //Pintamos las cartas de los oponentes activos
                foreach (Jugador j in juego.Jugadores)
                {
                    if (j is JugadorIA)
                    {
                        DibujarPanelOponente(j, !j.Retirado);
                    }
                }


                GenerarBotonNuevoTurno();
            }));
            //Manipular estadísticas
            
        }
    }
}
