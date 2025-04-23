using System;
using System.Collections;
using UnityEngine;

namespace Funciones.Partida
{
    public class EnemigoDefecto : MonoBehaviour
    {
        [SerializeField] public BarraVida barraVida;
        [SerializeField] public GameObject enemigo;
        [SerializeField] private int saludInicial;
        public int salud = 100;
        
        [Range(0f, 1f)]
        public float probabilidadRegeneracion = 0.5f;
        public float maxPorcentajeCuracion = 0.4f;
        public float duracionCuracion = 5f;
        public float intervalo = 0.5f;
        private bool regenera;
        
        private bool comportamientoVariado = false;
        public Temporizador temporizador;

        void Start()
        {
            saludInicial = salud;
            regenera = UnityEngine.Random.value < probabilidadRegeneracion;
        }

        void Update()
        {
            float tiempoActual = temporizador.getTiempoRestante();

            if (tiempoActual < 40 && comportamientoVariado == false)
            {
                if (regenera)
                {
                    comportamientoVariado = true;
                    StartCoroutine(CurarProgresivo());
                }
            }
        }
        
        private IEnumerator CurarProgresivo()
        {
            float porcentaje = UnityEngine.Random.Range(0f, maxPorcentajeCuracion);
            int cantidadTotal = Mathf.CeilToInt(salud * porcentaje);

            int pasos = Mathf.CeilToInt(duracionCuracion / intervalo);
            int porPaso = Mathf.CeilToInt((float)cantidadTotal / pasos);

            int curadoAcumulado = 0;

            for (int i = 0; i < pasos && curadoAcumulado < cantidadTotal; i++)
            {
                yield return new WaitForSeconds(intervalo);
                int restante = cantidadTotal - curadoAcumulado;
                int curacionTick = Mathf.Min(porPaso, restante);
                salud = Mathf.Min(salud + curacionTick, saludInicial);
                curadoAcumulado += curacionTick;
                barraVida.Curar(curacionTick);
            }
        }

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