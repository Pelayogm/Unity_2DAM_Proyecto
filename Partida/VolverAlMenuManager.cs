using Armeria;
using Funciones.BBDD;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Funciones.Partida
{
    public class VolverAlMenuManager : MonoBehaviour
    {
        public Temporizador temporizador;

        public TMP_InputField nombreJugador;
        public TextMeshProUGUI puntuacionFinalPopUp;

        public void volverAlMenu()
        {
            SceneManager.LoadScene("Menu2");
            DataUsuario.armaActual = 0;
            temporizador.reanudarPartida();
        }
        
        public void guardarDatosYSalir()
        {
            
            int _posicionArmaUsada = DataUsuario.armaActual;
            string _armaUsada = DataUsuario.nombresArmas[_posicionArmaUsada];
            
            //Herramientas de debugging
            print("Arma usada:" + _posicionArmaUsada);
            print("Nombre" + _armaUsada);
            
            if (string.IsNullOrWhiteSpace(nombreJugador.text))
            {
                nombreJugador.text = "Jugador Anónimo";
            }
            
            BBDDManager.Instance.insertarPuntuacion(int.Parse(puntuacionFinalPopUp.text), nombreJugador.text, _armaUsada);
            volverAlMenu();
        }

    }
}