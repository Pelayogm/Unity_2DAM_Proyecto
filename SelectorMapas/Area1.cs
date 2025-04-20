using UnityEngine;
using UnityEngine.SceneManagement;

namespace Funciones.SelectorMapas
{
    public class Area1 : MonoBehaviour
    {
        public void cambiarEscena()
        {
            SceneManager.LoadScene("Area1");
        }
    }
}