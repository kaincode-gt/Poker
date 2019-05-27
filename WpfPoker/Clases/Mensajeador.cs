using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPoker
{
    class Mensajeador
    {
        public int MaximoNumeroMensajes = 5;
        public List<string> ListaMensajes { get; set; }


        public Mensajeador()
        {
            ListaMensajes = new List<string>();
        }

        /// <summary>
        /// Costructor basado en el número máximo de mensajes almacenados 
        /// </summary>
        /// <param name="numMensajes">Número máximo de mensajes</param>
        public Mensajeador(int numMensajes)
        {
            MaximoNumeroMensajes = numMensajes;
            ListaMensajes = new List<string>();
        }

        public void AñadirMensaje(string mensaje)
        {
            if (ListaMensajes.Count >= MaximoNumeroMensajes)
            {
                ListaMensajes.RemoveAt(0);
            }
            ListaMensajes.Add(mensaje);
        }

        public string ListaEnCadena
        {
            get
            {
                string resultado = string.Empty;
                foreach (string mensaje in ListaMensajes)
                    resultado += mensaje + "\n";
                return resultado;
            }
        }
    }
}
