using System;
using Armeria;
using TMPro;
using UnityEngine;

namespace Funciones.Partida
{
    public class PuntuacionManager : MonoBehaviour
    {
        private int _puntuacionPartida;
        private int _creditos;
        
        public TextMeshProUGUI puntuacionUI;
        public TextMeshProUGUI puntuacionFinalPopUp;
        public TextMeshProUGUI creditosPopUp;
        
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

        public void calcularCreditos()
        {
            int arma = DataUsuario.armaActual;
            int nivelActual = DataUsuario.nivelesSuerte[arma];
            _creditos = (_puntuacionPartida / 100) * nivelActual;
            DataUsuario.creditos += _creditos;
        }
        
        public void aumentarPuntuacion(int cantidad)
        {
            _puntuacionPartida += cantidad;
        }

        void Update()
        {
            puntuacionUI.text = _puntuacionPartida.ToString();
            puntuacionFinalPopUp.text = _puntuacionPartida.ToString();
            creditosPopUp.text = _creditos.ToString();
        }
    }
}