using System;
using Armeria;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class CambiarFondo : MonoBehaviour
    {
        [Header("UI y fondos")]
        public Sprite [] fondos;
        public Image fondoActual;
        
        [Header("Indice de fondos")]
        private int posicionActual = DataUsuario.posicionFondos;

        private void Awake()
        {
            DataUsuario.cargarDatos();
            posicionActual = DataUsuario.posicionFondos;
            fondoActual.sprite = fondos[posicionActual];
        }

        public void cambiarFondo()
        {
            if (posicionActual + 1 >= fondos.Length)
            {
                posicionActual = 0;
            }
            else
            {
                posicionActual++;
            }
            
            fondoActual.sprite = fondos[posicionActual];
            DataUsuario.posicionFondos = posicionActual;
            DataUsuario.guardarDatos();
        }
    }
}
