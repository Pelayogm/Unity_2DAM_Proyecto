using Funciones.Partida;
using TMPro;
using UnityEngine;

namespace Funciones 
{
    public class Temporizador : MonoBehaviour
    {
       [SerializeField] TextMeshProUGUI temporizador;
       [SerializeField] float tiempoRestante;
       private bool partidaParada = false;
       
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
               if (!partidaParada)
               {
                   pararPartida();
                   PuntuacionManager.Instance.calcularCreditos();
               }
               partidaParada = true;
               popUpFinal.SetActive(true);
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
       
       public void reanudarPartida()
       {
           Time.timeScale = 1;
           Cursor.visible = true;
           movimientoJugador.canMove = true;
       }
       
       public float getTiempoRestante()
       {
           return tiempoRestante;
       }
    }
}
