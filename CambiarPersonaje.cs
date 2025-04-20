using System;
using UnityEngine;

namespace Funciones
{
    public class CambiarPersonaje : MonoBehaviour
    {
        public GameObject Canon;
        public GameObject Fusil;
        
        public static Action cambioDePersonaje;
        
        void Start()
        {
            if (Canon != null && Fusil != null)
            {
                Canon.SetActive(true);
                Fusil.SetActive(false);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                cambiarPersonaje();
            }
        }

        void cambiarPersonaje()
        {
            if (Canon.activeSelf)
            {
                Canon.SetActive(false);
                Fusil.SetActive(true);
            }
            else
            {
                Canon.SetActive(true);
                Fusil.SetActive(false);
            }
            
            cambioDePersonaje?.Invoke();
        }
    }
}
