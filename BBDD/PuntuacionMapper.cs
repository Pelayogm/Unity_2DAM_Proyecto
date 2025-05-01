using UnityEngine;
using UnityEngine.UI;

namespace Funciones.BBDD
{
    public class PuntuacionMapper : MonoBehaviour
    {
        public GameObject posicion;
        public GameObject puntuacionJugador;
        public GameObject nombreJugador;
        public GameObject armaJugador;

        public void setPuntuacion(string posicionInt, string puntuacionStr, string nombreJugadorStr, string armaJugadorStr)
        {
            // Valores por defecto si vienen null o vacíos
            string posText  = string.IsNullOrEmpty(posicionInt)     ? "1"             : posicionInt;
            string puntText = string.IsNullOrEmpty(puntuacionStr)   ? "sin registros" : puntuacionStr;
            string nomText  = string.IsNullOrEmpty(nombreJugadorStr) ? "no conocido"   : nombreJugadorStr;
            string armaText = string.IsNullOrEmpty(armaJugadorStr)   ? "sin arma"      : armaJugadorStr;

            // Asignación directa con null-propagation
            var txt = posicion?.GetComponent<Text>();
            if (txt != null) txt.text = posText + "º";

            txt = puntuacionJugador?.GetComponent<Text>();
            if (txt != null) txt.text = puntText;

            txt = nombreJugador?.GetComponent<Text>();
            if (txt != null) txt.text = nomText;

            txt = armaJugador?.GetComponent<Text>();
            if (txt != null) txt.text = armaText;
        }
    }
}