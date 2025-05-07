using System;
using UnityEngine;
using UnityEngine.UI;

namespace Funciones.Menu
{
    public class VolumenManager : MonoBehaviour
    {
        
        [SerializeField] private Slider volumenSlider;

        void Start()
        {
            if (PlayerPrefs.HasKey("volumenMusica"))
            {
              cargarVolumenMusica();  
            }
            else
            {
                PlayerPrefs.SetFloat("volumenMusica", 1);
                cargarVolumenMusica();
            }
        }
        
        public void cambiarVolumenMusica()
        {
            AudioListener.volume = volumenSlider.value;
            PlayerPrefs.SetFloat("volumenMusica", volumenSlider.value);
        }
        
        public void cargarVolumenMusica()
        {
            volumenSlider.value = PlayerPrefs.GetFloat("volumenMusica");
        }
    }
}