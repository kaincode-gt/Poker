using Poker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPoker
{
    /// <summary>
    /// Jugador Usuario para interfaz gráfica WPF
    /// </summary>
    public class JugadorWPF : Jugador , INotifyPropertyChanged
    {
        private MainWindow VentanaJuego; //Referencia a la ventana de juego
        private Accion _accionActual;

       public event PropertyChangedEventHandler PropertyChanged;

        public Accion AccionActual
        { get { return _accionActual; } set { _accionActual = value; } }

        public new int Saldo
        {
            get
            {
                return _saldo;
            }
            set
            {
                _saldo = value;
                OnPropertyChanged("Saldo");
            }
        }

        public JugadorWPF(string nombre) : base(nombre)
        {

        }

        public JugadorWPF(string nombre, MainWindow ventana) : base(nombre)
        {
            VentanaJuego = ventana;
            //  VentanaJuego = Application.Current.Windows[0] as MainWindow;
        }

        public override Accion PedirAccion()
        {
            Accion accion = VentanaJuego.MostrarBotonesJugador();
            return _accionActual;
        }

        public override int PedirApuesta(List<int> apuestas)
        {
            return 5;
        }

        public override int Apostar(int cantidad)
        {
            Saldo -= cantidad;
            return cantidad;
        }

        public override void AgregarSaldo(int cantidad)
        {
            Saldo += cantidad;
        }


        public void OnPropertyChanged(string nombre)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nombre));
        }
    }
}
