using System;
using Armeria;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class FPS_Selector : MonoBehaviour
    {
        [Header("Texto UI que vemos en Ajustes")]
        public TextMeshProUGUI fpsSelectorTexto;
        
        [Header("Variables de los FPS, indices y valor actual.")]
        public int [] fps = {30, 60, 90, 120, 144};
        public int posicionActual = DataUsuario.posicionSelector;
        public int fpsSeleccionados = DataUsuario.fps;


        //Avanzamos en el array usando los índices y actualizamos para mostrar el texto adaptado al índice del array (60 - pos 1) - (90 - pos 2)
        public void Avanzar()
        {
            if (posicionActual + 1 >= fps.Length)
            {
                posicionActual = 0;
            }
            else
            {
                posicionActual++;
            }

            Update();
        }
        
        //Exactamente igual que Avanzar() pero reduciendo índices.
        public void Retroceder()
        {
            if (posicionActual - 1 < 0)
            {
                posicionActual = 0;
            } else
            {
                posicionActual--;
            }

            Update();
        }

        //Al confirmar, ponemos el vSync a 0.
        //Ponemos al juego a funcionar a esos FPS como máximo y guardamos los datos (Posición y FPS) para cargarlos al volver a abrir el menú.
        public void ConfirmarFPS()
        {
            fpsSeleccionados = fps[posicionActual];
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = fpsSeleccionados;
            DataUsuario.posicionSelector = posicionActual;
            DataUsuario.fps = fpsSeleccionados;
            DataUsuario.guardarDatos();
        }

        void Update()
        {
            fpsSelectorTexto.text = (fps[posicionActual].ToString()); 
        }
        
        //Cargamos los últimos datos guardados y ponemos el juego a los FPS últimos FPS que estableció el usuario,
        //Y ponemos los índices y el texto adaptados a las posiciones que hemos cargado del PlayerPrefs.
        
        void Awake() 
        {
            DataUsuario.cargarDatos();
            
            posicionActual = DataUsuario.posicionSelector;
            fpsSeleccionados = DataUsuario.fps;
            Application.targetFrameRate = fpsSeleccionados;
            
            fpsSelectorTexto.text = fps[posicionActual].ToString();
        }
    }
}