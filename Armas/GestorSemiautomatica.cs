using System;
using System.Collections;
using Armeria;
using UnityEngine;
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
        private bool isRecargando = false;

        [Header("UI")]
        public Slider ammoSlider;

        [Header("Cámara del jugador")]
        public Camera playerCamera;

        public static Action disparoUsuario;

        void Start()
        {
            balasRestantes = maxBalas;
            if (ammoSlider != null)
            {
                ammoSlider.minValue = 0f;
                ammoSlider.maxValue = 1f;
                ammoSlider.value = 1f; // cargador al 100%
            }
        }

        void Update()
        {
            // Si estamos recargando, dejamos que la coroutine actualice el slider
            if (isRecargando)
                return;

            // Mientras no recargue, el slider refleja la munición restante
            if (ammoSlider != null)
                ammoSlider.value = balasRestantes / (float)maxBalas;

            float tiempoPasado = Time.time - ultimoDisparo;

            if (Input.GetKeyDown(KeyCode.Space) && tiempoPasado >= tiempoEnfriamiento)
            {
                if (balasRestantes > 0)
                {
                    Disparar();
                    balasRestantes--;
                    ultimoDisparo = Time.time;

                    if (balasRestantes <= 0)
                        StartCoroutine(RecargarAutomatica());
                }
            }
        }

        void Disparar()
        {
            GameObject bala = Instantiate(proyectil, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = bala.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;  // No caiga a plomo, va en línea recta
                rb.linearVelocity = playerCamera.transform.forward * potencia;
            }
            disparoUsuario?.Invoke();
        }

        private IEnumerator RecargarAutomatica()
        {
            isRecargando = true;
            
            var arma = DataUsuario.armaActual;
            var nivel = DataUsuario.nivelesCadencia[arma];
            
            var factor = 1f + nivel * 0.1f;
            var tiempoAjustado = Mathf.Max(0.1f, tiempoRecarga / factor);
            
            var duracionRecarga = 0f;

            // Slider muestra progreso de 0 → 1 durante la recarga
            while (duracionRecarga < tiempoAjustado)
            {
                duracionRecarga += Time.deltaTime;
                if (ammoSlider != null)
                    ammoSlider.value = Mathf.Clamp01(duracionRecarga / tiempoRecarga);
                yield return null;
            }

            // Al terminar, recargamos el cargador y marcamos listo
            balasRestantes = maxBalas;
            if (ammoSlider != null)
                ammoSlider.value = 1f;

            isRecargando = false;
        }
    }
}
