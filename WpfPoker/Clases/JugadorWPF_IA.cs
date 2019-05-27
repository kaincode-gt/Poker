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
    public class JugadorWPF_IA : JugadorIA , INotifyPropertyChanged
    {
    //    private MainWindow VentanaJuego; //Referencia a la ventana de juego
       // private Accion _accionActual;

       public event PropertyChangedEventHandler PropertyChanged;

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

        public JugadorWPF_IA(string nombre) : base(nombre)
        {

        }

        public override void AgregarSaldo(int cantidad)
        {
            Saldo += cantidad;
        }

        public override int Apostar(int cantidad)
        {
            Saldo -= cantidad;
            return cantidad;
        }

        public void OnPropertyChanged(string nombre)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nombre));
        }
    }
}
