using Armeria;
using TMPro;
using UnityEngine;

namespace Funciones.Partida
{
    public class ArmaManager : MonoBehaviour
    
    {
        [Header("UI del arma en partida")]
        public GameObject [] armasDeljugador;
        public TextMeshProUGUI nombreArma;
        public TextMeshProUGUI tipoArma;

        //Recorremos todas las armas y las desactivamos todas para no ver ningún arma.
        void Awake()
        {
            for (int i = 0; i < armasDeljugador.Length; i++)
            {
                armasDeljugador[i].SetActive(false);
            }
        }

        //Al Start() ir después del Awake() ponemos en visible el arma seleccionada,
        //Sabemos cuál es porque lo tenemos guardado en PartidaManager.
        //Y adaptamos el texto al arma (Nombre, tipo y clasificación del arma).
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