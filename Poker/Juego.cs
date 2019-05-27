using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Poker
{
    public class Juego
    {
        //Lista original de jugadores
        private List<Jugador> _jugadores;
        private Jugador _jugadorActual;
        private int _repartidor =-1;
        IPantalla Pantalla;

        List<int> apuestas = new List<int> { 5, 10, 15, 20 };
        public DatosPartida Data { get; set; }

        //Lista para usar durante el juego
        public List<Jugador> Jugadores { get { return _jugadores; }set { _jugadores = value; } }

        public int bote { get; set; }
        int resto;
        int apuestaInicial = 10;
        int apuestaActual;

        //bool isFinished = false;

        Baraja baraja;

        public Juego()
        {
            _jugadores = new List<Jugador>();
            //_jugadores.Add(new ConsolePlayer("Nacho"));
            //_jugadores.Add(new JugadorIA("Alba"));
            //_jugadores.Add(new JugadorIA("Helena"));
            //_jugadores.Add(new JugadorIA("webon"));

        }

        public Juego(IPantalla pantalla)
        {
            _jugadores = new List<Jugador>();
            //_jugadores.Add(new ConsolePlayer("Jugador"));
            //_jugadores.Add(new JugadorIA("Alba"));
            //_jugadores.Add(new JugadorIA("ignacia"));
            //_jugadores.Add(new JugadorIA("Helena"));

            this.Pantalla = pantalla;
        }

        public void AñadirJugador(Jugador jugador)
        {
            _jugadores.Add(jugador);
        }

        public void EliminarJugador(Jugador jugador)
        {
            _jugadores.Remove(jugador);
        }

        public Jugador JugadorActual { get { return _jugadorActual; } }

        /// <summary>
        /// Crea una lista ordenada de jugadores a partir del que reparte.
        /// </summary>
        /// <param name="repartidor"></param>
        /// <returns>Lista ordenada de jugadores</returns>
        private List<Jugador> OrdenarJugadoresPorTurno()
        {
            List<Jugador> jugadores = new List<Jugador>();
            //Si la lista de jugadores no se ha iniciado, la crea con un repartidor en la posición Longitud -2
            //para que el jugador uno sea el primero en apostar;
            if (_repartidor == -1)
            {
                //Si hay número de jugadores >=3
                _repartidor = _jugadores.Count - 3; //La tercera posición desde el final.
            }
            else
            {
                //El repartidor pasa al siguiente.
                _repartidor = _repartidor + 1 % _jugadores.Count;
               // int indice = _jugadores.IndexOf(RepartidorX) + 1 % Jugadores.Count;
            }

            int offset = _repartidor + 3;

            for (int i = 0; i < _jugadores.Count; i++)
            {
                jugadores.Add(_jugadores[(i + offset) % _jugadores.Count]);
            }
            return jugadores;
        }

        public List<Jugador> OrdenarJugadoresParaTurno()
        {
            return null;
        }

        public void BoteInicial()
        {
            //Bote inicial
            bote = resto != 0 ? resto : 0;
            resto = 0;



            //Se colocan las apuestas iniciales (los dos puestos anteriores al que encabeza la lista.
            bote += Jugadores[Jugadores.Count - 2].Apostar(apuestaInicial);
            bote += Jugadores[Jugadores.Count - 1].Apostar(apuestaInicial * 2);

            //Apuesta actual a igualar
            apuestaActual = apuestaInicial * 2;

            Data.Bote = Data.Resto != 0 ? Data.Resto : 0;
            Data.Resto = 0;
            Data.Bote += Jugadores[Jugadores.Count - 2].Apostar(apuestaInicial);
            Data.Bote += Jugadores[Jugadores.Count - 1].Apostar(apuestaInicial*2);
        }

        public void NuevoJuego()
        {
            //Creamos la baraja.
            baraja = new Baraja(500);

            Jugadores = OrdenarJugadoresPorTurno();
            
            foreach(Jugador j in Jugadores)
            Console.WriteLine(j);

            BoteInicial();

            //Se reparten las cartas
            foreach (Jugador jugador in Jugadores)
                jugador.Mano = baraja.RepartirNuevaMano();


            //Primer turno de apuestas
            Mensaje("\nPRIMERA RONDA DE APUESTAS\n");
            TurnoApuestas();

            //Descartes
            foreach (Jugador jugador in Jugadores)
                Descartar( jugador, jugador.PedirDescartes() );

            //Segundo turno de apuestas
            Mensaje("\nSEGUNDA RONDA DE APUESTAS\n");
            TurnoApuestas();

            //Fin de partida
          //  Mensaje("\nFIN DE PARTIDA");
            ResolverPartida();
        }

        /// <summary>
        /// Muestra las cartas finales y resuelve el ganador
        /// </summary>
        /// <param name="jugadores">Lista de jugadores</param>
        public void ResolverPartida()
        {
            Mensaje("FIN DE PARTIDA");
            Pantalla.TurnoTerminado();

            //Pinto los jugadores activos
            foreach (Jugador jugador in Jugadores)
            {
                if (!jugador.Retirado) { Mensaje(jugador.Nombre + " tiene: " + jugador.Mano.Rango + " de " + jugador.Mano.Valor); };
            }

            //Si solo queda un jugador, el ganador es el único que no se ha retirado.
            var ganador = QuedaUnJugador(Jugadores) ? Jugadores.Where(j => j.Retirado == false).First() : null;

            if (ganador == null)
            {
                ganador = ObtenerMejorMano();
            }

            //Si hay un ganador
            if (ganador != null)
            {
                Mensaje($"{ganador.Nombre} gana + {Data.Bote} euros!".ToUpper());
                ganador.AgregarSaldo(Data.Bote);


                List<Jugador> eliminados = new List<Jugador>();
                //Compruebo si alguno de los otros jugadores ha perdido.
                foreach (Jugador jugador in Jugadores)
                {
                    if (!jugador.Retirado && jugador!=ganador)
                    {
                        if (jugador.Bancarrota())
                            eliminados.Add(jugador);
                    };
                }

                //Elimino a todos los 
                foreach (Jugador j in eliminados)
                {
                    Jugadores.Remove(j);
                    Mensaje($"Jugador {j.Nombre} está sin saldo y se retira del juego!");
                }
                    
                //  ganador.AgregarSaldo(5000);
            }
            else
            {
                //Si no, se produce empate y el bote se vuelca en el resto
                resto = bote;
                Data.Resto = Data.Bote;
                Mensaje("EMPATE! NINGUNO GANA!");
            }

        }

        public void Descartar(Jugador jugador, List<int> indices)
        {
            List<Carta> cartas = jugador.Mano.Cartas.ToList();
            for (int i = 0; i < indices.Count; i++)
            {
                //Guarda una nueva carta en el hueco de la descartada
                cartas[indices[i]] = baraja.Repartir(1).FirstOrDefault();
            }
            jugador.Mano.Cartas = cartas;
        }

        /// <summary>
        /// Gestiona el turno de los jugadores hasta que ninguno apuesta más o todos menos uno se retiran.
        /// </summary>
        /// <param name="jugadores">Lista de jugadores</param>
        public void TurnoApuestas()
        {
            bool TurnoAcabado;

            //Reseteo a los jugadores retirados
            foreach (Jugador j in Jugadores)
                j.Retirado = false;

            //Turno inicial de apuestas
            do
            {
                TurnoAcabado = true;
                
                foreach (Jugador jugador in Jugadores)
                {
                    _jugadorActual = jugador;
                    if (jugador.Retirado) continue;
                    if (QuedaUnJugador(Jugadores)) continue;

                    //Selecciono al jugador
                    if (Pantalla != null) { Pantalla.SeleccionarJugadorActivo(jugador); }
                    Thread.Sleep(1000);
                    if (AccionJugador(jugador, jugador.PedirAccion()))
                    TurnoAcabado = false;
                }
            } while (!TurnoAcabado);
        }

        /// <summary>
        /// Gestiona la acción llevada a cabo por el jugador: Abandonar, Ver o Subir.
        /// </summary>
        /// <param name="jugador">El jugador que lleva a cabo la acción</param>
        /// <param name="accion">La acción elegida por el jugador</param>
        /// <returns>Verdadero la acción ha sido apostar</returns>
        public bool AccionJugador(Jugador jugador, Accion accion)
        {
            //Controlar si el jugador ha apostado
            bool jugadorApuesta = false;

            switch (accion)
            {
                case Accion.ABANDONAR:
                    jugador.Retirado = true;
                    Mensaje($"{jugador.Nombre} abandona.");
                    break;
                case Accion.VER:
                    bote += jugador.Apostar(apuestaActual);
                    Mensaje($"{jugador.Nombre} iguala la apuesta.");
                    break;
                case Accion.SUBIR:
                    int cantidad = jugador.PedirApuesta(apuestas);
                    jugadorApuesta = true;
                    apuestaActual += cantidad;
                    //Si se trata del primer o segundo apostante del inicio de turno se les descuenta de la apuesta inicial.
                    
                    bote += jugador.Apostar(apuestaActual);
                    Data.Bote += jugador.Apostar(apuestaActual);
                    Mensaje($"{jugador.Nombre} apuesta {cantidad}.");
                    //Mensaje($"->{jugador.Saldo} actual.");
                    //   Mensaje($"Bote: " + Data.Bote);
                    break;
            }
            return jugadorApuesta;
        }

        public Jugador ObtenerMejorMano()
        {
            Jugador ganador = null;
            Mano mejorMano = Jugadores[0].Mano;

            //Obtengo la mejor jugada de la mesa.
            foreach (Jugador j in Jugadores)
            {
                if (j.Retirado)
                    continue;
                if (j.Mano > mejorMano)
                {
                    mejorMano = j.Mano;
                    ganador = j;
                }
            }
            return ganador;
        }

        /// <summary>
        /// Comprueba si el queda un único jugador no retirado en el turno
        /// </summary>
        /// <param name="jugadores"></param>
        /// <returns>Verdadero si es el último jugador</returns>
        public bool QuedaUnJugador(List<Jugador> jugadores)
        {
            return jugadores.Where(j => j.Retirado == false).Count() <= 1;
        }


        /// <summary>
        /// Envía un mensaje de sistema a la consola o pantalla
        /// </summary>
        /// <param name="Mensaje">Mensaje a enviar</param>
        public void Mensaje(string Mensaje)
        {
            if (Pantalla != null)
            {
                Pantalla.MostrarMensaje(Mensaje);
            }
        }
    }
}
