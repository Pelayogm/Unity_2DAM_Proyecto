using System;
using TMPro;
using UnityEngine;

namespace Funciones.Partida
{
    public class PuntuacionManager : MonoBehaviour
    {
        private int _puntuacionPartida;
        public TextMeshProUGUI puntuacionUI;
        public TextMeshProUGUI puntuacionFinalPopUp;
        
        public static PuntuacionManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void aumentarPuntuacion(int cantidad)
        {
            _puntuacionPartida += cantidad;
        }

        void Update()
        {
            puntuacionUI.text = _puntuacionPartida.ToString();
            puntuacionFinalPopUp.text = _puntuacionPartida.ToString();
        }
    }
}