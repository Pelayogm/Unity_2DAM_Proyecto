using System;
using TMPro;
using UnityEngine;

namespace Funciones.Partida
{
    public class ContadorFPS : MonoBehaviour
    {
        public TextMeshProUGUI contadorFPSPartida;
        private float tiempoActualizacionFPS = 1f; 
        private float tiempo;
        private int contadorFPS;
        
        void Update()
        {
            tiempo += Time.deltaTime;
            contadorFPS++;

            if (tiempo >= tiempoActualizacionFPS)
            {
                int frameRate = Mathf.RoundToInt(contadorFPS / tiempo);
                contadorFPSPartida.text = frameRate.ToString() + " FPS";

                tiempo -= tiempoActualizacionFPS;
                contadorFPS = 0;
            }
        }
    }
}