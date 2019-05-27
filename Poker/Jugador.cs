using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public enum Accion
    {
        SUBIR,
        VER,
        ABANDONAR
    }

    public class Jugador
    {
        protected string _nombre;
        protected int _saldo;
        private bool _retirado;



        public Mano Mano { get; set; }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Jugador()
        {
        }

        /// <summary>
        /// Constructor basado en nombre
        /// </summary>
        /// <param name="nombre"></param>
        public Jugador(string nombre)
        {
            _nombre = nombre;
            _saldo = 300;
        }

        public int Saldo
        {
            get
            {
                return _saldo;
            }
            set
            {
                _saldo = value;
            }
        }

        public string Nombre
        {
            get
            {
                return _nombre;
            }
            set
            {
                _nombre = value;
            }
        }

        public virtual int Apostar(int cantidad)
        {
            Saldo -= cantidad;
            return cantidad;
        }

        public virtual void AgregarSaldo(int cantidad)
        {
            Saldo += cantidad;
        }

        public bool Retirado
        {
            get { return _retirado; }
            set { _retirado = value; }
        }

        public virtual Accion PedirAccion()
        {
            return Accion.ABANDONAR;
        }

        public virtual List<int> PedirDescartes()
        {
            return new List<int>();
        }

        public override string ToString()
        {
            string resultado = string.Format($"Jugador: {Nombre.ToUpper()}, Saldo: {Saldo}", Nombre, Saldo);
            if (Mano != null)
            {
                resultado += "\n" +Mano;
            }
            return resultado;
        }

        public virtual int PedirApuesta(List<int> apuestas)
        {
            return 0;
        }

        public bool Bancarrota()
        {
            return Saldo<=0;
        }

    }
}
