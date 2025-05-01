namespace Funciones.BBDD
{
    //Objeto de la BBDD.
    public class Puntuacion
    {
        public int idRegistro { get; set; }
        public int puntuacion { get; set; }
        public string nombreJugador { get; set; }
        public string nombreArmaUsada { get; set; }

        public Puntuacion(int idRegistro, int puntuacion, string nombreJugador, string nombreArmaUsada)
        {
            this.idRegistro = idRegistro;
            this.puntuacion = puntuacion;
            this.nombreJugador = nombreJugador;
            this.nombreArmaUsada = nombreArmaUsada;
        }
    }
}