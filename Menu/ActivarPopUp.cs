using UnityEngine;
using UnityEngine.UI;

namespace Menu {
    public class ActivarPopUp : MonoBehaviour
    {
        public GameObject panelAjustes;
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
            if (panelAjustes.activeSelf)
            {
                panelAjustes.SetActive(false);
                animacion.enabled = true;
            }
        }
        
    }
}
    