using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Poker
{
    abstract public class DatosPartida
    {
        protected int _bote;
        protected int _resto;

        abstract public int Bote { get; set; }
        public int Resto { get { return _resto; } set { _resto = value; } }

        public DatosPartida()
        {
            _bote = _resto = 0;
        }
    }
}
