using System;
using System.Collections.Generic;

namespace Poker
{
    public class Baraja
    {
        private List<Carta> _cartas;

        public List<Carta> Cartas
        {
            get
            {
                return _cartas;
            }
            set
            {
                _cartas = value;
            }
        }

        public Baraja()
        {
            Cartas = CrearCartas();
        }

        public Baraja(int iteracciones)
        {
            Cartas = CrearCartas();
            Barajar(iteracciones);
        }

        private List<Carta> CrearCartas()
        {
            Cartas = new List<Carta>();
            int indice =0;
            foreach (Palo palo in (Palo[])Enum.GetValues(typeof(Palo)))
            {
                foreach (Valor valor in (Valor[])Enum.GetValues(typeof(Valor)))
                {
                    Cartas.Add(new Carta(palo, valor) { Id = indice});
                    indice++;
                }
            }
            return Cartas;
        }

        public void Barajar(int iteracciones)
        {
            Random rd = new Random();

            for (int i = 0; i < iteracciones; i++)
            {
                int aleatorio = rd.Next(_cartas.Count);
                Carta c = _cartas[aleatorio];
                _cartas.RemoveAt(aleatorio);
                _cartas.Add(c);
            }
        }

        public void Barajar()
        {
            Barajar(500);
        }

        /// <summary>
        /// Reparte un número n de cartas de la baraja
        /// </summary>
        /// <param name="numero">El número de cartas a repartir</param>
        /// <returns>Cartas repartidas</returns>
        public List<Carta> Repartir(int numeroCartas)
        {
            List<Carta> mano = new List<Carta>();

            for (int i = 0; i < numeroCartas; i++)
            {
                Carta c = _cartas[i];
                mano.Add(c);
            }
            _cartas.RemoveRange(0, numeroCartas);
            return mano;
        }

        
        public Mano RepartirNuevaMano()
        {
            List<Carta> cartas = new List<Carta>();

            for (int i = 0; i < 5; i++)
            {
                Carta c = _cartas[i];
                cartas.Add(c);
            }

          //  Mano mano= new Mano() { Cartas = _cartas.Take(5) };
            _cartas.RemoveRange(0, 5);
            return new Mano() { Cartas = cartas};
        }

        public override string ToString()
        {
            string devuelto = string.Format("Quedan {0} cartas en la baraja:\n", Cartas.Count);
            for (int i = 0; i < _cartas.Count; i++)
            {
                devuelto += string.Format("- {0}: " + Cartas[i] + "\n", i + 1);
            }
            return devuelto;
        }
    }
}
