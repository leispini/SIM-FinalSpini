using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPSimFinal
{
    public class Calculo
    {
        //Defino las variables necesarias para la clase cálculo
        // Es decir, para trabajar con la fila anterior y la fila actual
        public Simulacion actual;
        public Simulacion anterior;
        public Perrito perrito;

        public Form1 pantallaSimulacion;

        public string evento;


        //Definimos el random y las listas
        public Random rnd = new Random();
        public List<Perrito> dentro;
        public List<Perrito> afuera;


        //Creo el constructor de la clase calculo
        public Calculo(Form1 pantalla) {
            actual = new Simulacion();
            anterior = new Simulacion();
            dentro = new List<Perrito>();
            afuera = new List<Perrito>();
            pantallaSimulacion = pantalla;
            simular();

        }

        //Este método lo uso en el constructor para inciar la simulación y
        //tener en cuenta el tema de los desde y hasta
        public void simular() {
        
            //Se calcula cuando los perritos van a salir del canasto
            for(int i = 0; i < pantallaSimulacion.cantPerritos; i++) {
                Perrito perro = new Perrito();
                perro.proxFinDentro = - pantallaSimulacion.media * Math.Log(1 - rnd.NextDouble());
                dentro.Add(perro);
            }

            anterior.cantidadDentro = pantallaSimulacion.cantPerritos;
            
            anterior.acumulador = pantallaSimulacion.cantPerritos;
            anterior.promedioDentro = pantallaSimulacion.cantPerritos;
            anterior.proxFinDentro = determinarProxFinDentro();

           
            pantallaSimulacion.cargarFila(anterior);
                
            

            for (int i = 0; i<pantallaSimulacion.cantSimulaciones; i++) 
            {
                calcularSimulacion();

                if (i >= pantallaSimulacion.desde -1 && i < pantallaSimulacion.hasta)
                {
                    pantallaSimulacion.cargarFila(anterior);
                }
            }

            if (pantallaSimulacion.cantSimulaciones != pantallaSimulacion.hasta)
            {
                pantallaSimulacion.cargarFila(anterior);
            }

        }

        //Este método me permite calcular la simulacion propiamente dicha
        public void calcularSimulacion() {
            actual = new Simulacion();
            actual.numeroSimulacion = anterior.numeroSimulacion + 1;
            calcularReloj();
            determinarEvento();
            calcularEvento();
            calcularProbabilidadAfuera();
            calcularPromedio();
            anterior = (Simulacion)actual.Clone();

        }

        //Este método me permite determinar el próximo fin afuera.
        //Si la lista "afuera" no tiene elementos, devuelve un 0
        //Si la lista tiene elementos, se define que perrito ENTRA primero al canasto en base a "el mínimo valor"
        //En simples palabras, se trata de una lista que se actualiza constantemente y devuelve siempre el valor mínimo
        public double determinarProxFinAfuera() {

            if (afuera.Count == 0) { return 0; }

            //definimos que perrito entra primero
            double min = afuera.ElementAt<Perrito>(0).proxFinAfuera;
            foreach (var perro in afuera)
            {
                min = Math.Min(min, perro.proxFinAfuera);
            }
            return min;
        }

        //Este método me permite determinar el próximo fin dentro.
        //Si la lista "dentro" no tiene elementos, devuelve un 0
        //Si la lista tiene elementos, se define que perrito SALE primero del canasto en base a "el mínimo valor"
        //En simples palabras, se trata de una lista que se actualiza constantemente y devuelve siempre el valor mínimo
        public double determinarProxFinDentro() {

            if (dentro.Count == 0) { return 0; }

            //definimos que perrito sale primero
            double min = dentro.ElementAt<Perrito>(0).proxFinDentro;
            foreach(var perro in dentro)
                {
                    min = Math.Min(min, perro.proxFinDentro);
                }
            return min;
        }

        //Este método va calculando el reloj
        public void calcularReloj()
        {
            //Si alguno de los eventos es "0", o sea que no tiene calculado un "proxFin", el proximo evento
            //va a ser el que tenga un valor (que evidentemente es distinto de 0)
            if (anterior.proxFinAfuera == 0 || anterior.proxFinDentro == 0)
            {
                actual.reloj = Math.Max(anterior.proxFinAfuera,anterior.proxFinDentro);
                return;
            }

            actual.reloj = Math.Min(anterior.proxFinAfuera, anterior.proxFinDentro);

        }

        //Este método me permite determinar cuál es el evento actual
        public void determinarEvento() 
        {
            if (actual.reloj == anterior.proxFinDentro)
            {
                evento = "FinDentro";
                actual.evento = evento;
                return;
            }

            evento = "FinAfuera";
            actual.evento = evento;
           
        }

        //Este método me permite, en base al evento actual, actualizar las listas de los perros que están dentro y afuera.
        //Si se trata de un "FinDentro", el perro que SALE se elimina de la lista de perros que estan dentro y se le calcula
        //su proximo fin afuera y se lo añade a la lista de perros que estan afuera

        //Si se trata de un "FinAfuera", el perro que ENTRA se elimina de la lista de perros que estan afuera y se le calcula
        //su proximo fin dentro y se lo añade a la lista de perros que estan dentro

        //En ambos casos, se actualizan las cantidades de perros que estan afuera y dentro
        public void calcularEvento()
        {

            if (evento == "FinDentro")
            { 

                Perrito perroQueSale;
                foreach(var perro in dentro)
                {
                    if (perro.proxFinDentro == actual.reloj) 
                    {

                        perroQueSale = perro;
                        dentro.Remove(perro);
                        perroQueSale.proxFinAfuera = actual.reloj + ( pantallaSimulacion.a + rnd.NextDouble() * (pantallaSimulacion.b - pantallaSimulacion.a));
                        afuera.Add(perroQueSale);
                        break;
                    }
                }

                actual.proxFinAfuera = determinarProxFinAfuera();
                actual.proxFinDentro = determinarProxFinDentro();
                actual.cantidadDentro = anterior.cantidadDentro - 1;
                actual.cantidadAfuera = anterior.cantidadAfuera + 1;
                return;

            }

            Perrito perroQueEntra;
            foreach (var perro in afuera)
            {
                if (perro.proxFinAfuera == actual.reloj)
                {
                    perroQueEntra = perro;
                    afuera.Remove(perro);
                    perroQueEntra.proxFinDentro = actual.reloj + (-pantallaSimulacion.media * Math.Log(1 - rnd.NextDouble()));
                    dentro.Add(perroQueEntra);
                    break;
                }
            }

            actual.proxFinAfuera = determinarProxFinAfuera();
            actual.proxFinDentro = determinarProxFinDentro();
            actual.cantidadDentro = anterior.cantidadDentro + 1;
            actual.cantidadAfuera = anterior.cantidadAfuera - 1;
            return;

        }

        //Este método me permite realizar el cálculo para responder a la primer pregunta
        //que dice "¿Cuál es la probabilidad de que más perritos esten afuera que dentro de su canasto
        //en cualquier momento después de 2 horas?"
        public void calcularProbabilidadAfuera()
        {
            if (actual.cantidadAfuera > actual.cantidadDentro)
            {
                actual.exitos = 1;
            }
            else
            { 
                actual.exitos = 0;
            }

            actual.acExitos = anterior.acExitos + actual.exitos;
            actual.probabilidadAfuera = (double) actual.acExitos / actual.numeroSimulacion;


            //actual.acCantAfuera = anterior.acCantAfuera + actual.cantidadAfuera;
            //actual.probabilidadAfuera = (double) actual.acCantAfuera / (actual.numeroSimulacion * pantallaSimulacion.cantPerritos );
        }

        //Este método me permite realizar el cálculo para responder a la segunda pregunta
        //que dice "¿Cuántos cachorros estarán en promedio en el canasto?"
        public void calcularPromedio()
        {
            
            actual.acumulador = anterior.acumulador + actual.cantidadDentro;
            //actual.promedioDentro = (double )actual.acumulador / actual.numeroSimulacion;
            if (actual.numeroSimulacion == 1)
            {
                actual.promedioDentro = (double)(((anterior.promedioDentro) + actual.cantidadDentro) / 2);
            }
            else
            {
                actual.promedioDentro = (double)(((anterior.promedioDentro * (actual.numeroSimulacion)) + actual.cantidadDentro) / (actual.numeroSimulacion + 1));
            }
        }
       
    }

}
