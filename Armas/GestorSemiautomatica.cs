using System;
using System.Collections;
using Armeria;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Armas
{
    public class GestorSemiautomatica : MonoBehaviour
    {
        [Header("Munición")]
        public GameObject proyectil;
        public Transform spawnPoint;

        [Header("Parámetros de disparo")]
        public float potencia = 20f;
        public float tiempoEnfriamiento = 0.2f;

        private float ultimoDisparo = -Mathf.Infinity;

        [Header("Cargador")]
        public int maxBalas = 10;
        private int balasRestantes;
        
        public float tiempoRecarga = 5f;
        private bool recargando = false;
        
        [Header("UI")]
        public Slider municionSlider;

        [Header("Cámara del jugador")]
        public Camera playerCamera;

        public static Action disparoUsuario;

        void Start()
        {
            //Copiamos las balas restantes del máximo.
            balasRestantes = maxBalas;
            if (municionSlider != null)
            {
                //Establecemos los valores del slider.
                municionSlider.minValue = 0f;
                municionSlider.maxValue = 1f;
                municionSlider.value = 1f;
            }
        }

        void Update()
        {
            // Mientras recarga o dispara la ráfaga, manejamos los sliders internamente.
            if (recargando)
                return;

            // Mientras no se recargue, el slider refleja la munición restante
            if (municionSlider != null)
            {
                municionSlider.value = balasRestantes / (float) maxBalas;
            }

            var tiempoPasado = Time.time - ultimoDisparo;

            //Disparamos al espacio y si el tiempo entre disparos ha pasado
            if (Input.GetKeyDown(KeyCode.Space) && tiempoPasado >= tiempoEnfriamiento)
            {
                if (balasRestantes > 0)
                {
                    Disparar();
                    balasRestantes--;
                    ultimoDisparo = Time.time;

                    //Si no quedan balas recargamos automaticamente
                    if (balasRestantes <= 0)
                        StartCoroutine(Recargar());
                }
            }
        }

        void Disparar()
        {
            //Generación de una copia de la bala y su punto de aparición.
            GameObject bala = Instantiate(proyectil, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = bala.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //La bala no tiene gravedad para que no caiga por los valores de la masa y la potencia es la velocidad de salida.
                rb.useGravity = false;
                rb.linearVelocity = playerCamera.transform.forward * potencia;
            }
            disparoUsuario?.Invoke();
        }

        private IEnumerator Recargar()
        {
            recargando = true;
            
            //Buscamos el nivel de cadencia del arma actual.
            var arma = DataUsuario.armaActual;
            var nivelCadencia = DataUsuario.nivelesCadencia[arma];
            
            //Calculamos el tiempo de recarga.
            var factorCadencia = 1f + nivelCadencia * 0.1f;
            
            //Y al dividirlo por el tiempo de enfriamiento, al ser él dividiendo cada vez más grande, el cociente es
            //cada vez más pequeño, por tanto, el tiempo se reduce.
            var tiempoAjustado = Mathf.Max(0.1f, tiempoRecarga / factorCadencia);

            //Hacemos un bucle para la espera de la recarga, que durara más o menos según el resultado de la division de la 
            //variable de tiempoAjustado.
            float duracion = 0f;
            while (duracion < tiempoAjustado)
            {
                duracion += Time.deltaTime;
                if (municionSlider != null)
                    //"Clampamos" el dato, es decir le ponemos un mínimo para que no se salga de los límites.
                    municionSlider.value = Mathf.Clamp01(duracion / tiempoRecarga);
                yield return null;
            }

            //Volvemos a copiar los valores de las balas.
            balasRestantes = maxBalas;
            if (municionSlider != null)
                //Ponemos el slider lleno otra vez.
                municionSlider.value = 1f;

            recargando = false;
        }
    }
}
