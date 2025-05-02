using System;
using System.Collections;
using UnityEngine;
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
        public Slider ammoSlider;

        [Header("Cámara del jugador")]
        public Camera playerCamera;

        public static Action disparoUsuario;

        private bool isRecargando = false;
        private bool isDisparandoRafaga = false;
        private float ultimoRafaga = -Mathf.Infinity;

        void Start()
        {
            balasRestantes = maxBalas;
            if (ammoSlider != null)
            {
                ammoSlider.minValue = 0f;
                ammoSlider.maxValue = 1f;
                ammoSlider.value = 1f;
            }
        }

        void Update()
        {
            // Mientras recarga o dispara ráfaga, manejamos los sliders internamente
            if (isRecargando || isDisparandoRafaga)
                return;

            // Slider refleja balas restantes
            if (ammoSlider != null)
                ammoSlider.value = balasRestantes / (float)maxBalas;

            // Disparo de ráfaga
            if (Input.GetKeyDown(KeyCode.Space) &&
                Time.time - ultimoRafaga >= tiempoEntreRafagas &&
                balasRestantes > 0)
            {
                StartCoroutine(DispararRafaga());
            }
        }

        private IEnumerator DispararRafaga()
        {
            isDisparandoRafaga = true;

            int burstSize = Mathf.Min(3, balasRestantes);
            for (int i = 0; i < burstSize; i++)
            {
                // Instanciar y disparar
                var bala = Instantiate(proyectil, spawnPoint.position, spawnPoint.rotation);
                var rb = bala.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;         // trayectoria recta
                    rb.linearVelocity = playerCamera.transform.forward * potencia;
                }
                disparoUsuario?.Invoke();

                balasRestantes--;
                if (ammoSlider != null)
                    ammoSlider.value = balasRestantes / (float)maxBalas;

                // Esperar antes del siguiente tiro dentro de la ráfaga
                yield return new WaitForSeconds(tiempoEntreBalas);
            }

            ultimoRafaga = Time.time;
            isDisparandoRafaga = false;

            // Si agotamos, empezamos la recarga automática
            if (balasRestantes <= 0)
                StartCoroutine(RecargarAutomatica());
        }

        private IEnumerator RecargarAutomatica()
        {
            isRecargando = true;
            float elapsed = 0f;

            // Slider muestra progreso de recarga (0 → 1)
            while (elapsed < tiempoRecarga)
            {
                elapsed += Time.deltaTime;
                if (ammoSlider != null)
                    ammoSlider.value = Mathf.Clamp01(elapsed / tiempoRecarga);
                yield return null;
            }

            balasRestantes = maxBalas;
            if (ammoSlider != null)
                ammoSlider.value = 1f;

            isRecargando = false;
        }
    }
}
