using System;

namespace Poker
{
    public enum Valor
    {
        DOS =2,
        TRES,
        CUATRO,
        CINCO,
        SEIS,
        SIETE,
        OCHO,
        NUEVE,
        DIEZ,
        JOTA,
        REINA,
        REY,
        AS
    }

    public enum Palo
    {
        CORAZONES,
        PICAS,
        DIAMANTES,
        TRÉBOLES
    }

    public class Carta : IComparable
    {
        private Valor _valor { get; set; }
        private Palo _palo { get; set; }
        private int _id;

        public Carta()
        {
            _valor = Valor.AS;
            _palo = Palo.DIAMANTES;
        }

        public Carta(Palo palo, Valor valor)
        {
            this.Palo = palo;
            this.Valor = valor;
        }

        public Palo Palo
        {
            get
            {
                return _palo;
            }
            set
            {
                _palo = value;
            }
        }

        public Valor Valor
        {
            get
            {
                return _valor;
            }
            set
            {
                _valor = value;
            }
        }

        public int Id { get { return _id; } set { _id = value; } }

        public override string ToString()
        {
            return string.Format("{0} de {1}",_valor,_palo);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Carta otraCarta = obj as Carta;

            if (this.Valor > otraCarta.Valor)
                return 1;
            else if (this.Valor == otraCarta.Valor)
                return 0;
            else
                return -1;
        }
    }
}
