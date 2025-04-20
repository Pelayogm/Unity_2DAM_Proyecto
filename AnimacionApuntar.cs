using UnityEngine;

namespace Funciones
{
    public class AnimacionApuntar :MonoBehaviour
    {
        public GameObject arma;
        
        void Start()
        {
        
        }
        void Update()
        {
            if(Input.GetMouseButtonDown(1))
            {
                arma.GetComponent<Animator>().Play("FusilAnimacion");
            }

            if(Input.GetMouseButtonUp(1))
            {
                arma.GetComponent<Animator>().Play("New State");
            }
        }
    }
}
