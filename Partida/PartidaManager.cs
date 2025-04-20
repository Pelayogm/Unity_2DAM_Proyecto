using System;
using UnityEngine;

namespace Funciones.Partida
{
    public class PartidaManager : MonoBehaviour
    {
        public static PartidaManager instance;
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