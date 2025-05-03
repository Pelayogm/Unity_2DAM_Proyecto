using System;
using Armeria;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class FPS_Selector : MonoBehaviour
    {
        public TextMeshProUGUI fpsSelectorTexto;
        
        public int [] fps = {30, 60, 90, 120, 144};
        public int posicionActual = DataUsuario.posicionSelector;
        public int fpsSeleccionados = DataUsuario.fps;


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
    }
}