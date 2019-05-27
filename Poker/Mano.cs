using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
    public enum Rango
    {
        NADA = 0,
        PAREJA,
        DOBLE_PAREJA,
        TRIO,
        COLOR,
        POKER,
        FULL,
        ESCALERA,
        ESCALERA_COLOR,
        ESCALERA_REAL
    }

    public class Mano : IComparable
    {
        public IEnumerable<Carta> Cartas;

        #region Métodos Evaluación de Jugadas

        public bool EsEscaleraReal
        {
            get
            {
                return EsEscalera && EsColor && CartaMasAlta==Valor.AS;
            }
        }

        public bool EsEscaleraDeColor
        {
            get
            {
                return EsEscalera && EsColor && CartaMasAlta != Valor.AS;
            }
        }

        public bool EsPoker
        {
            get
            {
                return Cartas.GroupBy(c => c.Valor).Where(g => g.Count() == 4).Count() == 1;
            }
        }

        public bool EsFull
        {
            get
            {
                return EsPareja && EsTrio;
            }
        }

        public bool EsColor
        {
            get
            {
                return Cartas.GroupBy(c=> c.Palo).Where(g=>g.Count() == Cartas.Count()).Any();
            }
        }

        public bool EsEscalera
        {
            get
            {
                var listaOrdenada = Cartas.OrderBy(x => x.Valor).ToList();
                for (int i = 0; i < listaOrdenada.Count - 1; i++)
                {
                    if (listaOrdenada[i].Valor + 1 != listaOrdenada[i + 1].Valor)
                        return false;
                }
                return true;
            }
        }


        public bool EsTrio
        {
            get
            {
                return Cartas.GroupBy(c => c.Valor).Where(g => g.Count() == 3).Any();
            }
        }
        public bool EsDoblePareja
        {
            get
            {
                return Cartas.GroupBy(c => c.Valor).Where(g => g.Count() == 2).Count() == 2;
            }
        }

        public bool EsPareja
        {
            get
            {
                return Cartas.GroupBy(c => c.Valor).Where(g => g.Count() == 2).Count() == 1;
            }
        }

        public Valor CartaMasAlta
        {
            get
            {
                return Cartas.Max(x => x.Valor);
            }
        }


        #endregion

        /// <summary>
        /// Obtiene la puntuación sumada de todas las cartas.
        /// </summary>
        public int PuntuacionTotal
        {
            get
            {
                int resultado = 0;
                foreach (Carta c in Cartas)
                    resultado += (int)c.Valor;
                return resultado;
            }
        }

        public Rango Rango
        {
            get
            {
                if (EsEscaleraReal)
                    return Rango.ESCALERA_REAL;
                if (EsEscaleraDeColor)
                    return Rango.ESCALERA_COLOR;
                if (EsFull)
                    return Rango.FULL;
                if (EsPoker)
                    return Rango.POKER;
                if (EsColor)
                    return Rango.COLOR;
                if (EsEscalera)
                    return Rango.ESCALERA;
                if (EsTrio)
                    return Rango.TRIO;
                if (EsDoblePareja)
                    return Rango.DOBLE_PAREJA;
                if (EsPareja)
                    return Rango.PAREJA;
                return Rango.NADA;
            }
        }

        public Valor Valor
        {
            get
            {
                switch (Rango)
                {
                    case Rango.ESCALERA:
                        break;
                    case Rango.POKER:
                        //El valor del poker
                        return CartaMasAlta;
                    case Rango.TRIO:
                        //El valor del trío
                        return CartaMasAlta;
                    case Rango.DOBLE_PAREJA:
                        //El valor de la pareja más alta
                        return Cartas.GroupBy(c => c.Valor).Where(g => g.Count() == 2).Max(x => x.Key);
                    case Rango.PAREJA:
                        //El valor de la pareja
                        return Cartas.GroupBy(c => c.Valor).Where(g => g.Count() == 2).Max(x => x.Key);
                    case Rango.NADA:
                        //El valor la carta más alta
                        return CartaMasAlta;
                }
                return 0;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Mano otraMano = obj as Mano;
            if (this.Rango > otraMano.Rango)
                return 1;
            else if (this.Rango == otraMano.Rango)
                return 0;
            else
                return -1;
        }

        #region Comparadores

        public static bool operator >(Mano a, Mano b)
        {
            if (a.Rango > b.Rango)
                return true;
            else if (a.Rango == b.Rango)
                return a.Valor > b.Valor; //Puntuacion total?
            return false;
        }

        public static bool operator <(Mano a, Mano b)
        {
            if (a.Rango < b.Rango)
                return true;
            else if (a.Rango == b.Rango)
                return a.Valor < b.Valor;
            return false;
        }

        #endregion

        /// <summary>
        /// Muestra los valores de la mano por consola
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string devuelto = string.Empty + "\n";
          //  devuelto += string.Format("Cartas:\n", Cartas.Count());
            for (int i = 0; i < Cartas.Count(); i++)
            {
                devuelto += string.Format("- {0}: " + Cartas.ElementAt(i) + "\n", i + 1);
            }
            return devuelto;
        }
    }
}
