using UnityEngine;
using UnityEngine.SceneManagement;

namespace Funciones.Partida
{
    public class VolverAlMenuManager : MonoBehaviour
    {

        public Temporizador temporizador;
        
        public void volverAlMenu()
        {
            SceneManager.LoadScene("Menu2");
            temporizador.reanudarPartida();
        }
    }
}