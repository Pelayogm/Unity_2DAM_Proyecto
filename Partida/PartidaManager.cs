using System;
using UnityEngine;

namespace Funciones.Partida
{
    public class PartidaManager : MonoBehaviour
    {
        //Singleton de la Partida
        public static PartidaManager instance;
        
        //Posición del arma que se está usando en esa partida en concreto.
        public int armaSeleccionada;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}