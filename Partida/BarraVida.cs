using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Funciones.Partida
{
    public class BarraVida : MonoBehaviour
    {
    
        public Slider slider;
        public Slider slider2;
        public float maxVida = 100f;
        public float vida;
        private float lerpSpeed = 0.05f;
    
        void Start()
        {
            vida = maxVida;
        
            //Valor de los slider porque según la vida el slider tiene que variar el rango máximo de funcionamiento
            slider.minValue = 0;
            slider.maxValue = maxVida;
            slider2.minValue = 0;
            slider2.maxValue = maxVida;
        }

        void Update()
        {
            if (slider.value != vida)
            {
                slider.value = vida;
            }
     
            if (slider.value != slider2.value)
            {
                slider2.value = Mathf.Lerp(slider2.value, vida, lerpSpeed);
            }   
        }

        public void Impactar(float danoRecibido)
        {
            vida -= danoRecibido;
            Update();
        }

        public void Curar(float cantidad)
        {
            vida = Mathf.Clamp(vida + cantidad, 0, maxVida);
            Update();
        }
    
    }

}