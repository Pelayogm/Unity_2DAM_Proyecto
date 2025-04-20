using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Funciones
{
    public class CameraTransition : MonoBehaviour
    {
        [SerializeField] private Camera camera1;    // Cámara principal del usuario
        [SerializeField] private Camera camera2;    // Cámara secundaria para la transición
        [SerializeField] private Image fadeOverlay;   // Imagen que cubre la pantalla para el fade
        [SerializeField] private float transitionDuration = 1f;  // Duración del fade (en segundos)
        [SerializeField] private float displayTime = 1f; // Tiempo que se muestra la cámara secundaria

        private bool isTransitioning = false;

        void Start()
        {
            // La cámara principal siempre se mantiene activa
            camera1.gameObject.SetActive(true);
            // La cámara secundaria se desactiva al inicio
            camera2.gameObject.SetActive(false);
            
            // Configuramos el overlay para que comience completamente transparente
            if (fadeOverlay != null)
            {
                Color c = fadeOverlay.color;
                c.a = 0f;
                fadeOverlay.color = c;
            }
        }

        void Update()
        {
            // Por ejemplo, con la tecla T se inicia la transición
            if (Input.GetKeyDown(KeyCode.T) && !isTransitioning)
            {
                StartCoroutine(SwitchCamera2AndBack());
            }
        }

        IEnumerator SwitchCamera2AndBack()
        {
            isTransitioning = true;
            float t = 0f;
            
            // Fade in: de transparente a negro
            while (t < transitionDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, t / transitionDuration);
                if (fadeOverlay != null)
                {
                    Color c = fadeOverlay.color;
                    c.a = alpha;
                    fadeOverlay.color = c;
                }
                yield return null;
            }
            
            // Activar la cámara secundaria (la cual se mostrará sobre la principal)
            camera2.gameObject.SetActive(true);
            
            // Fade out: de negro a transparente para revelar la cámara secundaria
            t = 0f;
            while (t < transitionDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, t / transitionDuration);
                if (fadeOverlay != null)
                {
                    Color c = fadeOverlay.color;
                    c.a = alpha;
                    fadeOverlay.color = c;
                }
                yield return null;
            }
            
            // Se muestra la cámara secundaria durante un tiempo determinado
            yield return new WaitForSeconds(displayTime);
            
            // Volvemos a hacer fade in: de transparente a negro para cubrir el cambio
            t = 0f;
            while (t < transitionDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, t / transitionDuration);
                if (fadeOverlay != null)
                {
                    Color c = fadeOverlay.color;
                    c.a = alpha;
                    fadeOverlay.color = c;
                }
                yield return null;
            }
            
            // Desactivar la cámara secundaria (la cámara principal sigue activa)
            camera2.gameObject.SetActive(false);
            
            // Finalmente, hacemos fade out para volver a la transparencia
            t = 0f;
            while (t < transitionDuration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, t / transitionDuration);
                if (fadeOverlay != null)
                {
                    Color c = fadeOverlay.color;
                    c.a = alpha;
                    fadeOverlay.color = c;
                }
                yield return null;
            }
            
            isTransitioning = false;
        }
    }
}
