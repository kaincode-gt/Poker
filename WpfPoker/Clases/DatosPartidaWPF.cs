using Poker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPoker
{
    public class DatosPartidaWPF : DatosPartida, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public override int Bote
        {
            get { return _bote; }
            set
            {
                _bote = value;
                OnPropertyChanged("Bote");
            }
        }

        public DatosPartidaWPF()
        {
        //    BoteNuevo = 98;
        }



        public void OnPropertyChanged(string nombre)
        {
            if (PropertyChanged != null)
            {
                
                PropertyChanged(this, new PropertyChangedEventArgs(nombre));
            }
        }
    }
}
