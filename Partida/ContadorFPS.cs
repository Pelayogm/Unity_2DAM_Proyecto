using System;
using TMPro;
using UnityEngine;

namespace Funciones.Partida
{
    public class ContadorFPS : MonoBehaviour
    {
        [Header("UI FPS")]
        public TextMeshProUGUI contadorFPSPartida;
        
        [Header("Valores de actualizacion de FPS")]
        private float tiempoActualizacionFPS = 1f; 
        private float tiempo;
        private int contadorFPS;
        
        void Update()
        {
            tiempo += Time.deltaTime;
            contadorFPS++;
        
            //Si ya ha pasado o es el tiempo para volver a calcular los fps lo volvemos a hacer y actualizamos el texto
            if (tiempo >= tiempoActualizacionFPS)
            {
                int frameRate = Mathf.RoundToInt(contadorFPS / tiempo);
                contadorFPSPartida.text = frameRate.ToString() + " FPS";

                //Y volvemos a poner todo como antes y esperamos al siguiente ciclo para volver a calcular los fps.
                tiempo -= tiempoActualizacionFPS;
                contadorFPS = 0;
            }
        }
    }
}