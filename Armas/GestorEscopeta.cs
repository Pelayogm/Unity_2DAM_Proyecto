using System;
using System.Collections;
using Armeria;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Armas
{
    public class GestorEscopeta : MonoBehaviour
    {
        [Header("Munición")]
        public GameObject proyectil;
        public Transform spawnPoint;

        [Header("Parámetros de disparo")]
        public float potencia = 20f;
        public float tiempoEnfriamiento = 0.8f;

        [Header("Cargador")]
        public int maxCartuchos = 3;
        private int cartuchosRestantes;
        public float tiempoRecarga = 5f;
        private bool recargando = false;

        [Header("UI")]
        public Slider municionSlider;

        [Header("Cámara del jugador")]
        public Camera playerCamera;

        public static Action disparoUsuario;

        private float ultimoDisparo = -Mathf.Infinity;

        void Start()
        {
            //Copiamos las balas restantes del maximo
            cartuchosRestantes = maxCartuchos;
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
            if (recargando) return;

            // Actualiza la UI de munición
            if (municionSlider != null)
                municionSlider.value = cartuchosRestantes / (float)maxCartuchos;

            // Disparo al pulsar Espacio
            if (Input.GetKeyDown(KeyCode.Space) &&
                Time.time - ultimoDisparo >= tiempoEnfriamiento &&
                cartuchosRestantes > 0)
            {
                Disparar();
            }
        }

        void Disparar()
        {
            ultimoDisparo = Time.time;
            cartuchosRestantes--;
            
            //Generación de una copia de la bala y su punto de aparición
            Vector3 spawnPos = spawnPoint.position + playerCamera.transform.forward * 0.5f;
            GameObject bala = Instantiate(proyectil, spawnPos, spawnPoint.rotation);
            
            // Aplicar velocidad directa sin gravedad
            Rigidbody rb = bala.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //La bala no tiene gravedad para que no caiga por los valores de la masa y la potencia es la velocidad de salida
                rb.useGravity = false;
                rb.linearVelocity = playerCamera.transform.forward * potencia;
            }
            
            disparoUsuario?.Invoke();

            // Iniciar recarga automática si se acaba el cargador
            if (cartuchosRestantes <= 0)
                StartCoroutine(recargar());
        }

        private IEnumerator recargar()
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

            //Volvemos a copiar los valores de las balas
            cartuchosRestantes = maxCartuchos;
            if (municionSlider != null)
                //Ponemos el slider lleno otra vez
                municionSlider.value = 1f;

            recargando = false;
        }
    }
}
