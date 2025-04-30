using System;
using Funciones.Partida;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Armas
{
    public class ImpactoCanon : MonoBehaviour
    {
        //Los enemigos se dividen por las 3 clases que hay diferenciadas de enemigos.
        //Según el tipo el "switch" diferencia y clasifica la colisión.
        private void OnCollisionEnter(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Enemigo_Pesado":
                {
                    EnemigoDefecto estadisticasPorDefecto = collision.gameObject.GetComponent<EnemigoDefecto>();
                    if (estadisticasPorDefecto != null)
                    {
                        estadisticasPorDefecto.recibirImpacto(50);
                        print(50);
                    }
                }
                    break;

                case "Enemigo_Estandar":
                {
                    EnemigoDefecto estadisticasPorDefecto = collision.gameObject.GetComponent<EnemigoDefecto>();
                    if (estadisticasPorDefecto != null)
                    {
                        estadisticasPorDefecto.recibirImpacto(75);
                        print(75);
                    }   
                }
                    break;

                case "Enemigo_Furtivo":
                {
                    EnemigoDefecto estadisticasPorDefecto = collision.gameObject.GetComponent<EnemigoDefecto>();
                    if (estadisticasPorDefecto != null)
                    {
                        estadisticasPorDefecto.recibirImpacto(100);
                        print(100);
                    }
                }
                    break;
            }
        }
    }
}