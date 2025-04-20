using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Armeria
{
    public class Armeria : MonoBehaviour
    {
        public String [] nombresArmas;
        public String [] descripcionArmas;
        public TextMeshProUGUI nombreArma;
        public TextMeshProUGUI descripcionArma;
        public Button siguiente;
        public Button reverse;
        public Button salir;

        void Awake()
        {
            if (nombresArmas != null && descripcionArmas != null)
            {
                nombreArma.text = nombresArmas[0];
                descripcionArma.text = descripcionArmas[0];
            }
            
        }

        void Update()
        {
            
        }
    }
}
