using System;
using Armeria;
using TMPro;
using UnityEngine;

namespace Funciones.Partida
{
    public class PuntuacionManager : MonoBehaviour
    {
        [Header("Valores de puntuacion y creditos que se calculan")]
        private int _puntuacionPartida;
        private int _creditos;
        
        [Header("UI Pop-Up")]
        public TextMeshProUGUI puntuacionUI;
        public TextMeshProUGUI puntuacionFinalPopUp;
        public TextMeshProUGUI creditosPopUp;
        
        //Singleton para evitar varias inserciones de datos.
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

        //Los créditos que se van a dar por la partida
        public void calcularCreditos()
        {
            //Se cogen los niveles de suerte del arma usada.
            int arma = DataUsuario.armaActual;
            int nivelActual = DataUsuario.nivelesSuerte[arma];
            
            //Se dividen los puntos entre 100 y el resultante se multiplica por la suerte.
            _creditos = (_puntuacionPartida / 100) * nivelActual;
            print(_creditos);
            
            //Los mismos se añaden al usuario
            DataUsuario.creditos += _creditos;
        }
        
        //Suma puntos recibidos debido a impactar o eliminar un enemigo.
        public void aumentarPuntuacion(int cantidad)
        {
            _puntuacionPartida += cantidad;
        }

        //Bucle que actualiza los textos para que en partida se reflejen los puntos actuales y se guardan los datos.
        void Update()
        {
            puntuacionUI.text = _puntuacionPartida.ToString();
            puntuacionFinalPopUp.text = _puntuacionPartida.ToString();
            creditosPopUp.text = _creditos.ToString();
            DataUsuario.guardarDatos();
        }
    }
}