using UnityEngine;
using UnityEngine.SceneManagement;

namespace Armeria
{
    public class ControladorMenu : MonoBehaviour
    {
        public void cerrarMenu()
        {
            SceneManager.LoadScene("Menu2");
        }

        public void empezarPartida()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
