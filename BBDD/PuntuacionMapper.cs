using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Funciones.BBDD
{
    //Mapper de Objeto de la BBDD a GameObject con de Unity.
    public class PuntuacionMapper : MonoBehaviour
    {
        public GameObject posicion;
        public GameObject puntuacionJugador;
        public GameObject nombreJugador;
        public GameObject armaJugador;

        public void setPuntuacion(string posicionInt, string puntuacionStr, string nombreJugadorStr, string armaJugadorStr)
        {
            string posText  = string.IsNullOrEmpty(posicionInt)     ? "1"             : posicionInt;
            string puntText = string.IsNullOrEmpty(puntuacionStr)   ? "sin registros" : puntuacionStr;
            string nomText  = string.IsNullOrEmpty(nombreJugadorStr) ? "no conocido"   : nombreJugadorStr;
            string armaText = string.IsNullOrEmpty(armaJugadorStr)   ? "sin arma"      : armaJugadorStr;

            // Asignación directa con null-propagation
            var txt = posicion?.GetComponent<TextMeshProUGUI>();
            if (txt != null) txt.text = posText + "º";

            txt = puntuacionJugador?.GetComponent<TextMeshProUGUI>();
            if (txt != null) txt.text = puntText;

            txt = nombreJugador?.GetComponent<TextMeshProUGUI>();
            if (txt != null) txt.text = nomText;

            txt = armaJugador?.GetComponent<TextMeshProUGUI>();
            if (txt != null) txt.text = armaText;
        }
    }
}