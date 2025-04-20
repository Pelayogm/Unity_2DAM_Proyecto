using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public void MostrarPantalla() {
        print("Hola");
    }

    public void CambiarVista()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void Salir()
    {
        Application.Quit();
    }
}
