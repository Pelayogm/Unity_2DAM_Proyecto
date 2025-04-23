using System;
using Armeria;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ControladorMenuCentral : MonoBehaviour
    {
        public TMP_Text textoMonedas;

        void Start()
        {
            monedasUsuarioMenu();
        }

        public void monedasUsuarioMenu()
        {
            DataUsuario.cargarDatos();
            int creditosActuales = DataUsuario.creditos;
            if (creditosActuales > 10000)
            {
                textoMonedas.text = "9999";
            }
            else
            {
                textoMonedas.text = creditosActuales.ToString();
            }
        }
        
        public void cerrarJuego()
        {
            Application.Quit();
        }

        public void abrirArmeria()
        {
            SceneManager.LoadScene("Armeria_definitiva");
        }

        public void empezarPartida()
        {
            abrirArmeria();
        }
        
        private void Update()
        {
            textoMonedas.text = DataUsuario.creditos.ToString();
        }
    }

}