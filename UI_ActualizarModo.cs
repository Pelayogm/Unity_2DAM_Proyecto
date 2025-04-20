using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Funciones
{
    public class UI_ActualizarModo : MonoBehaviour
    {
        public TextMeshProUGUI[] texto;
        public GameObject arma;
        private int indexTexto = 0;
        private float velocidad = 2f;
        private float velocidadDesaparicion;
        private bool isTransitioning = false;
        
        void Awake()
        {
            velocidadDesaparicion = 1f / velocidad;
            // Inicializa todos los textos: los desactiva y asegura que tengan alfa = 1.
            for (int i = 0; i < texto.Length; i++)
            {
                Color c = texto[i].color;
                c.a = 1f;
                texto[i].color = c;
                texto[i].enabled = false;
            }
            // Activa el primer texto si lo hay.
            if (texto.Length > 0)
            {
                texto[0].enabled = true;
            }
        }
        
        void Update()
        {
            // Solo se procesa si no hay una transición en curso.
            if (arma.activeSelf)
            {
                texto[0].enabled = true;
                if (Input.GetKeyDown(KeyCode.B) && !isTransitioning && arma.activeSelf)
                {
                    StartCoroutine(TransitionText());
                }
            }
            else
            {
                for (int i = 0; i < texto.Length; i++)
                {
                    texto[i].enabled = false;
                }
            }
        }
        
        IEnumerator TransitionText()
        {
            isTransitioning = true;
            // Obtiene el texto actual.
            TextMeshProUGUI currentText = texto[indexTexto];
            float alpha = currentText.color.a;
            
            // Fade out progresivo: disminuye alfa hasta llegar a 0.
            while (alpha > 0f)
            {
                alpha -= velocidadDesaparicion * Time.deltaTime;
                if(alpha < 0f) 
                    alpha = 0f;
                Color c = currentText.color;
                c.a = alpha;
                currentText.color = c;
                yield return null;
            }
            // Desactiva el texto actual al terminar el fade.
            currentText.enabled = false;
            
            // Actualiza el índice para el siguiente texto.
            indexTexto = (indexTexto + 1) % texto.Length;
            TextMeshProUGUI nextText = texto[indexTexto];
            // Resetea el alfa a 1 y activa el siguiente texto.
            Color nc = nextText.color;
            nc.a = 1f;
            nextText.color = nc;
            nextText.enabled = true;
            
            isTransitioning = false;
        }
    }
}

