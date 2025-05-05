using System;
using System.Collections;
using Armeria;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Armas
{
    public class GestorRafaga : MonoBehaviour
    {
        [Header("Munición")]
        public GameObject proyectil;
        public Transform spawnPoint;

        [Header("Parámetros de disparo")]
        public float potencia = 20f;
        public float tiempoEntreRafagas = 0.5f;
        public float tiempoEntreBalas = 0.1f;

        [Header("Cargador")]
        public int maxBalas = 15;
        private int balasRestantes;
        public float tiempoRecarga = 5f;
        
        [Header("UI")]
        public Slider municionSlider;

        [Header("Cámara del jugador")]
        public Camera playerCamera;
        
        [Header("Sonidos")] 
        [SerializeField] private AudioClip sonidoDisparo;
        [SerializeField] private AudioClip sonidoRecarga;
        private AudioSource audioSource;

        public static Action disparoUsuario;

        private bool recargando = false;
        private bool disparandoRafaga = false;
        private float ultimoRafaga = -Mathf.Infinity;

        void Start()
        {
            //Cogemos el componente AudioSource
            audioSource = GetComponent<AudioSource>();
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
            if (recargando || disparandoRafaga)
                return;

            //El Slider refleja las ráfagas restantes
            if (municionSlider != null)
                municionSlider.value = balasRestantes / (float) maxBalas;

            // Disparo de la ráfaga.
            if (Input.GetKeyDown(KeyCode.Space) && Time.time - ultimoRafaga >= tiempoEntreRafagas && balasRestantes > 0)
            {
                StartCoroutine(Disparar());
            }
        }

        private IEnumerator Disparar()
        {
            disparandoRafaga = true;

            //Bucle por los tiros de la ráfaga, es decir realizamos el mismo disparo por tantas balas que tenga una ráfaga.
            int burstSize = Mathf.Min(3, balasRestantes);
            for (int i = 0; i < burstSize; i++)
            {
                
                //Generación de una copia de la bala y su punto de aparición.
                var bala = Instantiate(proyectil, spawnPoint.position, spawnPoint.rotation);
                var rb = bala.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    //La bala no tiene gravedad para que no caiga por los valores de la masa y la potencia es la velocidad de salida.
                    rb.useGravity = false;
                    rb.linearVelocity = playerCamera.transform.forward * potencia;
                }
                
                //Le ponemos a AudioSource el sonido
                audioSource.clip = sonidoDisparo;
                //Ejecutamos el sonido
                audioSource.PlayOneShot(audioSource.clip);
                disparoUsuario?.Invoke();

                balasRestantes--;
                if (municionSlider != null)
                    municionSlider.value = balasRestantes / (float)maxBalas;

                // Esperar antes del siguiente tiro dentro de la ráfaga.
                yield return new WaitForSeconds(tiempoEntreBalas);
            }

            ultimoRafaga = Time.time;
            disparandoRafaga = false;

            // Si agotamos la munición, empezamos la recarga.
            if (balasRestantes <= 0)
                StartCoroutine(Recargar());
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
            //Le ponemos a AudioSource el sonido
            audioSource.clip = sonidoRecarga;
            //Ejecutamos el sonido
            audioSource.PlayOneShot(audioSource.clip);

            //Volvemos a copiar los valores de las balas.
            balasRestantes = maxBalas;
            if (municionSlider != null)
                //Ponemos el slider lleno otra vez.
                municionSlider.value = 1f;

            recargando = false;
        }
    }
}
