using Armeria;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ControladorAjustes : MonoBehaviour
    {
        
        public void Reiniciar()
        {
            PlayerPrefs.DeleteAll();
            DataUsuario.ReiniciarDatos();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void addCreditos()
        {
            int creditosActuales = DataUsuario.creditos;
            creditosActuales += 500;
            DataUsuario.creditos = creditosActuales;
            DataUsuario.guardarDatos();
            //Recargar la escena
        }
    }

}