using System;
using Funciones;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Armas
{
    public class MostrarNombre : MonoBehaviour
    {
        [Header("IU")]
        public GameObject[] armas;
        public String[] nombreArmas;
        public TextMeshProUGUI textoPantalla;
        
        void Awake()
        {
            if (textoPantalla == null)
            {
                return;
            }
            else
            {
                textoPantalla.text = nombreArmas[0];
            }
            ActualizarTexto();
        }
        
        void OnEnable()
        {
            CambiarPersonaje.cambioDePersonaje += ActualizarTexto;
        }

        void OnDisable()
        {
            CambiarPersonaje.cambioDePersonaje -= ActualizarTexto;
        }

        void ActualizarTexto()
        {
            for (int i = 0; i < armas.Length; i++)
            {
                if (armas[i].activeSelf)
                {
                    textoPantalla.text = nombreArmas[i];
                    return;
                }
            }
        }
    }
}
