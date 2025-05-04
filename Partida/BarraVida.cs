using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Funciones.Partida
{
    public class BarraVida : MonoBehaviour
    {
    
        [Header("UI Slider")]
        public Slider slider;
        public Slider slider2;
        
        [Header("Valores de la barra de vida")]
        public float maxVida = 100f;
        public float vida;
        private float lerpSpeed = 0.05f;
    
        void Start()
        {
            vida = maxVida;
        
            //Valor de los slider porque según la vida, el slider tiene que variar el rango máximo de funcionamiento.
            slider.minValue = 0;
            slider.maxValue = maxVida;
            slider2.minValue = 0;
            slider2.maxValue = maxVida;
        }

        //Actualización de la barra de vida (hemos quitado vida al enemigo o se ha curado)
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

        //Actualizamos la variable de vida con el daño y forzamos la actualización de la barra de vida.
        public void Impactar(float danoRecibido)
        {
            vida -= danoRecibido;
            Update();
        }

        //Actualizamos la variable de vida con la curación provocada por el comportamiento aleatorio
        //y forzamos la actualización de la barra de vida.
        public void Curar(float cantidad)
        {
            vida = Mathf.Clamp(vida + cantidad, 0, maxVida);
            Update();
        }

        //Se asegura que la barra de vida tenga sus valores en 0 y el maximo de Vida posible y restaura los mismos valores de los sliders al maximo
        public void resetBarraVidaEnemigo(float cantidad)
        {
            vida = Mathf.Clamp(cantidad, 0, maxVida);
            slider.value = vida;
            slider2.value = vida;
        }
    
    }

}