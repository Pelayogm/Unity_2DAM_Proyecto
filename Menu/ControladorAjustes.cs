using Armeria;
using Funciones.BBDD;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ControladorAjustes : MonoBehaviour
    {
        
        public void Reiniciar()
        {
            //Usamos el método DeleteAll(); del PlayerPrefs para asegurarnos que no queda nada.
            PlayerPrefs.DeleteAll();
            //Reiniciamos nuestra clase de Datos usando el método de ReiniciarDatos(); que pone todos los niveles y las armas
            //como si hubiéramos descargado el juego de nuevo.
            DataUsuario.ReiniciarDatos();
            //Borramos los datos de la BBDD con el método reiniciarDatos();
            BBDDManager.Instance.reiniciarDatos();
            //Recargamos la escena principal.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void addCreditos()
        {
            //Añadimos al PlayerPrefs mas créditos y guardamos datos.
            int creditosActuales = DataUsuario.creditos;
            creditosActuales += 500;
            DataUsuario.creditos = creditosActuales;
            DataUsuario.guardarDatos();
        }
    }

}