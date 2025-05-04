using UnityEngine;
using System;
using Armeria;
using Funciones.Partida;

namespace Armas
{
    public class Municion : MonoBehaviour
    {
        [Header("Parametros de la bala")]
        float tiempoVida = 1.1f;
        float gravedad = 2f;
        private Rigidbody rb;
        private bool haChocado = false;

        void Awake()
        {
            Destroy(gameObject, tiempoVida);
        }
        
        void OnCollisionEnter(Collision collision)
        {
            //Control para evitar dobles colisiones
            if (haChocado) return;
            haChocado = true;
            GetComponent<Collider>().enabled = false;
            
            //Cogemos el nivel de potencia del arma, para multiplicarlo al valor fijo
            //que infligimos a los enemigos.
            int arma = DataUsuario.armaActual;
            int nivelActual = DataUsuario.nivelesPotencia[arma];
            
            //Diferenciamos las colisiones por los "tag" de los enemigos, lo que conocemos como
            //clases.
            switch (collision.gameObject.tag)
            {
                //Diferenciamos por el tag y cogemos su componente EnemigoDefecto donde están sus estadísticas.
                //Y las editamos según el nivel y el tipo del enemigo.
                //Por último eliminamos la munición, para evitar duplicaciones en la escena.
                case "Enemigo_Pesado":
                {
                    EnemigoDefecto estadisticasPorDefecto = collision.gameObject.GetComponent<EnemigoDefecto>();
                    if (estadisticasPorDefecto != null)
                    {
                        estadisticasPorDefecto.recibirImpacto(25 * nivelActual);
                        Destroy(gameObject, 1f);
                        //print("Le has pegao 50");
                    }
                    break;
                }
                    

                case "Enemigo_Estandar":
                {
                    EnemigoDefecto estadisticasPorDefecto = collision.gameObject.GetComponent<EnemigoDefecto>();
                    if (estadisticasPorDefecto != null)
                    {
                        estadisticasPorDefecto.recibirImpacto(35 * nivelActual);
                        Destroy(gameObject, 1f);
                        //print("Le has pegao 75");
                    }
                    break;
                }
                
                case "Enemigo_Furtivo":
                {
                    EnemigoDefecto estadisticasPorDefecto = collision.gameObject.GetComponent<EnemigoDefecto>();
                    if (estadisticasPorDefecto != null)
                    {
                        estadisticasPorDefecto.recibirImpacto(45 * nivelActual);
                        //print("Le has pegao 100");
                        Destroy(gameObject, 1f);
                    }
                    break;
                }

                case "Escenario":
                {
                    print("Le has pegado al escenario");
                    Destroy(gameObject,1f);
                    RachaDeTiros.Instance.reiniciarRacha();
                    break;
                }

                default:
                {
                    print("Le has pegado a la barra de vida");
                    break;
                }
            }
        }
    }
}
