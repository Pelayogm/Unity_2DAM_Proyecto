using UnityEngine;
using UnityEngine.SceneManagement;

namespace Funciones.Partida
{
    public class VolverAlMenuManager : MonoBehaviour
    {
        public void volverAlMenu()
        {
            SceneManager.LoadScene("Menu2");
        }
    }
}