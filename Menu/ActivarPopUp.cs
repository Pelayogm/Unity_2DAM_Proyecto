using System.Collections;
using Funciones.BBDD;
using UnityEngine;
using UnityEngine.UI;

namespace Menu {
    public class ActivarPopUp : MonoBehaviour
    {
        [Header("GameObjects de PopUps de interfaz")]
        public GameObject panelAjustes;
        public GameObject panelAjustes_2;
        public GameObject panelClasificacion;
        public GameObject tutorial;
        
        [Header("Animación de las olas del fondo")]
        public Animator animacion;

        void Start()
        {
            panelAjustes.SetActive(false);
        }

        public void activarAjustes()
        {
                if (!panelAjustes.activeSelf)
                {
                    panelAjustes.SetActive(true);
                    animacion.enabled = false;
                }
        }
        
        public void cerrarAjustes()
        {
            if (panelAjustes.activeSelf || panelAjustes_2.activeSelf)
            {
                panelAjustes.SetActive(false);
                panelAjustes_2.SetActive(false);
                animacion.enabled = true;
            }
        }

        public void avanzarPestana()
        {
            if (panelAjustes.activeSelf)
            {
                panelAjustes.SetActive(false);
                panelAjustes_2.SetActive(true);
            }
        }

        public void retrocederPestana()
        {
            if (panelAjustes_2.activeSelf)
            {
                panelAjustes.SetActive(true);
                panelAjustes_2.SetActive(false);
            }
        }

        public void abrirClasificacion()
        {
            if (!panelClasificacion.activeSelf)
            {
                panelClasificacion.SetActive(true);
                animacion.enabled = false;

                StartCoroutine(InvokeNextFrame(() =>
                {
                    BBDDManager.Instance.mostrarPuntuaciones();
                }));
            }
        }

// Helper coroutine
        private IEnumerator InvokeNextFrame(System.Action a)
        {
            yield return null;
            a();
        }

        
        public void cerrarClasificacion()
        {
            if (panelClasificacion.activeSelf)
            {
                panelClasificacion.SetActive(false);
                animacion.enabled = true;
            }
        }
        
        public void abrirTutorial()
        {
            if (!tutorial.activeSelf)
            {
                tutorial.SetActive(true);
            }
        }
        
        public void cerrarTutorial()
        {
            if (tutorial.activeSelf)
            {
                tutorial.SetActive(false);
            }
        }
        
    }
}
    