using Armeria;
using TMPro;
using UnityEngine;

namespace Funciones.Partida
{
    public class ArmaManager : MonoBehaviour
    
    {
        public GameObject [] armasDeljugador;
        public TextMeshProUGUI nombreArma;
        public TextMeshProUGUI tipoArma;

        void Awake()
        {
            for (int i = 0; i < armasDeljugador.Length; i++)
            {
                armasDeljugador[i].SetActive(false);
            }
        }

        void Start()
        {
            var armaEscogida = PartidaManager.instance.armaSeleccionada;
            print(armaEscogida);
            DataUsuario.armaActual = armaEscogida;
            if (armaEscogida >= 0 && armaEscogida < armasDeljugador.Length)
            {
                armasDeljugador[armaEscogida].SetActive(true);
                nombreArma.text = DataUsuario.nombresArmas[armaEscogida];
                tipoArma.text = DataUsuario.clasesArmas[armaEscogida];
            }
        }
    }
}