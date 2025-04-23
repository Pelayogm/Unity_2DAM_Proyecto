using UnityEngine;
using UnityEngine.UI;

namespace Menu {
    public class ActivarPopUp : MonoBehaviour
    {
        public GameObject panelAjustes;
        public GameObject panelAjustes_2;
        public GameObject panelClasificacion;
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
            }
        }
        
        public void cerrarClasificacion()
        {
            if (panelClasificacion.activeSelf)
            {
                panelClasificacion.SetActive(false);
                animacion.enabled = true;
            }
        }
        
    }
}
    