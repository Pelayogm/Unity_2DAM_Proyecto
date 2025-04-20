using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class CambiarFondo : MonoBehaviour
    {
        public Sprite [] fondos;
        private int posicionActual;
        public Image fondoActual;

        public void cambiarFondo()
        {
            posicionActual++;
            if (posicionActual >= fondos.Length)
            {
                posicionActual = 0;
            }
            fondoActual.sprite = fondos[posicionActual];
        }
    }
}
