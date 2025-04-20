using Funciones.Partida;
using TMPro;
using UnityEngine;

namespace Funciones 
{
    public class Temporizador : MonoBehaviour
    {
       [SerializeField] TextMeshProUGUI temporizador;
       [SerializeField] float tiempoRestante;
       
       //PopUp de la victoria
       public GameObject popUpFinal;
       
       //Script del movimiento del jugador
       public Movimiento movimientoJugador;

       void Update()
       {

           if (tiempoRestante > 0)
           {
               tiempoRestante -= Time.deltaTime;
           } else if (tiempoRestante <= 0)
           {
               tiempoRestante = 0;
               temporizador.text = "TIEMPO";
               temporizador.color = Color.red;
               pararPartida();
               popUpFinal.SetActive(true);
               PuntuacionManager.Instance.calcularCreditos();
           }
           
           int minutes = Mathf.FloorToInt(tiempoRestante / 60);
           int seconds = Mathf.FloorToInt(tiempoRestante % 60);
           temporizador.text = string.Format("{0:00}:{1:00}", minutes, seconds);
       }
       void pararPartida()
       {
           Time.timeScale = 0;
           Cursor.visible = true;
           Cursor.lockState = CursorLockMode.None;
           movimientoJugador.canMove = false;
           
        }
       
    }
}
