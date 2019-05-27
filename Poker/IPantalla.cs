using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public interface IPantalla
    {
        void IniciarJuego();
        void MostrarMensaje(object mensaje);
        void Actualizar();
        void SeleccionarJugadorActivo(Jugador jugador);
        void TurnoTerminado();
    }
}
