using System;
using System.Collections;
using Armeria;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Armas
{
    public class GestorAutomatico : MonoBehaviour
    {
        [Header("Munición")]
        public GameObject proyectil;
        public Transform spawnPoint;

        [Header("Parámetros de disparo")]
        public float potencia = 20f;
        public float cadencia = 10f;

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
        private float siguienteDisparo = 0f;

        void Start()
        {
            //Cogemos el componente AudioSource
            audioSource = GetComponent<AudioSource>();
            //Copiamos las balas restantes del maximo
            balasRestantes = maxBalas;
            if (municionSlider != null)
            {
                //Establecemos los valores del slider
                municionSlider.minValue = 0f;
                municionSlider.maxValue = 1f;
                municionSlider.value = 1f;
            }
        }

        void Update()
        {
            // Si está recargando, dejamos que la coroutine actualice el slider
            if (recargando)
            {
                return;
            }

            // Mientras no recarga, el slider muestra balas restantes
            if (municionSlider != null)
            {
                municionSlider.value = balasRestantes / (float)maxBalas;
            }

            // Disparo automático manteniendo el Espacio
            if (Input.GetKey(KeyCode.Space) && Time.time >= siguienteDisparo)
            {
                //Mientras queden balas entramos en esta condición
                if (balasRestantes > 0)
                {
                    Disparar();
                    balasRestantes--;
                    siguienteDisparo = Time.time + 1f / cadencia;

                    // Si se acaban, iniciamos la recarga
                    if (balasRestantes <= 0)
                    {
                        StartCoroutine(Recargar());  
                    }
                        
                }
            }
        }

        void Disparar()
        {
            //Generación de una copia de la bala y su punto de aparición
            GameObject bala = Instantiate(proyectil, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = bala.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //La bala no tiene gravedad para que no caiga por los valores de la masa y la potencia es la velocidad de salida
                rb.useGravity = false;
                rb.linearVelocity = playerCamera.transform.forward * potencia;
            }
            //Le ponemos a AudioSource el sonido
            audioSource.clip = sonidoDisparo;
            //Ejecutamos el sonido
            audioSource.PlayOneShot(audioSource.clip);
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
            //variable de tiempoAjustado
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

            //Volvemos a copiar los valores de las balas
            balasRestantes = maxBalas;
            if (municionSlider != null)
                //Ponemos el slider lleno otra vez
                municionSlider.value = 1f;

            recargando = false;
            // Permitir disparar de nuevo
            siguienteDisparo = Time.time;
        }
    }
}
