using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class JugadorIA : Jugador
    {
        public JugadorIA(string nombre) : base(nombre)
        {

        }

        public override Accion PedirAccion()
        {
            System.Threading.Thread.Sleep(1000);
            Random rd = new Random();
            int resultado = rd.Next(Enum.GetValues(typeof(Accion)).Length);
            return (Accion)resultado;
        }

        public override List<int> PedirDescartes()
        {
            System.Threading.Thread.Sleep(1000);
            Random rd = new Random();

            List<int> descartadas = new List<int>();
            int numDescartes = rd.Next(Mano.Cartas.Count());
            for (int i = 0; i < numDescartes; i++)
            {
                int indice = rd.Next(Mano.Cartas.Count());
                if (descartadas.Contains(indice))
                {
                    descartadas.Add(indice);
                }
            }
            return descartadas;
        }

        public override int PedirApuesta(List<int> apuestas)
        {
            
            Random rd = new Random();
            return apuestas[rd.Next(apuestas.Count)];
        }
    }
}
