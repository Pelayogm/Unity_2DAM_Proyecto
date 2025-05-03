using System;
using System.Collections;
using Armeria;
using UnityEngine;
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
        public float dispersionAngle = 10f;
        public int pelletsPorDisparo = 3;

        [Header("Cargador")]
        public int maxCartuchos = 3;
        private int cartuchosRestantes;
        public float tiempoRecarga = 5f;
        private bool isRecargando = false;

        [Header("UI")]
        public Slider ammoSlider;

        [Header("Cámara del jugador")]
        public Camera playerCamera;

        public static Action disparoUsuario;

        private float ultimoDisparo = -Mathf.Infinity;

        void Start()
        {
            // Inicializar cargador y slider
            cartuchosRestantes = maxCartuchos;
            if (ammoSlider != null)
            {
                ammoSlider.minValue = 0f;
                ammoSlider.maxValue = 1f;
                ammoSlider.value = 1f;
            }
        }

        void Update()
        {
            // Si estamos recargando, dejar que la coroutine maneje el slider
            if (isRecargando)
                return;

            // En reposo, el slider refleja los cartuchos restantes
            if (ammoSlider != null)
                ammoSlider.value = cartuchosRestantes / (float)maxCartuchos;

            // Disparo con Space, respetando enfriamiento y munición
            if (Input.GetKeyDown(KeyCode.Space) &&
                Time.time - ultimoDisparo >= tiempoEnfriamiento &&
                cartuchosRestantes > 0)
            {
                DispararEscopeta();
            }
        }

        void DispararEscopeta()
        {
            ultimoDisparo = Time.time;
            cartuchosRestantes--;

            for (int i = 0; i < pelletsPorDisparo; i++)
            {
                // Calcular una rotación aleatoria dentro del cono de dispersión
                float yaw  = UnityEngine.Random.Range(-dispersionAngle, dispersionAngle);
                float pitch = UnityEngine.Random.Range(-dispersionAngle, dispersionAngle);
                Quaternion desviacion = Quaternion.Euler(pitch, yaw, 0f);

                // Instanciar perdigón con desviación
                GameObject bala = Instantiate(
                    proyectil,
                    spawnPoint.position,
                    spawnPoint.rotation * desviacion
                );

                Rigidbody rb = bala.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;  // Sin caída a plomo
                    rb.linearVelocity   = playerCamera.transform.forward * potencia;
                }
            }

            disparoUsuario?.Invoke();

            // Si se agotan los cartuchos, iniciar recarga automática
            if (cartuchosRestantes <= 0)
                StartCoroutine(RecargarAutomatica());
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

            // Al completar, recargar y resetear slider
            cartuchosRestantes = maxCartuchos;
            if (ammoSlider != null)
                ammoSlider.value = 1f;

            isRecargando = false;
        }
    }
}
