using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPSimFinal
{

    public class Simulacion
    {

        public int numeroSimulacion { get; set; }
        public double reloj { get; set; }
        public string evento { get; set; }

        public double proxFinAfuera { get; set; }

        public double proxFinDentro { get; set; }

        public int cantidadDentro { get; set; }

        public int cantidadAfuera { get; set; }

        public int exitos { get; set; }

        public int acExitos { get; set; }

        public double probabilidadAfuera { get; set; }
        public int acumulador { get; set; }

        public double promedioDentro { get; set; }

        public Simulacion() { }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
