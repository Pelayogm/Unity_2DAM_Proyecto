using System;
using System.Collections;
using UnityEngine;
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
        [Tooltip("Balas por cargador")]
        public int maxBalas = 15;
        private int balasRestantes;
        public float tiempoRecarga = 5f;

        [Header("UI")]
        public Slider ammoSlider;

        [Header("Cámara del jugador")]
        public Camera playerCamera;

        public static Action disparoUsuario;

        private bool isRecargando = false;
        private float siguienteDisparo = 0f;

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
            // Si está recargando, dejamos que la coroutine actualice el slider
            if (isRecargando)
                return;

            // Mientras no recarga, el slider muestra balas restantes
            if (ammoSlider != null)
                ammoSlider.value = balasRestantes / (float)maxBalas;

            // Disparo automático manteniendo Space
            if (Input.GetKey(KeyCode.Space) && Time.time >= siguienteDisparo)
            {
                if (balasRestantes > 0)
                {
                    Disparar();
                    balasRestantes--;
                    siguienteDisparo = Time.time + 1f / cadencia;

                    // Si se acaban, iniciar recarga
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
                rb.useGravity = false;  // trayectoria recta
                rb.linearVelocity = playerCamera.transform.forward * potencia;
            }
            disparoUsuario?.Invoke();
        }

        private IEnumerator RecargarAutomatica()
        {
            isRecargando = true;
            float elapsed = 0f;

            // Slider muestra progreso de 0 → 1 durante la recarga
            while (elapsed < tiempoRecarga)
            {
                elapsed += Time.deltaTime;
                if (ammoSlider != null)
                    ammoSlider.value = Mathf.Clamp01(elapsed / tiempoRecarga);
                yield return null;
            }

            // Al completar, reponer balas y resetear slider
            balasRestantes = maxBalas;
            if (ammoSlider != null)
                ammoSlider.value = 1f;

            isRecargando = false;
            // Permitir disparar de nuevo
            siguienteDisparo = Time.time;
        }
    }
}
