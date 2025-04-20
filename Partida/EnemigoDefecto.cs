using UnityEngine;

namespace Funciones.Partida
{
    public class EnemigoDefecto : MonoBehaviour
    {
        public BarraVida barraVida;
        public GameObject enemigo;
        public int salud = 100;

        public void recibirImpacto(int cantidad)
        {
            print(salud);
            print(cantidad);
            if (salud > 0)
            {
                salud -= cantidad;
                
                if (barraVida != null)
                {
                    barraVida.Impactar(cantidad);
                }

                RachaDeTiros.Instance.impactoEnEnemigo();
                PuntuacionManager.Instance.aumentarPuntuacion(25);
                
            }
            
            if (salud <= 0)
            {
                desactivar();
                RachaDeTiros.Instance.eliminacion();
                PuntuacionManager.Instance.aumentarPuntuacion(100);
            }
            
        }

        public void desactivar()
        {
            Destroy(enemigo);
        }
    }
}